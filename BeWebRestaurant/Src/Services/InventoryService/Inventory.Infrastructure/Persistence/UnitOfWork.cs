using Domain.Core.Base;
using Inventory.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly InventoryDbContext _context;
        private readonly IMediator _mediator;
        private IDbContextTransaction? _transaction;
        public IFoodRecipesRepository FoodRecipesRepo { get; }
        public IIngredientsRepository IngredientsRepo { get; }
        public IStockItemsRepository StockItemsRepo { get; }
        public IStockRepository StockRepo { get; }

        public UnitOfWork(InventoryDbContext context, IFoodRecipesRepository foodRecipesRepo,
            IIngredientsRepository ingredientsRepo, IStockItemsRepository stockItemsRepo,
            IStockRepository stockRepo, IMediator mediator)
        {
            _context = context;
            FoodRecipesRepo = foodRecipesRepo;
            IngredientsRepo = ingredientsRepo;
            StockItemsRepo = stockItemsRepo;
            StockRepo = stockRepo;
            _mediator = mediator;
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

                await DispatchDomainEventsAsync(token);

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
