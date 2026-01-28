using GRC.BuildingBlocks.Domain.SeedWork;

public class DecisionType : Enumeration
{
    public static DecisionType PolicyApproval = new(1, nameof(PolicyApproval));
    public static DecisionType RiskAcceptance = new(2, nameof(RiskAcceptance));
    public static DecisionType BudgetApproval = new(3, nameof(BudgetApproval));
    public static DecisionType Strategic = new(4, nameof(Strategic));
    public static DecisionType Operational = new(5, nameof(Operational));
    public static DecisionType Compliance = new(6, nameof(Compliance));

    public DecisionType(int id, string name) : base(id, name) { }
}