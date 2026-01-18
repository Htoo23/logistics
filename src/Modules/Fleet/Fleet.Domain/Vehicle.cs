using SharedKernel;

namespace Fleet.Domain;

public sealed class Vehicle : Entity
{
    private Vehicle() {}

    public string PlateNumber { get; private set; } = default!;
    public decimal CapacityKg { get; private set; }
    public decimal CapacityM3 { get; private set; }
    public bool Active { get; private set; } = true;

    public static Vehicle Create(string plateNumber, decimal capacityKg, decimal capacityM3)
        => new() { PlateNumber = plateNumber, CapacityKg = capacityKg, CapacityM3 = capacityM3, Active = true };

    public void Deactivate() => Active = false;
}
