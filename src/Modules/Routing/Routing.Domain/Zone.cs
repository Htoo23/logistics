using NetTopologySuite.Geometries;
using SharedKernel;

namespace Routing.Domain;

public sealed class Zone : Entity
{
    private Zone() {}

    public string Name { get; private set; } = default!;
    public Polygon Area { get; private set; } = default!; // SRID 4326

    public static Zone Create(string name, Polygon area)
    {
        area.SRID = 4326;
        return new Zone { Name = name, Area = area };
    }
}
