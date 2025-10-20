using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.ValueObjects;
using Menu.Domain.Common.Factories.Rules;

namespace Menu.Domain.ValueObjects.FoodType
{
    public sealed class FoodTypeName : NameBase
    {
        private FoodTypeName(string value) : base(value)
        {
        }

        public static FoodTypeName Create(string value)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                FoodTypeRuleFactory.NameMaxLength(value),
                FoodTypeRuleFactory.NameNotEmpty(value)
            });
            return new FoodTypeName(value);
        }
    }
}