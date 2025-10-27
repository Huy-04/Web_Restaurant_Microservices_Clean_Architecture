using Domain.Core.Interface.Event;
using Menu.Domain.ValueObjects.FoodType;

namespace Menu.Domain.Events.FoodTypeEvents
{
    public class FoodTypeCreatedEvent : IDomainEvent
    {
        public Guid IdFoodType { get; }
        public FoodTypeName FoodTypeName { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodTypeCreatedEvent(Guid idFoodType, FoodTypeName foodTypeName)
        {
            IdFoodType = idFoodType;
            FoodTypeName = foodTypeName;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
