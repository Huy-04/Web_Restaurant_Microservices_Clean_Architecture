using Menu.Application.Interfaces;
using Menu.Domain.Entities;
using Menu.Domain.ValueObjects.FoodType;
using Menu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Menu.Infrastructure.Repository
{
    public class FoodTypeRepository : IFoodTypeRepository
    {
        private readonly MenuDbContext _context;

        public FoodTypeRepository(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodType>> GetAllAsync(CancellationToken token)
        {
            return await _context.FoodTypes
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<FoodType?> GetByIdAsync(Guid idFoodType, CancellationToken token)
        {
            return await _context.FoodTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(ft => ft.Id == idFoodType, token);
        }

        public async Task<FoodType> CreateAsync(FoodType foodType, CancellationToken token)
        {
            await _context.FoodTypes.AddAsync(foodType, token);
            return foodType;
        }

        public Task<FoodType> UpdateAsync(FoodType foodType)
        {
            _context.FoodTypes.Update(foodType);
            return Task.FromResult(foodType);
        }

        public async Task<bool> DeleteAsync(Guid idFoodType, CancellationToken token)
        {
            var foodtype = await _context.FoodTypes.FindAsync(new object[] { idFoodType }, token);
            if (foodtype is null) return false;
            _context.FoodTypes.Remove(foodtype);
            return true;
        }

        public async Task<bool> ExistsByNameAsync(FoodTypeName foodTypeName, CancellationToken token, Guid? idFoodType)
        {
            return await _context.FoodTypes
                .AsNoTracking()
                .AnyAsync(ft =>
                (idFoodType == null || ft.Id != idFoodType)
                && ft.FoodTypeName == foodTypeName, token);
        }

        public async Task<bool> ExistsByIdAsync(Guid idFoodType, CancellationToken token)
        {
            return await _context.FoodTypes
                .AsNoTracking()
                .AnyAsync(ft => ft.Id == idFoodType, token);
        }
    }
}