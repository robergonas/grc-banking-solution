using GRC.BuildingBlocks.Domain.SeedWork;

public class CommitteeType : Enumeration
{
    public static CommitteeType Board = new(1, nameof(Board));
    public static CommitteeType Audit = new(2, nameof(Audit));
    public static CommitteeType Risk = new(3, nameof(Risk));
    public static CommitteeType Compliance = new(4, nameof(Compliance));
    public static CommitteeType Ethics = new(5, nameof(Ethics));
    public static CommitteeType IT = new(6, nameof(IT));
    public static CommitteeType Investment = new(7, nameof(Investment));
    public static CommitteeType Executive = new(8, nameof(Executive));

    public CommitteeType(int id, string name) : base(id, name) { }
}