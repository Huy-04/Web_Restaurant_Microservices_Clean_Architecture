using Menu.Application.DTOs.Responses.FoodType;
using Menu.Domain.Entities;

namespace Menu.Application.Mapping.FoodTypeMapExtension
{
    public static class FoodTypeToResponse
    {
        public static FoodTypeResponse ToFoodTypeResponse(this FoodType foodType)
        {
            return new(foodType.Id,
                foodType.FoodTypeName.Value,
                foodType.CreatedAt,
                foodType.UpdatedAt);
        }
    }
}