using System.Threading;
using System.Threading.Tasks;
using Domain.Core.Base;
using FluentAssertions;
using MediatR;
using Menu.Domain.Entities;
using Menu.Domain.Enums;
using Menu.Domain.ValueObjects.Food;
using Menu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Menu.Domain.Tests;

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

    private static MenuDbContext MakeContext()
    {
        var options = new DbContextOptionsBuilder<MenuDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new MenuDbContext(options);
    }

    [Fact]
    public async Task Commit_Dispatches_Aggregate_DomainEvents()
    {
        using var ctx = MakeContext();
        var mediator = new FakeMediator();
        var uow = new Menu.Infrastructure.Persistence.UnitOfWork(ctx, foodRepo: null!, foodTypeRepo: null!, mediator);

        // Arrange aggregate with a domain event
        var food = Food.Create(FoodName.Create("Pho"), Money.Create(10m, CurrencyEnum.USD), Guid.NewGuid(), Img.Create("img"), Description.Create("desc"));
        ctx.Add(food);

        await uow.BeginTransactionAsync(CancellationToken.None);
        await uow.CommitAsync(CancellationToken.None);

        mediator.Published.Should().NotBeEmpty();
        mediator.Published.Any(e => e.GetType().Name.Contains("FoodCreatedEvent")).Should().BeTrue();
        // Ensure domain events cleared on aggregate
        ((AggregateRoot)food).DomainEvents.Should().BeEmpty();
    }
}
