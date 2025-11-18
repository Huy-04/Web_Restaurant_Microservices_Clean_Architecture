using Domain.Core.Interface.Event;
using Domain.Core.ValueObjects;
using Menu.Domain.Enums;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Domain.Events.FoodEvents
{
    public class FoodUpdatedEvent : IDomainEvent
    {
        public Guid IdFood { get; }
        public string FoodName { get; }
        public decimal Price { get; }
        public string ImgUrl { get; }
        public string Description { get; }
        public string FoodStatus { get; }
        public Guid FoodTypeId { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodUpdatedEvent(
            Guid idFood, 
            string foodName, 
            decimal price, 
            string imgUrl, 
            string description, 
            string foodStatus, 
            Guid foodTypeId,
            DateTimeOffset updatedAt)
        {
            IdFood = idFood;
            FoodName = foodName;
            Price = price;
            ImgUrl = imgUrl;
            Description = description;
            FoodStatus = foodStatus;
            FoodTypeId = foodTypeId;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
