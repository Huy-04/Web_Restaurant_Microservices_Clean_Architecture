using Menu.Application.Interface;
using Menu.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Menu.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        private readonly MenuDbContext _context;
        private IDbContextTransaction? _transaction;

        public IFoodRepository FoodRepo { get; }
        public IFoodTypeRepository FoodTypeRepo { get; }

        public UnitOfWork(MenuDbContext context, IFoodRepository foodRepo, IFoodTypeRepository foodTypeRepo)
        {
            _context = context;
            FoodRepo = foodRepo;
            FoodTypeRepo = foodTypeRepo;
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