using System.Threading;
using System.Threading.Tasks;
using Domain.Core.Base;
using FluentAssertions;
using MediatR;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Inventory.Domain.Tests;

public class DomainEventDispatchTests
{
    private class FakeMediator : IMediator
    {
        public readonly List<object> Published = new();
        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            Published.Add(notification);
            return Task.CompletedTask;
        }
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            Published.Add(notification!);
            return Task.CompletedTask;
        }
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<object?> Send(object request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }

    private static InventoryDbContext MakeContext()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new InventoryDbContext(options);
    }

    [Fact]
    public async Task Commit_Dispatches_Aggregate_DomainEvents()
    {
        using var ctx = MakeContext();
        var mediator = new FakeMediator();
        var uow = new Inventory.Infrastructure.Persistence.UnitOfWork(ctx, foodRecipesRepo: null!, ingredientsRepo: null!, stockItemsRepo: null!, stockRepo: null!, mediator);

        var stock = Stock.Create(Inventory.Domain.ValueObjects.Stock.StockName.Create("Main"), Domain.Core.ValueObjects.Description.Create("desc"));
        ctx.Add(stock);

        await uow.BeginTransactionAsync(CancellationToken.None);
        await uow.CommitAsync(CancellationToken.None);

        mediator.Published.Should().NotBeEmpty();
        mediator.Published.Any(e => e.GetType().Name.Contains("StockCreatedEvent")).Should().BeTrue();
        ((AggregateRoot)stock).DomainEvents.Should().BeEmpty();
    }
}
