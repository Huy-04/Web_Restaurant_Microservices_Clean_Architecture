using Domain.Core.Interface.IRepository;
using Infrastructure.Core.IRepository;
using Menu.Domain.Entities;
using Menu.Domain.IRepository.Write;
using Menu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Menu.Infrastructure.Repository.Write
{
    public class WFoodRepository : WriteRepositoryGeneric<Food>, IWFoodRepository
    {
        public WFoodRepository(MenuDbContext context) : base(context)
        {
        }

        public async Task<Food?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _entities
                .Include(f => f.FoodRecipes)
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }
    }
}