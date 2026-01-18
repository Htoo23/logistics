using SharedKernel;

namespace Analytics.Domain;

public sealed class KpiSnapshot : Entity
{
    private KpiSnapshot() {}

    public DateOnly Day { get; private set; }
    public int DeliveriesPlanned { get; private set; }
    public int DeliveriesCompleted { get; private set; }
    public decimal OnTimeRatio { get; private set; }

    public static KpiSnapshot Create(DateOnly day, int planned, int completed, decimal onTimeRatio)
        => new() { Day = day, DeliveriesPlanned = planned, DeliveriesCompleted = completed, OnTimeRatio = onTimeRatio };
}
