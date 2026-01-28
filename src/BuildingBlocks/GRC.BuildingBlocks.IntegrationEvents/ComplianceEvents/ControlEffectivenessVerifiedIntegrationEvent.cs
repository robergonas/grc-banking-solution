using GRC.BuildingBlocks.EventBus.Events;
using System;

namespace GRC.BuildingBlocks.IntegrationEvents.ComplianceEvents;

public class ControlEffectivenessVerifiedIntegrationEvent : IntegrationEvent
{
    public Guid ControlId { get; set; }
    public Guid RiskId { get; set; }
    public string EffectivenessRating { get; set; }
    public DateTime VerificationDate { get; set; }
    public Guid VerifiedBy { get; set; }
    public string VerificationMethod { get; set; }
    public bool IsEffective { get; set; }
    public string Findings { get; set; }
    public string RecommendedActions { get; set; }

    public ControlEffectivenessVerifiedIntegrationEvent()
    {
    }

    public ControlEffectivenessVerifiedIntegrationEvent(
        Guid controlId,
        Guid riskId,
        string effectivenessRating,
        Guid verifiedBy,
        string verificationMethod,
        bool isEffective,
        string findings,
        string recommendedActions)
    {
        ControlId = controlId;
        RiskId = riskId;
        EffectivenessRating = effectivenessRating;
        VerificationDate = DateTime.UtcNow;
        VerifiedBy = verifiedBy;
        VerificationMethod = verificationMethod;
        IsEffective = isEffective;
        Findings = findings;
        RecommendedActions = recommendedActions;
    }
}