using GRC.BuildingBlocks.Domain.SeedWork;

public class PolicyStatus : Enumeration
{
    public static PolicyStatus Draft = new(1, nameof(Draft));
    public static PolicyStatus PendingApproval = new(2, nameof(PendingApproval));
    public static PolicyStatus Approved = new(3, nameof(Approved));
    public static PolicyStatus Active = new(4, nameof(Active));
    public static PolicyStatus UnderReview = new(5, nameof(UnderReview));
    public static PolicyStatus Expired = new(6, nameof(Expired));
    public static PolicyStatus Archived = new(7, nameof(Archived));

    public PolicyStatus(int id, string name) : base(id, name) { }
}