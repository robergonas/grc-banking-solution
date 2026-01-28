using GRC.BuildingBlocks.EventBus.Events;
using System;

namespace GRC.BuildingBlocks.IntegrationEvents.GovernanceEvents;

public class PolicyApprovedIntegrationEvent : IntegrationEvent
{
    public Guid PolicyId { get; set; }
    public string PolicyCode { get; set; }
    public Guid ApprovedBy { get; set; }
    public string ApproverRole { get; set; }
    public DateTime ApprovalDate { get; set; }
    public int ApprovalLevel { get; set; }

    public PolicyApprovedIntegrationEvent()
    {
    }

    public PolicyApprovedIntegrationEvent(
        Guid policyId,
        string policyCode,
        Guid approvedBy,
        string approverRole,
        int approvalLevel)
    {
        PolicyId = policyId;
        PolicyCode = policyCode;
        ApprovedBy = approvedBy;
        ApproverRole = approverRole;
        ApprovalLevel = approvalLevel;
        ApprovalDate = DateTime.UtcNow;
    }
}