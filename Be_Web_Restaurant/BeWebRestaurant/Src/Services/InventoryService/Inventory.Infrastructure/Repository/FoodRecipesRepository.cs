using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repository
{
    public class FoodRecipesRepository : IFoodRecipesRepository
    {
        private readonly InventoryDbContext _context;

        public FoodRecipesRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodRecipe>> GetAllAsync(CancellationToken token)
        {
            return await _context.FoodRecipes
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<FoodRecipe?> GetByIdAsync(Guid idFoodRecipe, CancellationToken token)
        {
            return await _context.FoodRecipes
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == idFoodRecipe, token);
        }

        public async Task<IEnumerable<FoodRecipe>> GetByFoodAsync(Guid foodId, CancellationToken token)
        {
            return await _context.FoodRecipes
                .AsNoTracking()
                .Where(f => f.FoodId == foodId)
                .ToListAsync(token);
        }

        public async Task<IEnumerable<FoodRecipe>> GetByIngredientsAsync(Guid ingredientsId, CancellationToken token)
        {
            return await _context.FoodRecipes
                .AsNoTracking()
                .Where(f => f.IngredientsId == ingredientsId)
                .ToListAsync(token);
        }

        public async Task<IEnumerable<FoodRecipe>> GetByFoodAndIngredients(Guid foodId, Guid ingredientsId, CancellationToken token)
        {
            return await _context.FoodRecipes
                .AsNoTracking()
                .Where(f => f.IngredientsId == ingredientsId && f.FoodId == foodId)
                .ToListAsync(token);
        }

        public async Task<FoodRecipe> CreateAsync(FoodRecipe foodRecipe, CancellationToken token)
        {
            await _context.FoodRecipes.AddAsync(foodRecipe, token);
            return foodRecipe;
        }

        public Task<FoodRecipe> UpdateAsync(FoodRecipe foodRecipe)
        {
            var entity = _context.Update(foodRecipe);
            return Task.FromResult(foodRecipe);
        }

        public async Task<bool> DeleteAsync(Guid idFoodRecipe, CancellationToken token)
        {
            var entity = await _context.FoodRecipes.FindAsync(new object[] { idFoodRecipe }, token);
            if (entity is null) return false;
            _context.FoodRecipes.Remove(entity);
            return true;
        }

        public async Task<bool> ExistsByFoodIdAndIngredientsIdAsync(Guid foodId, Guid ingredientsId, CancellationToken token, Guid? idFoodRecipe)
        {
            return await _context.FoodRecipes
                .AsNoTracking()
                .AnyAsync(f =>
                (idFoodRecipe == null || f.Id != idFoodRecipe)
                && f.FoodId == foodId
                && f.IngredientsId == ingredientsId, token);
        }

        public async Task<bool> ExistsByIngredientsIdAsync(Guid ingredientsId, CancellationToken token )
        {
            return await _context.FoodRecipes
                .AsNoTracking()
                .AnyAsync(f => f.IngredientsId == ingredientsId, token);
        }
    }
}