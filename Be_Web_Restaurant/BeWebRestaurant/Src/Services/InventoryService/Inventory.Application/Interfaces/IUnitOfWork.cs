namespace Inventory.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IFoodRecipesRepository FoodRecipesRepo { get; }

        IStockItemsRepository StockItemsRepo { get; }

        IIngredientsRepository IngredientsRepo { get; }

        IStockRepository StockRepo { get; }

        public Task BeginTransactionAsync(CancellationToken token = default);

        public Task CommitAsync(CancellationToken token = default);

        public Task RollBackAsync(CancellationToken token = default);
    }
}