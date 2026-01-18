using NetTopologySuite.Geometries;

namespace Fleet.Domain;

public sealed class VehicleLocation
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid VehicleId { get; private set; }
    public DateTimeOffset RecordedAt { get; private set; }
    public Point Position { get; private set; } = default!; // SRID 4326
    public decimal? SpeedKph { get; private set; }

    private VehicleLocation() {}

    public static VehicleLocation Create(Guid vehicleId, DateTimeOffset recordedAt, double lon, double lat, decimal? speedKph)
        => new()
        {
            VehicleId = vehicleId,
            RecordedAt = recordedAt,
            Position = new Point(lon, lat) { SRID = 4326 },
            SpeedKph = speedKph
        };
}
