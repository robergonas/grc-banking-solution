using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.ControlAggregate;

public class ControlFrequency : Enumeration
{
    public static ControlFrequency Continuous = new(1, nameof(Continuous));
    public static ControlFrequency Daily = new(2, nameof(Daily));
    public static ControlFrequency Weekly = new(3, nameof(Weekly));
    public static ControlFrequency Monthly = new(4, nameof(Monthly));
    public static ControlFrequency Quarterly = new(5, nameof(Quarterly));
    public static ControlFrequency Annually = new(6, nameof(Annually));

    public ControlFrequency(int id, string name) : base(id, name)
    {
    }
}