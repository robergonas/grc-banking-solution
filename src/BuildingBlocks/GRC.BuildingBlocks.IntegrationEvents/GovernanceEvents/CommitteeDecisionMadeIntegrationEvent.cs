using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.GovernanceEvents;
public class CommitteeDecisionMadeIntegrationEvent : IntegrationEvent
{
    public Guid CommitteeId { get; set; }
    public string CommitteeName { get; set; }
    public Guid DecisionId { get; set; }
    public string DecisionType { get; set; }
    public string Subject { get; set; }
    public List<Guid> ImpactedPolicies { get; set; }
    public DateTime DecisionDate { get; set; }
    public CommitteeDecisionMadeIntegrationEvent()
    {
        ImpactedPolicies = new List<Guid>();
    }
    public CommitteeDecisionMadeIntegrationEvent(
        Guid committeeId,
        string committeeName,
        Guid decisionId,
        string decisionType,
        string subject,
        List<Guid> impactedPolicies)
    {
        CommitteeId = committeeId;
        CommitteeName = committeeName;
        DecisionId = decisionId;
        DecisionType = decisionType;
        Subject = subject;
        ImpactedPolicies = impactedPolicies !=null ? impactedPolicies: new List<Guid>();
        DecisionDate = DateTime.UtcNow;
    }
}