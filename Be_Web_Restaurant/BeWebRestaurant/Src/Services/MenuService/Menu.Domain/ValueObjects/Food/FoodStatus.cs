using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.ValueObjects;
using Menu.Domain.Common.Factories.Rules;
using Menu.Domain.Enums;

namespace Menu.Domain.ValueObjects.Food
{
    public class FoodStatus : Status<FoodStatusEnum>
    {
        private FoodStatus(FoodStatusEnum foodStatus) : base(foodStatus)
        {
        }

        public static FoodStatus Create(FoodStatusEnum foodStatus)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                FoodRuleFactory.FoodStatusValidate(foodStatus)
            });
            return new FoodStatus(foodStatus);
        }

        public static implicit operator FoodStatusEnum(FoodStatus foodStatus) => foodStatus.Value;

        public static implicit operator FoodStatus(FoodStatusEnum foodStatusEnum) => Create(foodStatusEnum);
    }
}