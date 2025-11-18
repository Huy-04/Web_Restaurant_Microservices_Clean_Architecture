using Application.Core.IUnitOfWork;
using Inventory.Domain.IRepository;

namespace Inventory.Application.IUnitOfWork
{
    public interface IInventoryUnitOfWork : IUintOfWorkGeneric
    {
        // DEPRECATED: FoodRecipes and StockItems are now child entities within their aggregates.
        // Use appropriate aggregate repositories instead.
        // IFoodRecipesRepository FoodRecipesRepo { get; }
        // IStockItemsRepository StockItemsRepo { get; }

        IIngredientsRepository IngredientsRepo { get; }

        IStockRepository StockRepo { get; }
    }
}
