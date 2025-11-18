using Inventory.Domain.IRepository;
using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.Stock;
using Inventory.Domain.ValueObjects.StockItems;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly InventoryDbContext _context;

        public StockRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stock>> GetAllAsync(CancellationToken token)
        {
            return await _context.Stocks
                .Include(s => s.StockItems)
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<Stock?> GetByIdAsync(Guid idStock, CancellationToken token)
        {
            return await _context.Stocks
                .Include(s => s.StockItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == idStock, token);
        }

        public async Task<Stock> CreateAsync(Stock stock, CancellationToken token)
        {
            await _context.Stocks.AddAsync(stock, token);
            return stock;
        }

        public Task<Stock> Update(Stock stock)
        {
            _context.Stocks.Update(stock);
            return Task.FromResult(stock);
        }

        public async Task<bool> DeleteAsync(Guid idStock, CancellationToken token)
        {
            var entity = await _context.Stocks
                .Include(s => s.StockItems)
                .FirstOrDefaultAsync(s => s.Id == idStock, token);
            if (entity is null) return false;
            _context.Stocks.Remove(entity);
            return true;
        }

        public async Task<bool> ExistsByNameAsync(StockName stockName, CancellationToken token, Guid? idStock)
        {
            return await _context.Stocks
                .AsNoTracking()
                .AnyAsync(s =>
                (idStock == null || s.Id != idStock)
                && s.StockName == stockName, token);
        }

        public async Task<bool> ExistsByIdAsync(Guid idStock, CancellationToken token)
        {
            return await _context.Stocks
                .AsNoTracking()
                .AnyAsync(s => s.Id == idStock, token);
        }

        // Methods for querying Stock with its StockItems (child entities)
        public async Task<Stock?> GetByIdWithItemsAsync(Guid stockId, CancellationToken token = default)
        {
            return await _context.Stocks
                .Include(s => s.StockItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == stockId, token);
        }

        public async Task<IEnumerable<Stock>> GetStocksByIngredientAsync(Guid ingredientsId, CancellationToken token = default)
        {
            return await _context.Stocks
                .Include(s => s.StockItems)
                .Where(s => s.StockItems.Any(i => i.IngredientsId == ingredientsId))
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<IEnumerable<Stock>> GetStocksByStatusAsync(StockItemsStatus status, CancellationToken token = default)
        {
            return await _context.Stocks
                .Include(s => s.StockItems)
                .Where(s => s.StockItems.Any(i => i.StockItemsStatus == status))
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<bool> HasItemWithIngredientAsync(Guid stockId, Guid ingredientsId, CancellationToken token = default)
        {
            return await _context.Stocks
                .Where(s => s.Id == stockId)
                .AnyAsync(s => s.StockItems.Any(i => i.IngredientsId == ingredientsId), token);
        }
    }
}