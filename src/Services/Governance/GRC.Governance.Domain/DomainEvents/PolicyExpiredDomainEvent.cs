using GRC.BuildingBlocks.Domain.Exceptions;
using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using GRC.Governance.Domain.Aggregates.PolicyAggregate;

public class PolicyExpiredDomainEvent : IDomainEvent
{
    public Guid PolicyId { get; }
    public string Code { get; }
    public DateTime OccurredOn { get; }

    public PolicyExpiredDomainEvent(Guid policyId, string code)
    {
        PolicyId = policyId;
        Code = code;
        OccurredOn = DateTime.UtcNow;
    }
}

