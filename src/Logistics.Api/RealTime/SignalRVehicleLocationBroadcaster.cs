using Microsoft.AspNetCore.SignalR;

namespace Logistics.Api.RealTime;

public sealed class SignalRVehicleLocationBroadcaster : IVehicleLocationBroadcaster
{
    private readonly IHubContext<FleetHub> _hub;
    public SignalRVehicleLocationBroadcaster(IHubContext<FleetHub> hub) => _hub = hub;

    public Task BroadcastLocationAsync(Guid vehicleId, object payload, CancellationToken ct)
        => _hub.Clients.Group(FleetHub.VehicleGroup(vehicleId)).SendAsync("vehicleLocation", payload, ct);
}
