using Inventory.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly InventoryDbContext _context;
        private IDbContextTransaction? _transaction;
        public IFoodRecipesRepository FoodRecipesRepo { get; }
        public IIngredientsRepository IngredientsRepo { get; }
        public IStockItemsRepository StockItemsRepo { get; }
        public IStockRepository StockRepo { get; }

        public UnitOfWork(InventoryDbContext context, IFoodRecipesRepository foodRecipesRepo,
            IIngredientsRepository ingredientsRepo, IStockItemsRepository stockItemsRepo,
            IStockRepository stockRepo)
        {
            _context = context;
            FoodRecipesRepo = foodRecipesRepo;
            IngredientsRepo = ingredientsRepo;
            StockItemsRepo = stockItemsRepo;
            StockRepo = stockRepo;
        }

        public async Task BeginTransactionAsync(CancellationToken token)
        {
            if (_transaction is not null) return;
            _transaction = await _context.Database.BeginTransactionAsync(token);
        }

        public async Task CommitAsync(CancellationToken token)
        {
            if (_transaction is null) return;

            try
            {
                await _context.SaveChangesAsync(token);
                await _transaction.CommitAsync(token);
            }
            catch
            {
                await _transaction.RollbackAsync(token);
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollBackAsync(CancellationToken token)
        {
            if (_transaction is null) return;

            await _transaction.RollbackAsync(token);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}