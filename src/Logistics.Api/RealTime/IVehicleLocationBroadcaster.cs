namespace Logistics.Api.RealTime;

public interface IVehicleLocationBroadcaster
{
    Task BroadcastLocationAsync(Guid vehicleId, object payload, CancellationToken ct);
}
