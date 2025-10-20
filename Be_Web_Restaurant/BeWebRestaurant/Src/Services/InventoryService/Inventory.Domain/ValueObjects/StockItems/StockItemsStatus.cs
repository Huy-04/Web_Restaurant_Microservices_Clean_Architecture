using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.ValueObjects;
using Inventory.Domain.Common.Factories.Rule;
using Inventory.Domain.Enums;

namespace Inventory.Domain.ValueObjects.StockItems
{
    public sealed class StockItemsStatus : Status<StockItemsStatusEnum>
    {
        private StockItemsStatus(StockItemsStatusEnum stockItemsStatus) : base(stockItemsStatus)
        {
        }

        public static StockItemsStatus Create(StockItemsStatusEnum stockItemsStatusEnum)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
{
                StockItemsRuleFactory.StockItemsStatusValidate(stockItemsStatusEnum)
});
            return new StockItemsStatus(stockItemsStatusEnum);
        }

        public static implicit operator StockItemsStatusEnum(StockItemsStatus stockItemsStatus) => stockItemsStatus.Value;

        public static implicit operator StockItemsStatus(StockItemsStatusEnum stockItemsStatusEnum) => Create(stockItemsStatusEnum);
    }
}