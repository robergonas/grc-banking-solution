using GRC.Governance.Domain.Aggregates.CommitteeAggregate;

namespace GRC.Governance.Domain.Services;

/// <summary>
/// Domain service for complex governance operations that don't belong to a single aggregate
/// </summary>
public interface IGovernanceDomainService
{
    /// <summary>
    /// Check if a policy can be superseded by another policy
    /// </summary>
    Task<bool> CanSupersedePolicyAsync(Guid oldPolicyId, Guid newPolicyId);

    /// <summary>
    /// Validate committee quorum for a decision
    /// </summary>
    bool ValidateQuorumForDecision(Committee committee, int attendees);
}