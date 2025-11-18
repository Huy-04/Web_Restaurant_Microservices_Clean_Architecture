using Domain.Core.Interface.Event;

namespace Menu.Domain.Events.FoodTypeEvents
{
    public class FoodTypeUpdatedEvent : IDomainEvent
    {
        public Guid IdFoodType { get; }
        public string FoodTypeName { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodTypeUpdatedEvent(Guid idFoodType, string foodTypeName, DateTimeOffset updatedAt)
        {
            IdFoodType = idFoodType;
            FoodTypeName = foodTypeName;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
