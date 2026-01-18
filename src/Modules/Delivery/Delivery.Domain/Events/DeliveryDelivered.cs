using SharedKernel;

namespace Delivery.Domain.Events;

public sealed record DeliveryDelivered(Guid DeliveryId) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
