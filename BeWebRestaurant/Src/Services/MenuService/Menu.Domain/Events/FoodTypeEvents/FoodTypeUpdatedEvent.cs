using Domain.Core.Interface.Event;

namespace Menu.Domain.Events.FoodTypeEvents
{
    public class FoodTypeUpdatedEvent : IDomainEvent
    {
        public Guid IdFoodType { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodTypeUpdatedEvent(Guid idFoodType, DateTimeOffset updatedAt)
        {
            IdFoodType = idFoodType;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
