using Common.Mapping.MoneyMapExtension;
using Menu.Application.DTOs.Responses.Food;
using Menu.Domain.Entities;

namespace Menu.Application.Mapping.FoodMapExtension
{
    public static class FoodToResponse
    {
        public static FoodResponse ToFoodResponse(this Food food, string foodTypeName)
        {
            return new(
                food.Id,
                food.FoodName.Value,
                food.Img.Value,
                food.Description.Value,
                food.FoodStatus.Value,
                food.FoodTypeId,
                foodTypeName,
                food.Money.ToMoneyResponse(),
                food.CreatedAt,
                food.UpdatedAt);
        }
    }
}