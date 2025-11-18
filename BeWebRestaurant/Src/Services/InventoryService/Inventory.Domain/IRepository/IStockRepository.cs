using Inventory.Domain.ValueObjects.Stock;
using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Domain.IRepository
{
    public interface IStockRepository
    {
        public Task<IEnumerable<Stock>> GetAllAsync(CancellationToken token = default);

        public Task<Stock?> GetByIdAsync(Guid idStock, CancellationToken token = default);

        public Task<Stock> CreateAsync(Stock stock, CancellationToken token = default);

        public Task<Stock> Update(Stock stock);

        public Task<bool> DeleteAsync(Guid idStock, CancellationToken token = default);

        public Task<bool> ExistsByNameAsync(StockName stockName, CancellationToken token = default, Guid? idStock = null);

        public Task<bool> ExistsByIdAsync(Guid idStock, CancellationToken token = default);

        // Methods for querying Stock with its StockItems (child entities)
        Task<Stock?> GetByIdWithItemsAsync(Guid stockId, CancellationToken token = default);

        Task<IEnumerable<Stock>> GetStocksByIngredientAsync(Guid ingredientsId, CancellationToken token = default);

        Task<IEnumerable<Stock>> GetStocksByStatusAsync(StockItemsStatus status, CancellationToken token = default);

        Task<bool> HasItemWithIngredientAsync(Guid stockId, Guid ingredientsId, CancellationToken token = default);
    }
}