using Delivery.Application.Contracts;
using MediatR;

namespace Delivery.Application.Commands;

public sealed record AssignDeliveryCommand(Guid DeliveryId, Guid DriverId, Guid VehicleId) : IRequest;

public sealed class AssignDeliveryHandler : IRequestHandler<AssignDeliveryCommand>
{
    private readonly IDeliveryRepository _repo;
    public AssignDeliveryHandler(IDeliveryRepository repo) => _repo = repo;

    public async Task Handle(AssignDeliveryCommand req, CancellationToken ct)
    {
        var delivery = await _repo.GetByIdAsync(req.DeliveryId, ct)
            ?? throw new InvalidOperationException("Delivery not found");

        delivery.Assign(req.DriverId, req.VehicleId);
        await _repo.AddAsync(delivery, ct); 
    }
}
