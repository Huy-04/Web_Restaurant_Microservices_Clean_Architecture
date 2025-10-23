using Application.Core.Mapping.MoneyMapExtension;
using Domain.Core.ValueObjects;
using Menu.Application.DTOs.Requests.Food;
using Menu.Domain.Entities;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Application.Mapping.FoodMapExtension
{
    public static class RequestToFood
    {
        public static Food ToFood(this CreateFoodRequest request)
        {
            return Food.Create(
                FoodName.Create(request.FoodName),
                request.Money.ToMoney(),
                request.FoodTypeId,
                Img.Create(request.Img),
                Description.Create(request.Description));
        }

        public static void ApplyFood(this Food food, UpdateFoodRequest request)
        {
            food.UpdateBasic(FoodName.Create(request.FoodName), Img.Create(request.Img), Description.Create(request.Description));
            food.UpdateFoodTypeId(request.FoodTypeId);
            food.UpdateStatus(FoodStatus.Create(request.FoodStatus));
            food.UpdateMoney(request.Money.ToMoney());
        }
    }
}