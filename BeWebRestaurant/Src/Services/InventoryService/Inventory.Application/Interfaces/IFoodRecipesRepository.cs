using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IFoodRecipesRepository
    {
        Task<IEnumerable<FoodRecipe>> GetAllAsync(CancellationToken token = default);

        Task<FoodRecipe?> GetByIdAsync(Guid idFoodRecipe, CancellationToken token = default);

        Task<IEnumerable<FoodRecipe>> GetByFoodAsync(Guid foodId, CancellationToken token = default);

        Task<IEnumerable<FoodRecipe>> GetByIngredientsAsync(Guid ingredientsId, CancellationToken token = default);

        Task<IEnumerable<FoodRecipe>> GetByFoodAndIngredients(Guid foodId, Guid ingredientsId, CancellationToken token = default);

        Task<FoodRecipe> CreateAsync(FoodRecipe foodRecipe, CancellationToken token = default);

        Task<FoodRecipe> Update(FoodRecipe foodRecipe);

        Task<bool> DeleteAsync(Guid idFoodRecipe, CancellationToken token = default);

        Task<bool> ExistsByFoodIdAndIngredientsIdAsync(Guid foodId, Guid ingredientsId, CancellationToken token = default, Guid? idFoodRecipe = null);

        Task<bool> ExistsByIngredientsIdAsync(Guid ingredientsId, CancellationToken token = default);
    }
}