namespace Delivery.Domain;

public sealed class ProofOfDelivery
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTimeOffset DeliveredAt { get; private set; }
    public string RecipientName { get; private set; } = default!;
    public string? Notes { get; private set; }
    public string? PhotoUrl { get; private set; }

    private ProofOfDelivery() {}

    public static ProofOfDelivery Create(string recipientName, string? notes, string? photoUrl)
        => new()
        {
            DeliveredAt = DateTimeOffset.UtcNow,
            RecipientName = recipientName,
            Notes = notes,
            PhotoUrl = photoUrl
        };
}
