using Domain.Core.Enums;
using Domain.Core.RuleException;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Core.Middleware
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        private static readonly Dictionary<ErrorCategory, int> StatusMap = new()
        {
            [ErrorCategory.ValidationFailed] = StatusCodes.Status400BadRequest,
            [ErrorCategory.NotFound] = StatusCodes.Status404NotFound,
            [ErrorCategory.Conflict] = StatusCodes.Status409Conflict,
            [ErrorCategory.Unauthorized] = StatusCodes.Status401Unauthorized,
            [ErrorCategory.Forbidden] = StatusCodes.Status403Forbidden,
            [ErrorCategory.InternalServerError] = StatusCodes.Status500InternalServerError
        };

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessRuleException ruleEx)
            {
                _logger.LogWarning(ruleEx, "Business rule violation ({ErrorCategory})", ruleEx.ErrorCategory);

                var errorDetails = new List<CustomErrorDetail>
                {
                    ToCustomErrorDetail(ruleEx)
                };

                await WriteProblemDetailsAsync(context, ruleEx.ErrorCategory, errorDetails);
            }
            catch (MultiRuleException multiEx)
            {
                _logger.LogWarning(multiEx, "Multiple business rule violations");

                var errorDetails = multiEx.Errors
                    .Select(ToCustomErrorDetail)
                    .ToList();

                await WriteProblemDetailsAsync(context, ErrorCategory.ValidationFailed, errorDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var errorDetails = new List<CustomErrorDetail>
                {
                    new CustomErrorDetail
                    {
                        Field = "ServerError",
                        ErrorCode = "UNKNOWN_SERVER_ERROR"
                    }
                };

                await WriteProblemDetailsAsync(context, ErrorCategory.InternalServerError, errorDetails);
            }
        }

        private static CustomErrorDetail ToCustomErrorDetail(BusinessRuleException ex)
        {
            Dictionary<string, object>? parameter = null;

            if (ex.Parameters != null && ex.Parameters.Count > 0)
            {
                parameter = new Dictionary<string, object>(ex.Parameters);
            }

            return new CustomErrorDetail
            {
                Field = ex.Field,
                ErrorCode = ex.ErrorCode.ToString(),
                Parameter = parameter?.Count > 0 ? parameter : null
            };
        }

        private static async Task WriteProblemDetailsAsync(
            HttpContext context,
            ErrorCategory errorCategory,
            List<CustomErrorDetail> errorDetails)
        {
            StatusMap.TryGetValue(errorCategory, out var status);
            if (status == 0)
                status = StatusCodes.Status500InternalServerError;

            var problem = new CustomProblemDetails
            {
                Type = $"https://httpstatuses.com/{status}",
                Status = status,
                ErrorCategory = errorCategory.ToString(),
                Title = errorDetails.FirstOrDefault()?.ErrorCode ?? errorCategory.ToString(),
                Detail = errorDetails.Count > 0 ? null : errorCategory.ToString(),
                Instance = context.Request.Path,
                TraceId = context.TraceIdentifier,
                Errors = errorDetails
            };

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";

            var opts = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, opts));
        }
    }
}