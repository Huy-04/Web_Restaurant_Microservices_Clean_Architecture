using Infrastructure.Core.UnitOfWork;
using Inventory.Application.IUnitOfWork;
using Inventory.Domain.IRepository;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.UnitOfWork
{
    public class InventoryUnitOfWork : UnitOfWorkGeneric, IInventoryUnitOfWork
    {
        public InventoryUnitOfWork(
            InventoryDbContext context,
            // IFoodRecipesRepository foodRecipesRepo,
            // IStockItemsRepository stockItemsRepo,
            IIngredientsRepository ingredientsRepo,
            IStockRepository stockRepo) : base(context)
        {
            // FoodRecipesRepo = foodRecipesRepo;
            // StockItemsRepo = stockItemsRepo;
            IngredientsRepo = ingredientsRepo;
            StockRepo = stockRepo;
        }

        // public IFoodRecipesRepository FoodRecipesRepo { get; }

        // public IStockItemsRepository StockItemsRepo { get; }

        public IIngredientsRepository IngredientsRepo { get; }

        public IStockRepository StockRepo { get; }
    }
}
