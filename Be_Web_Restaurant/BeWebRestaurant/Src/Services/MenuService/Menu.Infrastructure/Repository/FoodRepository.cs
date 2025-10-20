using Menu.Domain.Entities;
using Menu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Menu.Application.Interfaces;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Infrastructure.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private readonly MenuDbContext _context;

        public FoodRepository(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Food>> GetAllAsync(CancellationToken token)
        {
            return await _context.Foods
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<Food?> GetByIdAsync(Guid idFood, CancellationToken token)
        {
            return await _context.Foods
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == idFood,token);
        }

        public async Task<IEnumerable<Food>> GetByFoodTypeAsync(Guid idFoodType, CancellationToken token)
        {
            return await _context.Foods
                .AsNoTracking()
                .Where(f => f.FoodTypeId == idFoodType)
                .ToListAsync(token);
        }

        public async Task<IEnumerable<Food>> GetByStatusAsync(FoodStatus foodStatus, CancellationToken token)
        {
            return await _context.Foods
                .AsNoTracking()
                .Where(f => f.FoodStatus == foodStatus)
                .ToListAsync(token);
        }

        public async Task<Food> CreateAsync(Food food, CancellationToken token)
        {
            await _context.Foods.AddAsync(food, token);
            return food;
        }

        public Task<Food> UpdateAsync(Food food)
        {
            _context.Foods.Update(food);
            return Task.FromResult(food);
        }

        public async Task<bool> DeleteAsync(Guid idFood, CancellationToken token)
        {
            var food = await _context.Foods.FindAsync(new object[] { idFood }, token);
            if (food is null) return false;
            _context.Foods.Remove(food);
            return true;
        }

        public async Task<bool> ExistsByNameAsync(FoodName foodName, CancellationToken token, Guid? idFood)
        {
            return await _context.Foods
                .AsNoTracking()
                .AnyAsync(f =>
                (idFood == null || f.Id != idFood)
                && f.FoodName == foodName.Value, token);
        }

        public async Task<bool> ExistsByIdAsync(Guid idFood, CancellationToken token)
        {
            return await _context.Foods
                .AsNoTracking()
                .AnyAsync(f => f.Id == idFood, token);
        }
    }
}