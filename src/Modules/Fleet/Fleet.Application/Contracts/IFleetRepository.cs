using Fleet.Domain;

namespace Fleet.Application.Contracts;

public interface IFleetRepository
{
    Task AddDriverAsync(Driver driver, CancellationToken ct);
    Task AddVehicleAsync(Vehicle vehicle, CancellationToken ct);
    Task<Vehicle?> GetVehicleAsync(Guid vehicleId, CancellationToken ct);
    Task AddLocationAsync(VehicleLocation loc, CancellationToken ct);
}
