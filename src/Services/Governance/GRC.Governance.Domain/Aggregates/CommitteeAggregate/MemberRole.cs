using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Governance.Domain.Aggregates.CommitteeAggregate; // ← AGREGAR ESTO

public class MemberRole : Enumeration
{
    public static MemberRole Chairperson = new(1, nameof(Chairperson));
    public static MemberRole ViceChair = new(2, nameof(ViceChair));
    public static MemberRole Secretary = new(3, nameof(Secretary));
    public static MemberRole Member = new(4, nameof(Member));
    public static MemberRole Observer = new(5, nameof(Observer));

    public MemberRole(int id, string name) : base(id, name) { }
}