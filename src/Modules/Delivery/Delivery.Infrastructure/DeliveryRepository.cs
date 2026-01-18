using Delivery.Application.Contracts;
using Delivery.Domain;
using Logistics.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure;

public sealed class DeliveryRepository : IDeliveryRepository
{
    private readonly LogisticsDbContext _db;
    public DeliveryRepository(LogisticsDbContext db) => _db = db;

    public async Task AddAsync(DeliveryAggregate delivery, CancellationToken ct)
    {
        var exists = await _db.Deliveries.AnyAsync(x => x.Id == delivery.Id, ct);
        if (!exists)
            await _db.Deliveries.AddAsync(delivery, ct);
        else
            _db.Deliveries.Update(delivery);

        await _db.SaveChangesAsync(ct);
    }

    public Task<DeliveryAggregate?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Deliveries.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<DeliveryAggregate?> GetByTrackingNumberAsync(string trackingNumber, CancellationToken ct)
        => _db.Deliveries.FirstOrDefaultAsync(x => x.TrackingNumber == trackingNumber, ct);
}
