using SharedKernel;

namespace Routing.Domain;

public sealed class RoutePlan : Entity
{
    private RoutePlan() {}

    public string RouteCode { get; private set; } = default!;
    public DateOnly ServiceDate { get; private set; }
    public Guid VehicleId { get; private set; }
    public Guid DriverId { get; private set; }

    private readonly List<RouteStop> _stops = new();
    public IReadOnlyCollection<RouteStop> Stops => _stops;

    public static RoutePlan Create(string routeCode, DateOnly serviceDate, Guid vehicleId, Guid driverId)
        => new() { RouteCode = routeCode, ServiceDate = serviceDate, VehicleId = vehicleId, DriverId = driverId };

    public void AddStop(Guid deliveryId, int sequence)
        => _stops.Add(RouteStop.Create(deliveryId, sequence));
}

public sealed class RouteStop
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid DeliveryId { get; private set; }
    public int Sequence { get; private set; }
    public DateTimeOffset? Eta { get; private set; }

    private RouteStop() {}

    public static RouteStop Create(Guid deliveryId, int sequence)
        => new() { DeliveryId = deliveryId, Sequence = sequence };

    public void SetEta(DateTimeOffset eta) => Eta = eta;
}
