using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.RiskAggregate;

public class RiskType : Enumeration
{
    public static RiskType Internal = new(1, nameof(Internal));
    public static RiskType External = new(2, nameof(External));
    public static RiskType Both = new(3, nameof(Both));

    public RiskType(int id, string name) : base(id, name)
    {
    }
}