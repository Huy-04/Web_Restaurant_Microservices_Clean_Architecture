using Domain.Core.Interface.Rule;
using Domain.Core.Rule.EnumRule;
using Domain.Core.Rule.StringRule;
using Menu.Domain.Common.Messages.FieldNames;
using Menu.Domain.Enums;

namespace Menu.Domain.Common.Factories.Rules
{
    public static class FoodRuleFactory
    {
        // FoodName
        public static IBusinessRule NameMaxLength(string value)
        {
            return new StringMaxLength(value, 50, FoodField.FoodName);
        }

        public static IBusinessRule NameNotEmpty(string value)
        {
            return new StringNotEmpty(value, FoodField.FoodName);
        }

        // FoodStatus
        public static IBusinessRule FoodStatusValidate(FoodStatusEnum foodstatus)
        {
            var validate = Enum.GetValues(typeof(FoodStatusEnum)).Cast<FoodStatusEnum>().ToList();
            return new EnumValidateRule<FoodStatusEnum>(foodstatus, validate, FoodField.FoodStatus);
        }
    }
}