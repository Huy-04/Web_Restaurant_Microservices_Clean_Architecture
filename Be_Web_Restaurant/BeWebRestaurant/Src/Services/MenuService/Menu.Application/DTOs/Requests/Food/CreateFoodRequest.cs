using Common.DTOs.Requests.Money;

namespace Menu.Application.DTOs.Requests.Food
{
    public sealed record CreateFoodRequest(
        string FoodName,
        Guid FoodTypeId,
        string Img,
        string Description,
        MoneyRequest Money)
    {
    }
}