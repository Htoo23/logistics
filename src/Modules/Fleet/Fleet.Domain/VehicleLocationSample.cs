using NetTopologySuite.Geometries;

namespace Fleet.Domain;

public sealed class VehicleLocationSample
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid VehicleId { get; private set; }
    public DateTimeOffset RecordedAt { get; private set; }

    // PostGIS geometry(Point,4326)
    public Point Position { get; private set; } = default!;

    public double SpeedKph { get; private set; }
    public double HeadingDeg { get; private set; }

    private VehicleLocationSample() {}

    public static VehicleLocationSample Create(Guid vehicleId, DateTimeOffset recordedAt, Point pos, double speedKph, double headingDeg)
        => new()
        {
            VehicleId = vehicleId,
            RecordedAt = recordedAt,
            Position = pos,
            SpeedKph = speedKph,
            HeadingDeg = headingDeg
        };
}
