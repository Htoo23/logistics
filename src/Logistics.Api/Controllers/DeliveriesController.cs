using Delivery.Application.Commands;
using Delivery.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Api.Controllers;

[ApiController]
[Route("api/deliveries")]
public sealed class DeliveriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public DeliveriesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateDeliveryCommand cmd, CancellationToken ct)
        => Ok(await _mediator.Send(cmd, ct));

    [HttpPost("{id:guid}/assign")]
    public async Task<IActionResult> Assign(Guid id, [FromBody] AssignBody body, CancellationToken ct)
    {
        await _mediator.Send(new AssignDeliveryCommand(id, body.DriverId, body.VehicleId), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/delivered")]
    public async Task<IActionResult> MarkDelivered(Guid id, [FromBody] MarkDeliveredBody body, CancellationToken ct)
    {
        await _mediator.Send(new MarkDeliveredCommand(id, body.RecipientName, body.Notes, body.PhotoUrl), ct);
        return NoContent();
    }

    [HttpGet("tracking/{trackingNumber}")]
    public async Task<ActionResult<DeliveryDetailsDto?>> GetByTracking(string trackingNumber, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDeliveryByTrackingQuery(trackingNumber), ct));

    public sealed record AssignBody(Guid DriverId, Guid VehicleId);
    public sealed record MarkDeliveredBody(string RecipientName, string? Notes, string? PhotoUrl);
}
