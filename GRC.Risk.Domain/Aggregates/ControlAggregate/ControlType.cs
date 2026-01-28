using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.ControlAggregate;

public class ControlType : Enumeration
{
    public static ControlType Preventive = new(1, nameof(Preventive));
    public static ControlType Detective = new(2, nameof(Detective));
    public static ControlType Corrective = new(3, nameof(Corrective));
    public static ControlType Directive = new(4, nameof(Directive));

    public ControlType(int id, string name) : base(id, name)
    {
    }
}