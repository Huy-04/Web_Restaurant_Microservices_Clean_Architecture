using Domain.Core.Interface.Event;

namespace Domain.Core.Base
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected AggregateRoot() : base()
        { }

        protected AggregateRoot(Guid id) : base(id)
        { }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        protected void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
        {
            var snapshot = _domainEvents.ToArray();
            _domainEvents.Clear();
            return snapshot;
        }
    }
}