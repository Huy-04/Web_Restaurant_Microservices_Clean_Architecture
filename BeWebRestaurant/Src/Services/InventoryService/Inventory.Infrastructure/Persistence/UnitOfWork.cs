using Inventory.Application.Interface;
using Inventory.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
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

        public UnitOfWork
            (InventoryDbContext context,
            IFoodRecipesRepository foodRecipesRepo,
            IIngredientsRepository ingredientsRepo,
            IStockItemsRepository stockItemsRepo,
            IStockRepository stockRepo, IMediator mediator)
        {
            _context = context;
            FoodRecipesRepo = foodRecipesRepo;
            IngredientsRepo = ingredientsRepo;
            StockItemsRepo = stockItemsRepo;
            StockRepo = stockRepo;
        }

        public async Task BeginTransactionAsync(CancellationToken token)
        {
            if (_transaction is not null)
                throw new InvalidOperationException("Transaction already started");

            _transaction = await _context.Database
                .BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, token);
        }

        public async Task<int> SaveChangesAsync(CancellationToken token)
        {
            return await _context.SaveChangesAsync(token);
        }

        public async Task CommitAsync(CancellationToken token)
        {
            if (_transaction is null)
                throw new InvalidOperationException("No transaction to commit");

            try
            {
                await _transaction.CommitAsync(token);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync(CancellationToken token)
        {
            if (_transaction is null) return;

            try
            {
                await _transaction.RollbackAsync(token);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is not null)
                await _transaction.DisposeAsync();

            await _context.DisposeAsync();
        }
    }
}