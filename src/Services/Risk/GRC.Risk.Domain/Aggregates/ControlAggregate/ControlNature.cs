using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.ControlAggregate;

public class ControlNature : Enumeration
{
    public static ControlNature Manual = new(1, nameof(Manual));
    public static ControlNature Automated = new(2, nameof(Automated));
    public static ControlNature SemiAutomated = new(3, "Semi-Automated");

    public ControlNature(int id, string name) : base(id, name)
    {
    }
}