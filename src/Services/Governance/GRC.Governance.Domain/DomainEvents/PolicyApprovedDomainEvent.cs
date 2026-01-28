using GRC.BuildingBlocks.Domain.Exceptions;
using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using GRC.Governance.Domain.Aggregates.PolicyAggregate;

public class PolicyApprovedDomainEvent : IDomainEvent
{
    public Guid PolicyId { get; }
    public string Code { get; }
    public Guid ApprovedById { get; }
    public string ApprovedByName { get; }
    public DateTime OccurredOn { get; }

    public PolicyApprovedDomainEvent(Guid policyId, string code, Guid approvedById, string approvedByName)
    {
        PolicyId = policyId;
        Code = code;
        ApprovedById = approvedById;
        ApprovedByName = approvedByName;
        OccurredOn = DateTime.UtcNow;
    }
}