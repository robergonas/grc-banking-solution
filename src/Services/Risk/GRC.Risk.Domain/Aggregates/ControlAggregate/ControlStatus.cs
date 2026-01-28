using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.ControlAggregate;

public class ControlStatus : Enumeration
{
    public static ControlStatus Planned = new(1, nameof(Planned));
    public static ControlStatus InProgress = new(2, "In Progress");
    public static ControlStatus Implemented = new(3, nameof(Implemented));
    public static ControlStatus UnderReview = new(4, "Under Review");
    public static ControlStatus Inactive = new(5, nameof(Inactive));

    public ControlStatus(int id, string name) : base(id, name)
    {
    }
}