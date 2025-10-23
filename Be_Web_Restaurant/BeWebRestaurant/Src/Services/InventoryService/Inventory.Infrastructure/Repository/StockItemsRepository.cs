using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.StockItems;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repository
{
    public class StockItemsRepository : IStockItemsRepository
    {
        private readonly InventoryDbContext _context;

        public StockItemsRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockItems>> GetAllAsync(CancellationToken token)
        {
            return await _context.StockItems
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<StockItems?> GetByIdAsync(Guid idStockItems, CancellationToken token)
        {
            return await _context.StockItems
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == idStockItems, token);
        }

        public async Task<IEnumerable<StockItems>> GetByIngredientsAsync(Guid ingredientsId, CancellationToken token)
        {
            return await _context.StockItems
                .AsNoTracking()
                .Where(i => i.IngredientsId == ingredientsId)
                .ToListAsync(token);
        }

        public async Task<IEnumerable<StockItems>> GetByStockAsync(Guid stockId, CancellationToken token)
        {
            return await _context.StockItems
                .AsNoTracking()
                .Where(i => i.StockId == stockId)
                .ToListAsync(token);
        }

        public async Task<IEnumerable<StockItems>> GetByStatusAsync(StockItemsStatus stockStatus, CancellationToken token)
        {
            return await _context.StockItems
                .AsNoTracking()
                .Where(i => i.StockItemsStatus == stockStatus)
                .ToListAsync(token);
        }

        public async Task<StockItems> CreateAsync(StockItems stockItems, CancellationToken token)
        {
            await _context.StockItems.AddAsync(stockItems, token);
            return stockItems;
        }

        public Task<StockItems> Update(StockItems stockItems)
        {
            _context.Update(stockItems);
            return Task.FromResult(stockItems);
        }

        public async Task<bool> DeleteAsync(Guid idStockItems, CancellationToken token)
        {
            var entity = await _context.StockItems.FindAsync(new object[] {idStockItems},token);
            if (entity is null) return false;
            _context.StockItems.Remove(entity);
            return true;
        }

        public async Task<bool> ExistsByIdAsync(Guid idStockItems, CancellationToken token )
        {
            return await _context.StockItems
                .AsNoTracking()
                .AnyAsync(i => i.Id == idStockItems, token);
        }

        public async Task<bool> ExistsByStockIdAndIngredientsIdAsync(Guid stockId, Guid ingredientsId, CancellationToken token, Guid? idStockItems)
        {
            return await _context.StockItems
                .AsNoTracking()
                .AnyAsync(i =>
                    (idStockItems == null || i.Id != idStockItems)
                    && i.StockId == stockId
                    && i.IngredientsId == ingredientsId, token);
        }

        public async Task<bool> ExistsByStockAsync(Guid stockId, CancellationToken token)
        {
            return await _context.StockItems
                .AsNoTracking()
                .AnyAsync(i => i.StockId == stockId, token);
        }

        public async Task<bool> ExistsByIngredientsAsync(Guid ingredientsId, CancellationToken token )
        {
            return await _context.StockItems
                .AsNoTracking()
                .AnyAsync(i => i.IngredientsId == ingredientsId, token);
        }
    }
}