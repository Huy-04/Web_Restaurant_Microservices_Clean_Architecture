using Domain.Core.Base;
using Domain.Core.ValueObjects;
using Menu.Domain.Enums;
using Menu.Domain.Events.FoodEvents;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Domain.Entities
{
    public sealed class Food : AggregateRoot
    {
        // vo
        public FoodName FoodName { get; private set; } = default!;

        public Money Money { get; private set; } = default!;

        public Img Img { get; private set; } = default!;

        public Description Description { get; private set; } = default!;

        public FoodStatus FoodStatus { get; private set; } = default!;

        // time
        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        // relationship
        public Guid FoodTypeId { get; private set; }

        // for orm
        private Food()
        { }

        private Food(Guid id, FoodName foodName, Money money, Guid foodTypeId, Img img, Description description, FoodStatus foodStatus)
            : base(id)
        {
            FoodName = foodName;
            Money = money;
            FoodTypeId = foodTypeId;
            Img = img;
            Description = description;
            FoodStatus = foodStatus;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Food Create(FoodName foodName, Money money, Guid foodTypeId, Img img, Description description)
        {
            var food = new Food(Guid.NewGuid(), foodName, money, foodTypeId, img, description, FoodStatus.Create(FoodStatusEnum.Active));

            var foodCreatedEvent = new FoodCreatedEvent(food.Id, foodName, description, img, foodTypeId, food.FoodStatus);

            food.AddDomainEvent(foodCreatedEvent);

            return food;
        }

        // behavior
        public void UpdateBasic(FoodName foodName, Img img, Description description)
        {
            if (FoodName == foodName && Img == img && Description == description) return;
            FoodName = foodName;
            Img = img;
            Description = description;
            Touch();
        }

        public void UpdateStatus(FoodStatus foodStatus)
        {
            if (FoodStatus == foodStatus) return;
            FoodStatus = foodStatus;
            Touch();

            AddDomainEvent(new FoodUpdatedStatusEvent(Id, UpdatedAt, FoodStatus));
        }

        public void MarkAsActive() => UpdateStatus(FoodStatus.Create(FoodStatusEnum.Active));

        public void MarkAsDiscontinued() => UpdateStatus(FoodStatus.Create(FoodStatusEnum.Discontinued));

        public void UpdateFoodTypeId(Guid foodTypeId)
        {
            if (FoodTypeId == foodTypeId) return;
            FoodTypeId = foodTypeId;
            Touch();
        }

        public void UpdateMoney(Money money)
        {
            if (Money == money) return;
            Money = money;
            Touch();
        }

        // extenstion
        private void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}