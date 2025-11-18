using Domain.Core.Enums;
using Domain.Core.RuleException;

namespace Domain.Core.Rule.RuleFactory
{
    public static class RuleFactory
    {
        public static BusinessRuleException SimpleRuleException(ErrorCategory errorCategory, string field, ErrorCode errorCode, IReadOnlyDictionary<string, object> parameters)
        {
            return new BusinessRuleException(errorCategory, field, errorCode, parameters);
        }
    }
}