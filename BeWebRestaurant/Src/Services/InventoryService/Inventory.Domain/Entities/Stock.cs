using Domain.Core.Base;
using Domain.Core.ValueObjects;
using Inventory.Domain.Events.StockEvents;
using Inventory.Domain.ValueObjects.Stock;

namespace Inventory.Domain.Entities
{
    public sealed class Stock : AggregateRoot
    {
        public Description Description { get; private set; }

        public StockName StockName { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

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
            AddDomainEvent(new StockUpdatedEvent(Id, UpdatedAt));
        }

        public void Delete()
        {
            Touch();
            AddDomainEvent(new StockDeletedEvent(Id, UpdatedAt));
        }

        private void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}