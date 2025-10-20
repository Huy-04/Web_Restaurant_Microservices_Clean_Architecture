using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Inventory.Domain.Common.Factories.Rule;

namespace Inventory.Domain.ValueObjects.StockItems
{
    public sealed class Capacity : QuantityBase<decimal, Capacity>
    {
        private Capacity(decimal value) : base(value)
        {
        }

        public static Capacity Create(decimal value)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                StockItemsRuleFactory.CapacityNotNegative(value)
            });
            return new Capacity(value);
        }

        protected override Capacity CreateCore(decimal value)
        {
            return Create(value);
        }
    }
}