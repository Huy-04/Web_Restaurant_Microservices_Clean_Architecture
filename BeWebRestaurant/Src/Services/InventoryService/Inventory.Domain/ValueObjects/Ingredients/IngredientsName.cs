using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.ValueObjects;
using Inventory.Domain.Common.Factories.Rule;

namespace Inventory.Domain.ValueObjects.Ingredients
{
    public sealed class IngredientsName : NameBase
    {
        private IngredientsName(string value) : base(value)
        {
        }

        public static IngredientsName Create(string value)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
           {
                IngredientsRuleFactory.NameMaxLength(value),
                IngredientsRuleFactory.NameNotEmpty(value)
           });
            return new IngredientsName(value);
        }
    }
}