using Fleet.Application.Contracts;
using Fleet.Domain;
using MediatR;

namespace Fleet.Application.Commands;

public sealed record IngestLocationCommand(Guid VehicleId, DateTimeOffset RecordedAt, double Lon, double Lat, decimal? SpeedKph) : IRequest;

public sealed class IngestLocationHandler : IRequestHandler<IngestLocationCommand>
{
    private readonly IFleetRepository _repo;
    public IngestLocationHandler(IFleetRepository repo) => _repo = repo;

    public async Task Handle(IngestLocationCommand req, CancellationToken ct)
    {
        var loc = VehicleLocation.Create(req.VehicleId, req.RecordedAt, req.Lon, req.Lat, req.SpeedKph);
        await _repo.AddLocationAsync(loc, ct);
    }
}
