using Microsoft.AspNetCore.SignalR;

namespace Logistics.Api.RealTime;

public sealed class FleetHub : Hub
{
    public async Task SubscribeVehicle(Guid vehicleId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, VehicleGroup(vehicleId));

    public async Task UnsubscribeVehicle(Guid vehicleId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, VehicleGroup(vehicleId));

    internal static string VehicleGroup(Guid vehicleId) => $"vehicle:{vehicleId}";
}
