using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;

public class MitigationPlanStatus : Enumeration
{
    public static MitigationPlanStatus Planned = new(1, nameof(Planned));
    public static MitigationPlanStatus InProgress = new(2, "In Progress");
    public static MitigationPlanStatus Completed = new(3, nameof(Completed));
    public static MitigationPlanStatus OnHold = new(4, "On Hold");
    public static MitigationPlanStatus Cancelled = new(5, nameof(Cancelled));

    public MitigationPlanStatus(int id, string name) : base(id, name)
    {
    }
}