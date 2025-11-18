using Domain.Core.Interface.Event;
using Domain.Core.ValueObjects;

namespace Inventory.Domain.Events.IngredientsEvents
{
    public class IngredientsCreatedEvent : IDomainEvent
    {
        public Guid IdIngredients { get; }
        public string Name { get; }
        public Description Description { get; }
        public DateTimeOffset OccurredOn { get; }

        public IngredientsCreatedEvent(Guid idIngredients, string name, Description description)
        {
            IdIngredients = idIngredients;
            Name = name;
            Description = description;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
