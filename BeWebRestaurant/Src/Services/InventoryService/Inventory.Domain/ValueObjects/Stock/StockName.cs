using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.ValueObjects;
using Inventory.Domain.Common.Factories.Rule;

namespace Inventory.Domain.ValueObjects.Stock
{
    public sealed class StockName : NameBase
    {
        private StockName(string value) : base(value)
        {
        }

        public static StockName Create(string value)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                StockRuleFactory.NameMaxLength(value),
                StockRuleFactory.NameNotEmpty(value)
            });
            return new StockName(value);
        }
    }
}