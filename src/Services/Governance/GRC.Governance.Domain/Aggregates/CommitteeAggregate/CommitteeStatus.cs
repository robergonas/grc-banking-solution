
using GRC.BuildingBlocks.Domain.SeedWork;
public class CommitteeStatus : Enumeration
{
    public static CommitteeStatus Active = new(1, nameof(Active));
    public static CommitteeStatus Suspended = new(2, nameof(Suspended));
    public static CommitteeStatus Dissolved = new(3, nameof(Dissolved));

    public CommitteeStatus(int id, string name) : base(id, name) { }
}