using Domain.Core.ValueObjects;
using Inventory.Application.DTOs.Requests.Ingredients;
using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.Ingredients;

namespace Inventory.Application.Mapping.IngredientsMapExtension
{
    public static class RequestToIngredients
    {
        public static Ingredients ToIngredients(this IngredientsRequest request)
        {
            return Ingredients.Create(
                IngredientsName.Create(request.IngredientsName),
                Description.Create(request.Description)
                );
        }
    }
}