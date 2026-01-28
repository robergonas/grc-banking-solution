using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;

public class CommitteeMember : Entity
{
    public Guid CommitteeId { get; private set; }
    public Guid UserId { get; private set; }
    public string UserName { get; private set; }
    public string Position { get; private set; }
    public MemberRole Role { get; private set; }
    public DateTime JoinedDate { get; private set; }
    public DateTime? LeftDate { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    private CommitteeMember() { }

    public CommitteeMember(Guid userId, string userName, string position, MemberRole role)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        UserName = userName;
        Position = position;
        Role = role;
        JoinedDate = DateTime.UtcNow;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateRole(MemberRole newRole)
    {
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        LeftDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}