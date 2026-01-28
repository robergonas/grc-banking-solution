using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;

public class MitigationStrategy : Enumeration
{
    public static MitigationStrategy Avoid = new(1, nameof(Avoid));
    public static MitigationStrategy Reduce = new(2, nameof(Reduce));
    public static MitigationStrategy Transfer = new(3, nameof(Transfer));
    public static MitigationStrategy Accept = new(4, nameof(Accept));

    public MitigationStrategy(int id, string name) : base(id, name)
    {
    }
}