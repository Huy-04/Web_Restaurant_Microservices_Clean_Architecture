using Inventory.Domain.IRepository;

namespace Inventory.Application.Interface
{
    public interface IUnitOfWork
    {
        IFoodRecipesRepository FoodRecipesRepo { get; }

        IStockItemsRepository StockItemsRepo { get; }

        IIngredientsRepository IngredientsRepo { get; }

        IStockRepository StockRepo { get; }

        Task BeginTransactionAsync(CancellationToken token = default);

        Task CommitAsync(CancellationToken token = default);

        Task<int> SaveChangesAsync(CancellationToken token = default);

        Task RollbackAsync(CancellationToken token = default);
    }
}