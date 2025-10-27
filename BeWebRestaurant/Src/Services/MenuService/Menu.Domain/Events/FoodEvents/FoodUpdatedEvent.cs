using Domain.Core.Interface.Event;

namespace Menu.Domain.Events.FoodEvents
{
    // Raised when any non-status property on Food changes
    public class FoodUpdatedEvent : IDomainEvent
    {
        public Guid IdFood { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodUpdatedEvent(Guid idFood, DateTimeOffset updatedAt)
        {
            IdFood = idFood;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
