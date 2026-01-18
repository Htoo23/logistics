using SharedKernel;

namespace Fleet.Domain;

public sealed class Driver : Entity
{
    private Driver() {}

    public string FullName { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public bool Active { get; private set; } = true;

    public static Driver Create(string fullName, string phone)
        => new() { FullName = fullName, Phone = phone, Active = true };

    public void Deactivate() => Active = false;
}
