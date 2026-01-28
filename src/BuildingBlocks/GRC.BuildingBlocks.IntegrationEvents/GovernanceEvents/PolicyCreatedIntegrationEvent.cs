using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.GovernanceEvents;
public class PolicyCreatedIntegrationEvent : IntegrationEvent
{
    public Guid PolicyId { get; set; }
    public string PolicyCode { get; set; }
    public string PolicyName { get; set; }
    public string Category { get; set; }
    public DateTime EffectiveDate { get; set; }
    public List<string> ApplicableAreas { get; set; }
    public string CreatedBy { get; set; }
    public PolicyCreatedIntegrationEvent()
    {
        ApplicableAreas = new List<string>();
    }
    public PolicyCreatedIntegrationEvent(
        Guid policyId,
        string policyCode,
        string policyName,
        string category,
        DateTime effectiveDate,
        List<string> applicableAreas,
        string createdBy)
    {
        PolicyId = policyId;
        PolicyCode = policyCode;
        PolicyName = policyName;
        Category = category;
        EffectiveDate = effectiveDate;
        ApplicableAreas = applicableAreas ?? new List<string>();
        CreatedBy = createdBy;
    }
}