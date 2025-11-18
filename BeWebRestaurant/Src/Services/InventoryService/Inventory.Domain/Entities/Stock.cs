using Domain.Core.Base;
using Domain.Core.Enums;
using Domain.Core.ValueObjects;
using Inventory.Domain.Enums;
using Inventory.Domain.Events.StockEvents;
using Inventory.Domain.Events.StockItemsEvents;
using Inventory.Domain.ValueObjects.Stock;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Domain.Entities
{
    public sealed class Stock : AggregateRoot
    {
        public Description Description { get; private set; } = default!;

        public StockName StockName { get; private set; } = default!;

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        // collection
        private readonly List<StockItems> _stockItems = new List<StockItems>();
        public IReadOnlyCollection<StockItems> StockItems => _stockItems.AsReadOnly();

        // for orm
        private Stock()
        {
        }

        private Stock(Guid id, StockName stockName, Description description) : base(id)
        {
            StockName = stockName;
            Description = description;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Stock Create(StockName stockName, Description description)
        {
            var entity = new Stock(Guid.NewGuid(), stockName, description);
            entity.AddDomainEvent(new StockCreatedEvent(entity.Id, entity.StockName, entity.Description));
            return entity;
        }

        public void Update(StockName stockName, Description description)
        {
            if (StockName == stockName && Description == description) return;
            StockName = stockName;
            Description = description;
            Touch();
            AddDomainEvent(new StockUpdatedEvent(Id, StockName.Value, Description.Value, UpdatedAt));
        }

        public void Delete()
        {
            Touch();
            AddDomainEvent(new StockDeletedEvent(Id, UpdatedAt));
        }

        // StockItems Management
        public void AddItem(Guid ingredientsId, Capacity capacity, UnitEnum unit)
        {
            var item = new StockItems(
                Guid.NewGuid(),
                Id,
                ingredientsId,
                Measurement.Create(0, unit),
                capacity,
                StockItemsStatus.Create(StockItemsStatusEnum.OutOfStock)
            );
            _stockItems.Add(item);
            Touch();
            AddDomainEvent(new StockItemAddedEvent(Id, item.Id, ingredientsId));
        }

        public void RemoveItem(Guid itemId)
        {
            var item = _stockItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null) return;

            _stockItems.Remove(item);
            Touch();
            AddDomainEvent(new StockItemRemovedEvent(Id, itemId));
        }

        public void IncreaseItemQuantity(Guid itemId, Measurement delta)
        {
            var item = _stockItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null) return;

            item.IncreaseMeasurement(delta);
            Touch();
            AddDomainEvent(new StockItemsUpdatedEvent(
                itemId, Id, item.IngredientsId,
                item.Measurement.Quantity,
                item.Measurement.Unit.ToString(),
                item.Capacity.Value,
                item.StockItemsStatus.Value.ToString(),
                UpdatedAt
            ));
        }

        public void DecreaseItemQuantity(Guid itemId, Measurement delta)
        {
            var item = _stockItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null) return;

            item.DecreaseMeasurement(delta);
            Touch();
            AddDomainEvent(new StockItemsUpdatedEvent(
                itemId, Id, item.IngredientsId,
                item.Measurement.Quantity,
                item.Measurement.Unit.ToString(),
                item.Capacity.Value,
                item.StockItemsStatus.Value.ToString(),
                UpdatedAt
            ));
        }

        public void UpdateItemMeasurement(Guid itemId, Measurement measurement)
        {
            var item = _stockItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null) return;

            var oldMeasurement = item.Measurement;
            item.UpdateMeasurement(measurement);

            if (oldMeasurement != measurement)
            {
                Touch();
                AddDomainEvent(new StockItemsUpdatedEvent(
                    itemId, Id, item.IngredientsId,
                    item.Measurement.Quantity,
                    item.Measurement.Unit.ToString(),
                    item.Capacity.Value,
                    item.StockItemsStatus.Value.ToString(),
                    UpdatedAt
                ));
            }
        }

        public void UpdateItemCapacity(Guid itemId, Capacity capacity)
        {
            var item = _stockItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null) return;

            var oldCapacity = item.Capacity;
            item.UpdateCapacity(capacity);

            if (oldCapacity != capacity)
            {
                Touch();
                AddDomainEvent(new StockItemsUpdatedEvent(
                    itemId, Id, item.IngredientsId,
                    item.Measurement.Quantity,
                    item.Measurement.Unit.ToString(),
                    item.Capacity.Value,
                    item.StockItemsStatus.Value.ToString(),
                    UpdatedAt
                ));
            }
        }

        public void UpdateItemIngredient(Guid itemId, Guid ingredientsId)
        {
            var item = _stockItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null) return;

            var oldIngredientsId = item.IngredientsId;
            item.UpdateIngredientsId(ingredientsId);

            if (oldIngredientsId != ingredientsId)
            {
                Touch();
                AddDomainEvent(new StockItemsUpdatedEvent(
                    itemId, Id, item.IngredientsId,
                    item.Measurement.Quantity,
                    item.Measurement.Unit.ToString(),
                    item.Capacity.Value,
                    item.StockItemsStatus.Value.ToString(),
                    UpdatedAt
                ));
            }
        }

        private void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}