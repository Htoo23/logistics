namespace Delivery.Domain;

public sealed class DeliveryStop
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int Sequence { get; private set; }
    public string Address { get; private set; } = default!;
    public DateTimeOffset? Eta { get; private set; }

    private DeliveryStop() {}

    public static DeliveryStop Create(int sequence, string address)
        => new() { Sequence = sequence, Address = address };

    public void SetEta(DateTimeOffset eta) => Eta = eta;
}
