using GRC.BuildingBlocks.EventBus.Events;
using System;

namespace GRC.BuildingBlocks.IntegrationEvents.GovernanceEvents;

public class PolicyExpiredIntegrationEvent : IntegrationEvent
{
    public Guid PolicyId { get; set; }
    public string PolicyCode { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool RequiresRenewal { get; set; }

    public PolicyExpiredIntegrationEvent()
    {
    }

    public PolicyExpiredIntegrationEvent(
        Guid policyId,
        string policyCode,
        DateTime expirationDate,
        bool requiresRenewal)
    {
        PolicyId = policyId;
        PolicyCode = policyCode;
        ExpirationDate = expirationDate;
        RequiresRenewal = requiresRenewal;
    }
}