using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Application.Interfaces
{
    public interface IStockItemsRepository
    {
        Task<IEnumerable<StockItems>> GetAllAsync(CancellationToken token);

        Task<StockItems?> GetByIdAsync(Guid idStockItems, CancellationToken token = default);

        Task<IEnumerable<StockItems>> GetByIngredientsAsync(Guid ingredientsId, CancellationToken token = default);

        Task<IEnumerable<StockItems>> GetByStockAsync(Guid stockId, CancellationToken token = default);

        Task<IEnumerable<StockItems>> GetByStatusAsync(StockItemsStatus stockItemsStatus, CancellationToken token = default);

        Task<StockItems> CreateAsync(StockItems stockItems, CancellationToken token = default);

        Task<StockItems> UpdateAsync(StockItems stockItems);

        Task<bool> DeleteAsync(Guid idStockItems, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid idStockItems, CancellationToken token = default);

        Task<bool> ExistsByStockIdAndIngredientsIdAsync(Guid stockId, Guid ingredientsId, CancellationToken token = default, Guid? stockItemsId = null);

        Task<bool> ExistsByStockAsync(Guid stockId, CancellationToken token = default);

        Task<bool> ExistsByIngredientsAsync(Guid ingredientsId, CancellationToken token = default);
    }
}