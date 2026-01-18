using SharedKernel;

namespace Delivery.Domain;

public sealed class DeliveryAggregate : Entity
{
    private DeliveryAggregate() {}

    public string TrackingNumber { get; private set; } = default!;
    public DeliveryStatus Status { get; private set; }

    public DateTimeOffset PlannedStart { get; private set; }
    public DateTimeOffset PlannedEnd { get; private set; }

    public Guid? AssignedDriverId { get; private set; }
    public Guid? AssignedVehicleId { get; private set; }

    public string CustomerName { get; private set; } = default!;
    public string CustomerPhone { get; private set; } = default!;

    public string PickupAddress { get; private set; } = default!;
    public string DropoffAddress { get; private set; } = default!;

    public decimal WeightKg { get; private set; }
    public decimal VolumeM3 { get; private set; }

    private readonly List<DeliveryStop> _stops = new();
    public IReadOnlyCollection<DeliveryStop> Stops => _stops;

    public ProofOfDelivery? Pod { get; private set; }

    public static DeliveryAggregate Create(
        string trackingNumber,
        string customerName,
        string customerPhone,
        string pickupAddress,
        string dropoffAddress,
        DateTimeOffset plannedStart,
        DateTimeOffset plannedEnd,
        decimal weightKg,
        decimal volumeM3)
    {
        return new DeliveryAggregate
        {
            TrackingNumber = trackingNumber,
            CustomerName = customerName,
            CustomerPhone = customerPhone,
            PickupAddress = pickupAddress,
            DropoffAddress = dropoffAddress,
            PlannedStart = plannedStart,
            PlannedEnd = plannedEnd,
            WeightKg = weightKg,
            VolumeM3 = volumeM3,
            Status = DeliveryStatus.Planned
        };
    }

    public void Assign(Guid driverId, Guid vehicleId)
    {
        AssignedDriverId = driverId;
        AssignedVehicleId = vehicleId;
        Status = DeliveryStatus.Assigned;
        AddDomainEvent(new Events.DeliveryAssigned(Id, driverId, vehicleId));
    }

    public void MarkEnRoute()
    {
        Status = DeliveryStatus.EnRoute;
        AddDomainEvent(new Events.DeliveryEnRoute(Id));
    }

    public void MarkDelivered(string recipientName, string? notes, string? photoUrl)
    {
        Status = DeliveryStatus.Delivered;
        Pod = ProofOfDelivery.Create(recipientName, notes, photoUrl);
        AddDomainEvent(new Events.DeliveryDelivered(Id));
    }
}
