using Domain.Core.Interface.Event;

namespace Menu.Domain.Events.FoodEvents
{
    public class FoodDeletedEvent : IDomainEvent
    {
        public Guid IdFood { get; }
        public DateTimeOffset DeletedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodDeletedEvent(Guid idFood, DateTimeOffset deletedAt)
        {
            IdFood = idFood;
            DeletedAt = deletedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
