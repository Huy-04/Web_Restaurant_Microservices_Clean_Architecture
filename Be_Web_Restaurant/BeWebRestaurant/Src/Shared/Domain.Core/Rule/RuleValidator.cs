using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.RuleException;

namespace Domain.Core.Rule
{
    public static class RuleValidator
    {
        public static void CheckRules(IEnumerable<IBusinessRule> rules)
        {
            var broken = rules.Where(r => !r.IsSatisfied()).ToList();
            if (broken.Any())
            {
                var ex = broken.Select(r => new BusinessRuleException(
                    ErrorCategory.ValidationFailed,
                    r.Field,
                    r.Error,
                    r.Parameters));

                throw new MultiRuleException(ex);
            }
        }
    }
}