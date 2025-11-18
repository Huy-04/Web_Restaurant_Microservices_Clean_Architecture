using Domain.Core.Interface.Event;

namespace Menu.Domain.Events.FoodTypeEvents
{
    public class FoodTypeDeletedEvent : IDomainEvent
    {
        public Guid IdFoodType { get; }
        public DateTimeOffset DeletedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodTypeDeletedEvent(Guid idFoodType, DateTimeOffset deletedAt)
        {
            IdFoodType = idFoodType;
            DeletedAt = deletedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
