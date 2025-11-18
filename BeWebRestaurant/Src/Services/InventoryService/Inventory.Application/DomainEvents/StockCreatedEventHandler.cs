using MediatR;
using Microsoft.Extensions.Logging;
using Inventory.Domain.Events.StockEvents;

namespace Inventory.Application.DomainEvents
{
    public sealed class StockCreatedEventHandler : INotificationHandler<DomainEventNotification<StockCreatedEvent>>
    {
        private readonly ILogger<StockCreatedEventHandler> _logger;

        public StockCreatedEventHandler(ILogger<StockCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<StockCreatedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StockCreatedEvent handled for Id={Id}", notification.DomainEvent.IdStock);
            return Task.CompletedTask;
        }
    }
}