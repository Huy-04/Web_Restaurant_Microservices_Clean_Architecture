using Domain.Core.Interface.Rule;
using Domain.Core.Rule.StringRule;
using Menu.Domain.Common.Messages.FieldNames;

namespace Menu.Domain.Common.Factories.Rules
{
    public static class FoodTypeRuleFactory
    {
        // FoodTypeName
        public static IBusinessRule NameMaxLength(string value)
        {
            return new StringMaxLength(value, 50, FoodTypeField.FoodTypeName);
        }

        public static IBusinessRule NameNotEmpty(string value)
        {
            return new StringNotEmpty(value, FoodTypeField.FoodTypeName);
        }
    }
}