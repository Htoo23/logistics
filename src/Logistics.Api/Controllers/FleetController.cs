using Fleet.Application.Commands;
using Logistics.Api.RealTime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Api.Controllers;

[ApiController]
[Route("api/fleet")]
public sealed class FleetController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IVehicleLocationBroadcaster _broadcaster;

    public FleetController(IMediator mediator, IVehicleLocationBroadcaster broadcaster)
    {
        _mediator = mediator;
        _broadcaster = broadcaster;
    }

    /// <summary>
    /// </summary>
    [HttpPost("vehicles/{vehicleId:guid}/locations")]
    public async Task<IActionResult> IngestLocation(Guid vehicleId, [FromBody] LocationBody body, CancellationToken ct)
    {
        await _mediator.Send(new IngestLocationCommand(vehicleId, body.RecordedAt, body.Lon, body.Lat, body.SpeedKph), ct);

        await _broadcaster.BroadcastLocationAsync(vehicleId, new
        {
            vehicleId,
            body.RecordedAt,
            body.Lon,
            body.Lat,
            body.SpeedKph
        }, ct);

        return Accepted();
    }

    public sealed record LocationBody(DateTimeOffset RecordedAt, double Lon, double Lat, decimal? SpeedKph);
}
