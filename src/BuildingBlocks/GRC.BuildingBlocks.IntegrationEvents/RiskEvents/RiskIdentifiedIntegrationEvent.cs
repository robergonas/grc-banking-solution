using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.RiskEvents;
public class RiskIdentifiedIntegrationEvent : IntegrationEvent
{
    public Guid RiskId { get; set; }
    public string RiskCode { get; set; }
    public string RiskName { get; set; }
    public string Category { get; set; }
    public string InherentRiskLevel { get; set; }
    public List<string> AffectedAreas { get; set; }
    public Guid IdentifiedBy { get; set; }
    public DateTime IdentificationDate { get; set; }
    public decimal PotentialImpact { get; set; }
    public decimal Probability { get; set; }
    public RiskIdentifiedIntegrationEvent()
    {
        AffectedAreas = new List<string>();
    }
    public RiskIdentifiedIntegrationEvent(
        Guid riskId,
        string riskCode,
        string riskName,
        string category,
        string inherentRiskLevel,
        List<string> affectedAreas,
        Guid identifiedBy,
        decimal potentialImpact,
        decimal probability)
    {
        RiskId = riskId;
        RiskCode = riskCode;
        RiskName = riskName;
        Category = category;
        InherentRiskLevel = inherentRiskLevel;
        AffectedAreas = affectedAreas ?? new List<string>();
        IdentifiedBy = identifiedBy;
        IdentificationDate = DateTime.UtcNow;
        PotentialImpact = potentialImpact;
        Probability = probability;
    }
}