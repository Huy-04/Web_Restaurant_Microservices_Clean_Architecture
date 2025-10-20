using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.EnumRule;
using Domain.Core.Rule.NumberRule;
using Domain.Core.ValueObjects;
using Inventory.Domain.Common.Messages.FieldNames;
using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Domain.Common.Factories.Rule
{
    public static class StockItemsRuleFactory
    {
        // StockItems Status
        public static IBusinessRule StockItemsStatusValidate(StockItemsStatusEnum stockItemsStatus)
        {
            var validate = Enum.GetValues(typeof(StockItemsStatusEnum)).Cast<StockItemsStatusEnum>().ToList();
            return new EnumValidateRule<StockItemsStatusEnum>(stockItemsStatus, validate, StockItemsField.StockItemsStatus);
        }

        public static IBusinessRule ExceedCapacity(Measurement measurement, Capacity capacity)
        {
            return new MaxValueLimitRule<decimal>(measurement.Quantity, capacity.Value, MeasurementField.Quantity);
        }

        public static IBusinessRule CapacityNotNegative(decimal capacity)
        {
            return new NotNegativeRule<decimal>(capacity, StockItemsField.Capacity);
        }
    }
}