namespace Delivery.Domain;

public enum DeliveryStatus
{
    Draft = 0,
    Planned = 1,
    Assigned = 2,
    EnRoute = 3,
    Delivered = 4,
    Failed = 5,
    Cancelled = 6
}
