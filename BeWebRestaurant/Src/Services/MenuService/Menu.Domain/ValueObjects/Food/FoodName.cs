using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.ValueObjects;
using Menu.Domain.Common.Factories.Rules;

namespace Menu.Domain.ValueObjects.Food
{
    public sealed class FoodName : NameBase
    {
        private FoodName(string value) : base(value)
        {
        }

        public static FoodName Create(string value)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                FoodRuleFactory.NameMaxLength(value),
                FoodRuleFactory.NameNotEmpty(value)
            });
            return new FoodName(value);
        }
    }
}