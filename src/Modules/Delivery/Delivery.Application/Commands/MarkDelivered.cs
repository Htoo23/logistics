using Delivery.Application.Contracts;
using MediatR;

namespace Delivery.Application.Commands;

public sealed record MarkDeliveredCommand(Guid DeliveryId, string RecipientName, string? Notes, string? PhotoUrl) : IRequest;

public sealed class MarkDeliveredHandler : IRequestHandler<MarkDeliveredCommand>
{
    private readonly IDeliveryRepository _repo;
    public MarkDeliveredHandler(IDeliveryRepository repo) => _repo = repo;

    public async Task Handle(MarkDeliveredCommand req, CancellationToken ct)
    {
        var delivery = await _repo.GetByIdAsync(req.DeliveryId, ct)
            ?? throw new InvalidOperationException("Delivery not found");

        delivery.MarkDelivered(req.RecipientName, req.Notes, req.PhotoUrl);
        await _repo.AddAsync(delivery, ct);
    }
}
