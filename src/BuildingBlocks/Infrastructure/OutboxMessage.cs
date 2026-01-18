using System.Text.Json;
using SharedKernel;

namespace BuildingBlocks.Infrastructure;

public sealed class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset OccurredAt { get; set; }
    public string Type { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public bool Processed { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }

    public static OutboxMessage FromDomainEvent(IDomainEvent evt)
        => new()
        {
            OccurredAt = evt.OccurredAt,
            Type = evt.GetType().FullName ?? evt.GetType().Name,
            Payload = JsonSerializer.Serialize(evt, evt.GetType())
        };
}
