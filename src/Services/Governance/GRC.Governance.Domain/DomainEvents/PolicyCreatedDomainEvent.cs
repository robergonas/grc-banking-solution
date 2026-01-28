using GRC.BuildingBlocks.Domain.Exceptions;
using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using GRC.Governance.Domain.Aggregates.PolicyAggregate;

namespace GRC.Governance.Domain.DomainEvents;
public class PolicyCreatedDomainEvent : IDomainEvent
{
    public Guid PolicyId { get; }
    public string Code { get; }
    public string Title { get; }
    public PolicyType Type { get; }
    public DateTime OccurredOn { get; }

    public PolicyCreatedDomainEvent(Guid policyId, string code, string title, PolicyType type)
    {
        PolicyId = policyId;
        Code = code;
        Title = title;
        Type = type;
        OccurredOn = DateTime.UtcNow;
    }
}