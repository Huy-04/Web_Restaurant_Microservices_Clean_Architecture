using MediatR;
using Microsoft.Extensions.Logging;
using Menu.Domain.Events.FoodEvents;

namespace Menu.Application.DomainEvents
{
    public sealed class FoodCreatedEventHandler : INotificationHandler<DomainEventNotification<FoodCreatedEvent>>
    {
        private readonly ILogger<FoodCreatedEventHandler> _logger;

        public FoodCreatedEventHandler(ILogger<FoodCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<FoodCreatedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("FoodCreatedEvent handled for Id={Id}", notification.DomainEvent.IdFood);
            return Task.CompletedTask;
        }
    }
}