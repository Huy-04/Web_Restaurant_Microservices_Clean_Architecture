using Domain.Core.Base;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.ValueObjects;
using Inventory.Domain.Common.Factories.Rule;
using Inventory.Domain.Enums;
using Inventory.Domain.Events.StockItemsEvents;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Domain.Entities
{
    public sealed class StockItems : AggregateRoot
    {
        public Guid StockId { get; private set; }

        public Guid IngredientsId { get; private set; }

        public Measurement Measurement { get; private set; }

        public Capacity Capacity { get; private set; }

        public StockItemsStatus StockItemsStatus { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        // for orm
        private StockItems()
        {
        }

        private StockItems(Guid id, Guid stockId, Guid ingredientsId, Measurement measurement, Capacity capacity, StockItemsStatus stockItemsStatus)
            : base(id)
        {
            IngredientsId = ingredientsId;
            StockId = stockId;
            Measurement = measurement;
            Capacity = capacity;
            StockItemsStatus = stockItemsStatus;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static StockItems Create(Guid StockId, Guid ingredientsId, Capacity capacity, UnitEnum unit)
        {
            var entity = new StockItems(Guid.NewGuid(), StockId, ingredientsId, Measurement.Create(0, unit), capacity, StockItemsStatus.Create(StockItemsStatusEnum.OutOfStock));
            entity.AddDomainEvent(new StockItemsCreatedEvent(entity.Id, entity.StockId, entity.IngredientsId));
            return entity;
        }

        public void IncreaseMeasurement(Measurement delta)
        {
            var newMeasurement = Measurement + delta;
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                StockItemsRuleFactory.ExceedCapacity(newMeasurement,Capacity)
            });
            Measurement = newMeasurement;
            EvaluateStatus();
            Touch();
            AddDomainEvent(new StockItemsUpdatedEvent(Id, UpdatedAt));
        }

        public void DecreaseMeasurement(Measurement delta)
        {
            Measurement = Measurement - delta;
            EvaluateStatus();
            Touch();
            AddDomainEvent(new StockItemsUpdatedEvent(Id, UpdatedAt));
        }

        public void UpdateMeasurement(Measurement measurement)
        {
            if (Measurement == measurement) return;
            var newMeasurement = measurement;
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                StockItemsRuleFactory.ExceedCapacity(newMeasurement,Capacity)
            });
            Measurement = newMeasurement;
            EvaluateStatus();
            Touch();
            AddDomainEvent(new StockItemsUpdatedEvent(Id, UpdatedAt));
        }

        public void UpdateCapacity(Capacity capacity)
        {
            if (Capacity == capacity) return;
            Capacity = capacity;
            EvaluateStatus();
            Touch();
            AddDomainEvent(new StockItemsUpdatedEvent(Id, UpdatedAt));
        }

        public void UpdateStatus(StockItemsStatus stockItemsStatus)
        {
            if (StockItemsStatus == stockItemsStatus) return;
            StockItemsStatus = stockItemsStatus;
            Touch();
            AddDomainEvent(new StockItemsUpdateStatusEvent(Id, StockItemsStatus, UpdatedAt));
            AddDomainEvent(new StockItemsUpdatedEvent(Id, UpdatedAt));
        }

        public void UpdateIngredientsId(Guid ingredientsId)
        {
            if (IngredientsId == ingredientsId) return;
            IngredientsId = ingredientsId;
            Touch();
            AddDomainEvent(new StockItemsUpdatedEvent(Id, UpdatedAt));
        }

        public void UpdateStockId(Guid stockId)
        {
            if (StockId == stockId) return;
            StockId = stockId;
            Touch();
            AddDomainEvent(new StockItemsUpdatedEvent(Id, UpdatedAt));
        }

        public void Delete()
        {
            Touch();
            AddDomainEvent(new StockItemsDeletedEvent(Id, UpdatedAt));
        }

        public void OutOfStock() => UpdateStatus(StockItemsStatus.Create(StockItemsStatusEnum.OutOfStock));

        public void Available() => UpdateStatus(StockItemsStatus.Create(StockItemsStatusEnum.Available));

        public void LowStock() => UpdateStatus(StockItemsStatus.Create(StockItemsStatusEnum.LowStock));

        public void Restocking() => UpdateStatus(StockItemsStatus.Create(StockItemsStatusEnum.Restocking));

        private void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }

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