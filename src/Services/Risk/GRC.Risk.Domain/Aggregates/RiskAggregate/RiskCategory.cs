using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.RiskAggregate;

public class RiskCategory : Enumeration
{
    public static RiskCategory Strategic = new(1, nameof(Strategic));
    public static RiskCategory Operational = new(2, nameof(Operational));
    public static RiskCategory Financial = new(3, nameof(Financial));
    public static RiskCategory Compliance = new(4, nameof(Compliance));
    public static RiskCategory Reputational = new(5, nameof(Reputational));
    public static RiskCategory Technology = new(6, nameof(Technology));
    public static RiskCategory Market = new(7, nameof(Market));
    public static RiskCategory Credit = new(8, nameof(Credit));
    public static RiskCategory Liquidity = new(9, nameof(Liquidity));

    public RiskCategory(int id, string name) : base(id, name)
    {
    }
}