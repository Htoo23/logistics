using Delivery.Application.Contracts;
using Delivery.Domain;
using MediatR;

namespace Delivery.Application.Commands;

public sealed record CreateDeliveryCommand(
    string TrackingNumber,
    string CustomerName,
    string CustomerPhone,
    string PickupAddress,
    string DropoffAddress,
    DateTimeOffset PlannedStart,
    DateTimeOffset PlannedEnd,
    decimal WeightKg,
    decimal VolumeM3
) : IRequest<Guid>;

public sealed class CreateDeliveryHandler : IRequestHandler<CreateDeliveryCommand, Guid>
{
    private readonly IDeliveryRepository _repo;
    public CreateDeliveryHandler(IDeliveryRepository repo) => _repo = repo;

    public async Task<Guid> Handle(CreateDeliveryCommand req, CancellationToken ct)
    {
        var delivery = DeliveryAggregate.Create(
            req.TrackingNumber,
            req.CustomerName,
            req.CustomerPhone,
            req.PickupAddress,
            req.DropoffAddress,
            req.PlannedStart,
            req.PlannedEnd,
            req.WeightKg,
            req.VolumeM3);

        await _repo.AddAsync(delivery, ct);
        return delivery.Id;
    }
}
