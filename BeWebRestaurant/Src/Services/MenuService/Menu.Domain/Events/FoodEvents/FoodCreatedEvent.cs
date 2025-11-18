using Domain.Core.Interface.Event;
using Domain.Core.ValueObjects;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Domain.Events.FoodEvents
{
    public class FoodCreatedEvent : IDomainEvent
    {
        public Guid IdFood { get; }
        public FoodName FoodName { get; }
        public Description Description { get; }
        public Img Img { get; }
        public Guid FoodTypeId { get; }
        public FoodStatus FoodStatus { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodCreatedEvent(Guid idfood, FoodName foodName, Description description, Img img, Guid foodTypeId, FoodStatus foodStatus)
        {
            IdFood = idfood;
            FoodName = foodName;
            Description = description;
            Img = img;
            FoodTypeId = foodTypeId;
            FoodStatus = foodStatus;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}