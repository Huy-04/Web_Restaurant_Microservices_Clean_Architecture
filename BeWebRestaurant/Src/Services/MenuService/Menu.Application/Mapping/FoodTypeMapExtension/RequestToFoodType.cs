using Menu.Application.DTOs.Requests.FoodType;
using Menu.Domain.Entities;
using Menu.Domain.ValueObjects.FoodType;

namespace Menu.Application.Mapping.FoodTypeMapExtension
{
    public static class RequestToFoodType
    {
        public static FoodType ToFoodType(this FoodTypeRequest request)
        {
            return FoodType.Create(FoodTypeName.Create(request.FoodTypeName));
        }

        public static FoodTypeName ToFoodTypeName(this FoodTypeRequest request)
        {
            return FoodTypeName.Create(request.FoodTypeName);
        }

        public static void ApplyFoodType(this FoodType foodType, FoodTypeRequest request)
        {
            foodType.UpdateName(request.ToFoodTypeName());
        }
    }
}