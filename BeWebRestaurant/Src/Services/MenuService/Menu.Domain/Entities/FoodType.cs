using Domain.Core.Base;
using Menu.Domain.Events.FoodTypeEvents;
using Menu.Domain.ValueObjects.FoodType;

namespace Menu.Domain.Entities
{
    public sealed class FoodType : AggregateRoot
    {
        public FoodTypeName FoodTypeName { get; private set; } = default!;

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        private FoodType()
        { }

        private FoodType(Guid id, FoodTypeName foodTypeName)
            : base(id)
        {
            FoodTypeName = foodTypeName;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static FoodType Create(FoodTypeName foodTypeName)
        {
            var entity = new FoodType(Guid.NewGuid(), foodTypeName);
            entity.AddDomainEvent(new FoodTypeCreatedEvent(entity.Id, entity.FoodTypeName));
            return entity;
        }

        public void UpdateName(FoodTypeName foodTypeName)
        {
            if (FoodTypeName == foodTypeName) return;
            FoodTypeName = foodTypeName;
            Touch();
            AddDomainEvent(new FoodTypeUpdatedEvent(Id, FoodTypeName.Value, UpdatedAt));
        }

        public void Delete()
        {
            Touch();
            AddDomainEvent(new FoodTypeDeletedEvent(Id, UpdatedAt));
        }

        private void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
