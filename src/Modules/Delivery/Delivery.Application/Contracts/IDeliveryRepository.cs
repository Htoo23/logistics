using Delivery.Domain;

namespace Delivery.Application.Contracts;

public interface IDeliveryRepository
{
    Task AddAsync(DeliveryAggregate delivery, CancellationToken ct);
    Task<DeliveryAggregate?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<DeliveryAggregate?> GetByTrackingNumberAsync(string trackingNumber, CancellationToken ct);
}
