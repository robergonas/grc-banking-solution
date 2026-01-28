using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using GRC.Governance.Domain.Exceptions;

public class CommitteeMeeting : Entity
{
    public Guid CommitteeId { get; private set; }
    public string Title { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public DateTime? ActualStartTime { get; private set; }
    public DateTime? ActualEndTime { get; private set; }
    public string Agenda { get; private set; }
    public string Location { get; private set; }
    public string MeetingLink { get; private set; }
    public MeetingStatus Status { get; private set; }
    public string Minutes { get; private set; }
    public int AttendeesCount { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    private readonly List<DecisionRecord> _decisions;
    public IReadOnlyCollection<DecisionRecord> Decisions => _decisions.AsReadOnly();

    private CommitteeMeeting()
    {
        _decisions = new List<DecisionRecord>();
    }

    public CommitteeMeeting(
        Guid committeeId,
        string title,
        DateTime scheduledDate,
        string agenda,
        string location = null,
        string meetingLink = null)
    {
        Id = Guid.NewGuid();
        CommitteeId = committeeId;
        Title = title;
        ScheduledDate = scheduledDate;
        Agenda = agenda;
        Location = location;
        MeetingLink = meetingLink;
        Status = MeetingStatus.Scheduled;
        CreatedAt = DateTime.UtcNow;
        _decisions = new List<DecisionRecord>();
    }

    public void Start(int attendeesCount)
    {
        if (Status != MeetingStatus.Scheduled)
            throw new GovernanceDomainException("Meeting is not in scheduled status");

        Status = MeetingStatus.InProgress;
        ActualStartTime = DateTime.UtcNow;
        AttendeesCount = attendeesCount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete(string minutes, List<DecisionRecord> decisions)
    {
        if (Status != MeetingStatus.InProgress && Status != MeetingStatus.Scheduled)
            throw new GovernanceDomainException("Meeting must be in progress or scheduled");

        Status = MeetingStatus.Completed;
        ActualEndTime = DateTime.UtcNow;
        Minutes = minutes;

        if (decisions != null)
        {
            foreach (var decision in decisions)
            {
                _decisions.Add(decision);
            }
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (Status == MeetingStatus.Completed)
            throw new GovernanceDomainException("Cannot cancel a completed meeting");

        Status = MeetingStatus.Cancelled;
        Minutes = $"Cancelled: {reason}";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reschedule(DateTime newDate)
    {
        if (Status != MeetingStatus.Scheduled)
            throw new GovernanceDomainException("Can only reschedule scheduled meetings");

        ScheduledDate = newDate;
        UpdatedAt = DateTime.UtcNow;
    }
}