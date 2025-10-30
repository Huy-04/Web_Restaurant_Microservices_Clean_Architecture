using Inventory.Domain.IRepository;
using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.Ingredients;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repository
{
    public class IngredientsRepository : IIngredientsRepository
    {
        private readonly InventoryDbContext _context;

        public IngredientsRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredients>> GetAllAsync(CancellationToken token)
        {
            return await _context.Ingredients
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<Ingredients?> GetByIdAsync(Guid idIngredients, CancellationToken token)
        {
            return await _context.Ingredients
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == idIngredients, token);
        }

        public async Task<Ingredients> CreateAsync(Ingredients ingredients, CancellationToken token)
        {
            await _context.Ingredients.AddAsync(ingredients, token);
            return ingredients;
        }

        public Task<Ingredients> Update(Ingredients ingredients)
        {
            _context.Update(ingredients);
            return Task.FromResult(ingredients);
        }

        public async Task<bool> DeleteAsync(Guid idIngredients, CancellationToken token)
        {
            var entity = await _context.Ingredients.FindAsync(new object[] { idIngredients }, token);
            if (entity is null) return false;
            _context.Ingredients.Remove(entity);
            return true;
        }

        public async Task<bool> ExistsByNameAsync(IngredientsName ingredientsName, CancellationToken token, Guid? idIngredients)
        {
            return await _context.Ingredients
                .AsNoTracking()
                .AnyAsync(i =>
                (idIngredients == null || i.Id != idIngredients)
                && i.IngredientsName == ingredientsName, token);
        }

        public async Task<bool> ExistsByIdAsync(Guid idIngredients, CancellationToken token)
        {
            return await _context.Ingredients
                .AsNoTracking()
                .AnyAsync(i => i.Id == idIngredients, token);
        }
    }
}