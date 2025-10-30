using Inventory.Domain.ValueObjects.Stock;
using Inventory.Domain.Entities;

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
    }
}