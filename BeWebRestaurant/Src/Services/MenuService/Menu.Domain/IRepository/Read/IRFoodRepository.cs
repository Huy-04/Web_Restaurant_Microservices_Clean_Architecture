using Domain.Core.Interface.IRepository;
using Menu.Domain.Entities;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Domain.IRepository.Read
{
    public interface IRFoodRepository : IReadRepositoryGeneric<Food>
    {
        Task<IEnumerable<Food>> GetByFoodTypeAsync(Guid foodTypeId, CancellationToken token = default);

        Task<IEnumerable<Food>> GetByStatusAsync(FoodStatus foodStatus, CancellationToken token = default);

        Task<bool> ExistsByNameAsync(FoodName foodName, CancellationToken token = default, Guid? idFood = null);

        // Methods for querying Food with its FoodRecipes (child entities)
        Task<Food?> GetByIdWithRecipesAsync(Guid foodId, CancellationToken token = default);

        Task<IEnumerable<Food>> GetFoodsByIngredientAsync(Guid ingredientsId, CancellationToken token = default);

        Task<bool> HasRecipeWithIngredientAsync(Guid foodId, Guid ingredientsId, CancellationToken token = default);
    }
}