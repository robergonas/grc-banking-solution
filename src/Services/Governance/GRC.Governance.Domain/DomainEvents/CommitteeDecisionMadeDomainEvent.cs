using GRC.BuildingBlocks.Domain.Exceptions;
using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using GRC.Governance.Domain.Aggregates.PolicyAggregate;

public class CommitteeDecisionMadeDomainEvent : IDomainEvent
{
    public Guid CommitteeId { get; }
    public string CommitteeName { get; }
    public Guid MeetingId { get; }
    public int DecisionsCount { get; }
    public DateTime OccurredOn { get; }

    public CommitteeDecisionMadeDomainEvent(
        Guid committeeId,
        string committeeName,
        Guid meetingId,
        int decisionsCount)
    {
        CommitteeId = committeeId;
        CommitteeName = committeeName;
        MeetingId = meetingId;
        DecisionsCount = decisionsCount;
        OccurredOn = DateTime.UtcNow;
    }
}