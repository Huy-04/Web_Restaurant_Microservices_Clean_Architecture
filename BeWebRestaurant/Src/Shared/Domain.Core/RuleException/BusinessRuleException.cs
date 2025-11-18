using Domain.Core.Enums;

namespace Domain.Core.RuleException
{
    public class BusinessRuleException : Exception
    {
        public ErrorCategory ErrorCategory { get; }

        public ErrorCode ErrorCode { get; }

        public String Field { get; }

        public IReadOnlyDictionary<string, object> Parameters { get; }

        public BusinessRuleException(
            ErrorCategory errorCategory,
            string field,
            ErrorCode errorCode,
            IReadOnlyDictionary<string, object> parameters)
        {
            Field = field;
            ErrorCategory = errorCategory;
            ErrorCode = errorCode;
            Parameters = parameters;
        }
    }
}