namespace SharedKernel;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}
