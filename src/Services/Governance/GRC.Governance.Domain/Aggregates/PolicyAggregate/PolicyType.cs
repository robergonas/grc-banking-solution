using GRC.BuildingBlocks.Domain.SeedWork;

public class PolicyType : Enumeration
{
    public static PolicyType Corporate = new(1, nameof(Corporate));
    public static PolicyType Operational = new(2, nameof(Operational));
    public static PolicyType Security = new(3, nameof(Security));
    public static PolicyType Compliance = new(4, nameof(Compliance));
    public static PolicyType Risk = new(5, nameof(Risk));
    public static PolicyType IT = new(6, nameof(IT));
    public static PolicyType HR = new(7, nameof(HR));
    public static PolicyType Financial = new(8, nameof(Financial));

    public PolicyType(int id, string name) : base(id, name) { }
}