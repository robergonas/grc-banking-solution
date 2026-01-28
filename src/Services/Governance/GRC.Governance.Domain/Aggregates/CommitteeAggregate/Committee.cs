using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.DomainEvents;
using GRC.Governance.Domain.Exceptions;

namespace GRC.Governance.Domain.Aggregates.CommitteeAggregate;

/// <summary>
/// Committee Aggregate Root - Representa un comité de gobernanza
/// </summary>
public class Committee : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public CommitteeType Type { get; private set; }
    public CommitteeStatus Status { get; private set; }
    public string Charter { get; private set; } // Carta constitutiva

    // Chairperson
    public Guid ChairpersonId { get; private set; }
    public string ChairpersonName { get; private set; }

    // Meeting schedule
    public MeetingFrequency MeetingFrequency { get; private set; }
    public DateTime? LastMeetingDate { get; private set; }
    public DateTime? NextMeetingDate { get; private set; }

    // Quorum requirements
    public int MinimumQuorum { get; private set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    private readonly List<CommitteeMember> _members;
    public IReadOnlyCollection<CommitteeMember> Members => _members.AsReadOnly();

    private readonly List<CommitteeMeeting> _meetings;
    public IReadOnlyCollection<CommitteeMeeting> Meetings => _meetings.AsReadOnly();

    // EF Core constructor
    private Committee()
    {
        _members = new List<CommitteeMember>();
        _meetings = new List<CommitteeMeeting>();
    }

    public Committee(
        string name,
        string description,
        CommitteeType type,
        Guid chairpersonId,
        string chairpersonName,
        MeetingFrequency meetingFrequency,
        int minimumQuorum = 3)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new GovernanceDomainException("Committee name cannot be empty");

        if (minimumQuorum < 2)
            throw new GovernanceDomainException("Minimum quorum must be at least 2 members");

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Type = type;
        Status = CommitteeStatus.Active;
        ChairpersonId = chairpersonId;
        ChairpersonName = chairpersonName;
        MeetingFrequency = meetingFrequency;
        MinimumQuorum = minimumQuorum;
        CreatedAt = DateTime.UtcNow;

        _members = new List<CommitteeMember>();
        _meetings = new List<CommitteeMeeting>();

        // Auto-add chairperson as member
        AddMember(chairpersonId, chairpersonName, "Chairperson", MemberRole.Chairperson);
    }

    public void UpdateDetails(string name, string description)
    {
        Name = name ?? Name;
        Description = description ?? Description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCharter(string charter)
    {
        Charter = charter;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeChairperson(Guid newChairpersonId, string newChairpersonName)
    {
        // Remove chairperson role from old chairperson
        var oldChair = _members.FirstOrDefault(m => m.UserId == ChairpersonId);
        if (oldChair != null)
        {
            oldChair.UpdateRole(MemberRole.Member);
        }

        ChairpersonId = newChairpersonId;
        ChairpersonName = newChairpersonName;

        // Add or update new chairperson
        var newChair = _members.FirstOrDefault(m => m.UserId == newChairpersonId);
        if (newChair != null)
        {
            newChair.UpdateRole(MemberRole.Chairperson);
        }
        else
        {
            AddMember(newChairpersonId, newChairpersonName, "Chairperson", MemberRole.Chairperson);
        }

        UpdatedAt = DateTime.UtcNow;
    }
    public void AddMember(Guid userId, string userName, string position, MemberRole role = null)
    {
        if (_members.Any(m => m.UserId == userId && m.IsActive))
            throw new GovernanceDomainException("User is already a member of this committee");

        var memberRole = role ?? MemberRole.Member;
        var member = new CommitteeMember(userId, userName, position, memberRole);
        _members.Add(member);
        UpdatedAt = DateTime.UtcNow;
    }
    public void RemoveMember(Guid userId)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId && m.IsActive);
        if (member == null)
            throw new GovernanceDomainException("Member not found in committee");

        if (member.UserId == ChairpersonId)
            throw new GovernanceDomainException("Cannot remove chairperson. Assign a new chairperson first");

        member.Deactivate();
        UpdatedAt = DateTime.UtcNow;
    }

    public CommitteeMeeting ScheduleMeeting(
        string title,
        DateTime scheduledDate,
        string agenda,
        string location = null,
        string meetingLink = null)
    {
        var meeting = new CommitteeMeeting(Id, title, scheduledDate, agenda, location, meetingLink);
        _meetings.Add(meeting);

        NextMeetingDate = scheduledDate;
        UpdatedAt = DateTime.UtcNow;

        return meeting;
    }

    public void CompleteMeeting(Guid meetingId, string minutes, List<DecisionRecord> decisions)
    {
        var meeting = _meetings.FirstOrDefault(m => m.Id == meetingId);
        if (meeting == null)
            throw new GovernanceDomainException("Meeting not found");

        meeting.Complete(minutes, decisions);
        LastMeetingDate = meeting.ActualStartTime ?? meeting.ScheduledDate;

        // Calculate next meeting date based on frequency
        CalculateNextMeetingDate();

        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CommitteeDecisionMadeDomainEvent(Id, Name, meetingId, decisions.Count));
    }

    public void CancelMeeting(Guid meetingId, string reason)
    {
        var meeting = _meetings.FirstOrDefault(m => m.Id == meetingId);
        if (meeting == null)
            throw new GovernanceDomainException("Meeting not found");

        meeting.Cancel(reason);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend(string reason)
    {
        Status = CommitteeStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        if (Status != CommitteeStatus.Suspended)
            throw new GovernanceDomainException("Can only reactivate suspended committees");

        Status = CommitteeStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Dissolve(string reason)
    {
        Status = CommitteeStatus.Dissolved;

        // Deactivate all members
        foreach (var member in _members.Where(m => m.IsActive))
        {
            member.Deactivate();
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasQuorum(int attendeeCount)
    {
        return attendeeCount >= MinimumQuorum;
    }

    private void CalculateNextMeetingDate()
    {
        if (!LastMeetingDate.HasValue) return;

        NextMeetingDate = MeetingFrequency.Id switch
        {
            1 => LastMeetingDate.Value.AddDays(7),    // Weekly
            2 => LastMeetingDate.Value.AddDays(14),   // Biweekly
            3 => LastMeetingDate.Value.AddMonths(1),  // Monthly
            4 => LastMeetingDate.Value.AddMonths(3),  // Quarterly
            5 => LastMeetingDate.Value.AddMonths(6),  // Semiannually
            6 => LastMeetingDate.Value.AddYears(1),   // Annually
            _ => null
        };
    }
}