using Inventory.Application.DTOs.Responses.Ingredients;
using Inventory.Domain.Entities;

namespace Inventory.Application.Mapping.IngredientsMapExtension
{
    public static class IngredientsToResponse
    {
        public static IngredientsResponse ToIngredientsResponse(this Ingredients ingredients)
        {
            return new(
                ingredients.Id,
                ingredients.IngredientsName,
                ingredients.Description,
                ingredients.CreatedAt,
                ingredients.UpdatedAt);
        }
    }
}