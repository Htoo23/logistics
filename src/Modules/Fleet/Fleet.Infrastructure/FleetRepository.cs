using Fleet.Application.Contracts;
using Fleet.Domain;
using Logistics.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure;

public sealed class FleetRepository : IFleetRepository
{
    private readonly LogisticsDbContext _db;
    public FleetRepository(LogisticsDbContext db) => _db = db;

    public async Task AddDriverAsync(Driver driver, CancellationToken ct)
    {
        if (!await _db.Drivers.AnyAsync(d => d.Id == driver.Id, ct))
            await _db.Drivers.AddAsync(driver, ct);
        else
            _db.Drivers.Update(driver);

        await _db.SaveChangesAsync(ct);
    }

    public async Task AddVehicleAsync(Vehicle vehicle, CancellationToken ct)
    {
        if (!await _db.Vehicles.AnyAsync(v => v.Id == vehicle.Id, ct))
            await _db.Vehicles.AddAsync(vehicle, ct);
        else
            _db.Vehicles.Update(vehicle);

        await _db.SaveChangesAsync(ct);
    }

    public Task<Vehicle?> GetVehicleAsync(Guid vehicleId, CancellationToken ct)
        => _db.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId, ct);

    public async Task AddLocationAsync(VehicleLocation loc, CancellationToken ct)
    {
        await _db.VehicleLocations.AddAsync(loc, ct);
        await _db.SaveChangesAsync(ct);
    }
}
