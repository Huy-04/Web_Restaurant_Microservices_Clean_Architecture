using Domain.Core.Interface.Request;
using Domain.Core.Rule;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Behaviors
{
    public sealed class ValidationBehaviors<TRequest, TResponse>
       : IPipelineBehavior<TRequest, TResponse> where TRequest : IValidateRequest
    {
        private readonly ILogger<ValidationBehaviors<TRequest, TResponse>> _logger;

        public ValidationBehaviors(ILogger<ValidationBehaviors<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next, CancellationToken token)
        {
            _logger.LogInformation($"Validating {typeof(TRequest).Name}");

            try
            {
                RuleValidator.CheckRules(request.GetRule());
                _logger.LogInformation($"Validation succeeded for {typeof(TRequest).Name}");
                return await next();
            }
            catch
            {
                _logger.LogWarning($"Failed to execute {typeof(TRequest).Name}");
                throw;
            }
        }
    }
}