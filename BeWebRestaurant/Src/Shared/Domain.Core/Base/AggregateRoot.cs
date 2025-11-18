using Domain.Core.Base;
using Domain.Core.Interface.Event;

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

    public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
    {
        var snapshot = _domainEvents.ToArray();
        _domainEvents.Clear();
        return snapshot;
    }
}