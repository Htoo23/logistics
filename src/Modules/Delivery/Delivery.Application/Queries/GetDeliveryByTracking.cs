using Delivery.Application.Contracts;
using MediatR;

namespace Delivery.Application.Queries;

public sealed record DeliveryDetailsDto(
    Guid Id,
    string TrackingNumber,
    string Status,
    Guid? AssignedDriverId,
    Guid? AssignedVehicleId,
    string CustomerName,
    string PickupAddress,
    string DropoffAddress,
    DateTimeOffset PlannedStart,
    DateTimeOffset PlannedEnd,
    DateTimeOffset? DeliveredAt,
    string? PodRecipientName,
    string? PodPhotoUrl
);

public sealed record GetDeliveryByTrackingQuery(string TrackingNumber) : IRequest<DeliveryDetailsDto?>;

public sealed class GetDeliveryByTrackingHandler : IRequestHandler<GetDeliveryByTrackingQuery, DeliveryDetailsDto?>
{
    private readonly IDeliveryRepository _repo;
    public GetDeliveryByTrackingHandler(IDeliveryRepository repo) => _repo = repo;

    public async Task<DeliveryDetailsDto?> Handle(GetDeliveryByTrackingQuery req, CancellationToken ct)
    {
        var d = await _repo.GetByTrackingNumberAsync(req.TrackingNumber, ct);
        if (d is null) return null;

        return new DeliveryDetailsDto(
            d.Id,
            d.TrackingNumber,
            d.Status.ToString(),
            d.AssignedDriverId,
            d.AssignedVehicleId,
            d.CustomerName,
            d.PickupAddress,
            d.DropoffAddress,
            d.PlannedStart,
            d.PlannedEnd,
            d.Pod?.DeliveredAt,
            d.Pod?.RecipientName,
            d.Pod?.PhotoUrl
        );
    }
}
