namespace Domain.Core.Interface.Event
{
    public interface IDomainEvent
    {
        DateTimeOffset OccurredOn { get; }
    }
}