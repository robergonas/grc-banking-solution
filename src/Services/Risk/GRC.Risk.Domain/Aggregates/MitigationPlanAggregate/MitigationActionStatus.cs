using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;

public class MitigationActionStatus : Enumeration
{
    public static MitigationActionStatus NotStarted = new(1, "Not Started");
    public static MitigationActionStatus InProgress = new(2, "In Progress");
    public static MitigationActionStatus Completed = new(3, nameof(Completed));
    public static MitigationActionStatus Blocked = new(4, nameof(Blocked));

    public MitigationActionStatus(int id, string name) : base(id, name)
    {
    }
}