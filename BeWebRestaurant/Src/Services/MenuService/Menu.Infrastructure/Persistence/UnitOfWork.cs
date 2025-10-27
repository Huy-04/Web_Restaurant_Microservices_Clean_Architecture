using Domain.Core.Base;
using MediatR;
using Menu.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Menu.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        private readonly MenuDbContext _context;
        private readonly IMediator _mediator;
        public IFoodRepository FoodRepo { get; }
        public IFoodTypeRepository FoodTypeRepo { get; }

        private IDbContextTransaction? _transaction;

        public UnitOfWork(MenuDbContext context, IFoodRepository foodRepo, IFoodTypeRepository foodTypeRepo, IMediator mediator)
        {
            _context = context;
            FoodRepo = foodRepo;
            FoodTypeRepo = foodTypeRepo;
            _mediator = mediator;
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

                await DispatchDomainEventsAsync(token);

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

        private async Task DispatchDomainEventsAsync(CancellationToken token)
        {
            var aggregates = _context.ChangeTracker.Entries<AggregateRoot>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var events = aggregates.SelectMany(a => a.DomainEvents).ToList();

            foreach (var aggregate in aggregates)
            {
                aggregate.GetType().GetMethod("ClearDomainEvents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.Invoke(aggregate, null);
            }

            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent, token);
            }
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
