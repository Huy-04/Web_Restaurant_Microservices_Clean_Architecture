using Menu.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Menu.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        private readonly MenuDbContext _context;
        public IFoodRepository FoodRepo { get; }
        public IFoodTypeRepository FoodTypeRepo { get; }

        private IDbContextTransaction? _transaction;

        public UnitOfWork(MenuDbContext context, IFoodRepository foodRepo, IFoodTypeRepository foodTypeRepo)
        {
            _context = context;
            FoodRepo = foodRepo;
            FoodTypeRepo = foodTypeRepo;
        }

        public async Task BeginTransactionAsync(CancellationToken token)
        {
            if (_transaction is not null) return;
            _transaction = await _context.Database
                .BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, token);
        }

        public async Task CommitAsync(CancellationToken token)
        {
            if (_transaction is null) return;

            try
            {
                await _context.SaveChangesAsync(token);
                await _transaction.CommitAsync(token);
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

        public void Dispose()
        {
            _context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}