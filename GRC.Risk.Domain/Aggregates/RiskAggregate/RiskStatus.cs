using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.RiskAggregate;

public class RiskStatus : Enumeration
{
    public static RiskStatus Identified = new(1, nameof(Identified));
    public static RiskStatus Assessed = new(2, nameof(Assessed));
    public static RiskStatus UnderTreatment = new(3, "Under Treatment");
    public static RiskStatus Mitigated = new(4, nameof(Mitigated));
    public static RiskStatus Accepted = new(5, nameof(Accepted));
    public static RiskStatus Closed = new(6, nameof(Closed));

    public RiskStatus(int id, string name) : base(id, name)
    {
    }
}