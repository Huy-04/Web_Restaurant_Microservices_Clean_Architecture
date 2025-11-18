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

        public async Task<Food?> GetByIdWithRecipesAsync(Guid foodId, CancellationToken token = default)
        {
            return await _context.Set<Food>()
                .Include(f => f.FoodRecipes)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == foodId, token);
        }

        public async Task<IEnumerable<Food>> GetFoodsByIngredientAsync(Guid ingredientsId, CancellationToken token = default)
        {
            return await _context.Set<Food>()
                .Include(f => f.FoodRecipes)
                .Where(f => f.FoodRecipes.Any(r => r.IngredientsId == ingredientsId))
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<bool> HasRecipeWithIngredientAsync(Guid foodId, Guid ingredientsId, CancellationToken token = default)
        {
            return await _context.Set<Food>()
                .Where(f => f.Id == foodId)
                .AnyAsync(f => f.FoodRecipes.Any(r => r.IngredientsId == ingredientsId), token);
        }
    }
}