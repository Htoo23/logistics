using Delivery.Domain;

namespace Delivery.Application;

public sealed record DeliveryDto(
    Guid Id,
    string TrackingNumber,
    DeliveryStatus Status,
    string CustomerName,
    string CustomerPhone,
    string PickupAddress,
    string DropoffAddress,
    DateTimeOffset PlannedStart,
    DateTimeOffset PlannedEnd,
    Guid? AssignedDriverId,
    Guid? AssignedVehicleId,
    DateTimeOffset? DeliveredAt
);
