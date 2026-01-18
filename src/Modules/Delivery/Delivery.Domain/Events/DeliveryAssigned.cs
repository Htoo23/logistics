using SharedKernel;

namespace Delivery.Domain.Events;

public sealed record DeliveryAssigned(Guid DeliveryId, Guid DriverId, Guid VehicleId) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
