using SharedKernel;

namespace Modules.Delivery.Domain.Events;

public sealed record DeliveryScheduled(Guid DeliveryId, string TrackingNumber) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}

public sealed record DeliveryAssigned(Guid DeliveryId, Guid DriverId, Guid VehicleId) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}

public sealed record DeliveryCompleted(Guid DeliveryId, string ReceivedBy, DateTimeOffset CompletedAt) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
