using Domain.Core.Base;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.ValueObjects;
using Inventory.Domain.Common.Factories.Rule;
using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Domain.Entities
{
    public sealed class StockItems : Entity
    {
        // Foreign key to parent aggregate - managed by EF Core
        public Guid StockId { get; private set; }

        public Guid IngredientsId { get; private set; }

        public Measurement Measurement { get; private set; } = default!;

        public Capacity Capacity { get; private set; } = default!;

        public StockItemsStatus StockItemsStatus { get; private set; } = default!;

        // for orm
        private StockItems()
        {
        }

        internal StockItems(Guid id, Guid stockId, Guid ingredientsId, Measurement measurement, Capacity capacity, StockItemsStatus stockItemsStatus)
            : base(id)
        {
            StockId = stockId;
            IngredientsId = ingredientsId;
            Measurement = measurement;
            Capacity = capacity;
            StockItemsStatus = stockItemsStatus;
        }

        internal void IncreaseMeasurement(Measurement delta)
        {
            var newMeasurement = Measurement + delta;
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                StockItemsRuleFactory.ExceedCapacity(newMeasurement, Capacity)
            });
            Measurement = newMeasurement;
            EvaluateStatus();
        }

        internal void DecreaseMeasurement(Measurement delta)
        {
            Measurement = Measurement - delta;
            EvaluateStatus();
        }

        internal void UpdateMeasurement(Measurement measurement)
        {
            if (Measurement == measurement) return;
            var newMeasurement = measurement;
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                StockItemsRuleFactory.ExceedCapacity(newMeasurement, Capacity)
            });
            Measurement = newMeasurement;
            EvaluateStatus();
        }

        internal void UpdateCapacity(Capacity capacity)
        {
            if (Capacity == capacity) return;
            Capacity = capacity;
            EvaluateStatus();
        }

        internal void UpdateStatus(StockItemsStatus stockItemsStatus)
        {
            if (StockItemsStatus == stockItemsStatus) return;
            StockItemsStatus = stockItemsStatus;
        }

        internal void UpdateIngredientsId(Guid ingredientsId)
        {
            if (IngredientsId == ingredientsId) return;
            IngredientsId = ingredientsId;
        }

        private void OutOfStock() => UpdateStatus(StockItemsStatus.Create(StockItemsStatusEnum.OutOfStock));

        private void Available() => UpdateStatus(StockItemsStatus.Create(StockItemsStatusEnum.Available));

        private void LowStock() => UpdateStatus(StockItemsStatus.Create(StockItemsStatusEnum.LowStock));

        private void Restocking() => UpdateStatus(StockItemsStatus.Create(StockItemsStatusEnum.Restocking));

        private void EvaluateStatus()
        {
            var ratio = Measurement.Quantity / Capacity.Value;

            switch (ratio)
            {
                case 0: OutOfStock(); break;
                case <= 0.20m: Restocking(); break;
                case <= 0.25m: LowStock(); break;
                default: Available(); break;
            }
        }
    }
}
