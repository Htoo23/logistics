using SharedKernel;

namespace Delivery.Domain.Events;

public sealed record DeliveryEnRoute(Guid DeliveryId) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
