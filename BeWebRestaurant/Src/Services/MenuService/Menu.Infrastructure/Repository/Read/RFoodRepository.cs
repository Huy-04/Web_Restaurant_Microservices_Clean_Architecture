using Infrastructure.Core.Repository;
using Menu.Domain.Entities;
using Menu.Domain.IRepository.Read;
using Menu.Domain.ValueObjects.Food;
using Menu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Menu.Infrastructure.Repository.Read
{
    public class RFoodRepository : ReadRepositoryGeneric<Food>, IRFoodRepository
    {
        public RFoodRepository(MenuDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Food>> GetByFoodTypeAsync(Guid foodTypeId, CancellationToken token = default)
        {
            return await FindAsync(f => f.FoodTypeId == foodTypeId, token);
        }

        public async Task<IEnumerable<Food>> GetByStatusAsync(FoodStatus foodStatus, CancellationToken token = default)
        {
            return await FindAsync(f => f.FoodStatus == foodStatus, token);
        }

        public async Task<bool> ExistsByNameAsync(FoodName foodName, CancellationToken token = default, Guid? idFood = null)
        {
            return await AnyAsync(f => (idFood == null || f.Id != idFood)
            && f.FoodName == foodName, token);
        }

        // FoodRecipe
        public async Task<IEnumerable<FoodRecipe>> GetReipeByFoodAsync(Guid foodId, CancellationToken token = default)
        {
            return await FindChildEntityAsync<FoodRecipe>(f => f.FoodRecipes, f => f.FoodId == foodId, token);
        }

        public async Task<IEnumerable<FoodRecipe>> GetFoodsByIngredientAsync(Guid ingredientsId, CancellationToken token = default)
        {
            return await FindChildEntityAsync<FoodRecipe>(f => f.FoodRecipes, f => f.IngredientsId == ingredientsId, token);
        }
    }
}