using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.StringRule;

namespace Domain.Core.Rule.RuleFactory
{
    public static class DescriptionRuleFactory
    {
        public static IBusinessRule DescriptionMaxLength(string value)
        {
            return new StringMaxLength(value, 255, CommonField.Description);
        }

        public static IBusinessRule DescriptionNotEmpty(string value)
        {
            return new StringNotEmpty(value, CommonField.Description);
        }
    }
}