using Domain.Core.Interface.Event;
using Menu.Domain.Entities;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Domain.Events.FoodEvents
{
    public class FoodUpdatedStatusEvent : IDomainEvent
    {
        public Guid IdFood { get; }
        public DateTimeOffset UpdatedAt { get; }
        public FoodStatus FoodStatus { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodUpdatedStatusEvent(Guid idFood, DateTimeOffset updatedAt, FoodStatus foodStatus)
        {
            IdFood = idFood;
            UpdatedAt = updatedAt;
            FoodStatus = foodStatus;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}