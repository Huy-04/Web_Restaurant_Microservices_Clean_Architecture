using MediatR;
using Menu.Application.DTOs.Requests.Food;
using Menu.Application.DTOs.Responses.Food;

namespace Menu.Application.Modules.Food.Commands.AddFoodRecipe
{
    public sealed record AddFoodRecipeCommand(FoodRecipeRequest Request) : IRequest<FoodRecipeResponse>
    {
    }
}