using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.RiskEvents;

public class RiskAssessmentCompletedIntegrationEvent : IntegrationEvent
{
    public Guid AssessmentId { get; set; }
    public Guid RiskId { get; set; }
    public string RiskCode { get; set; }
    public string ResidualRiskLevel { get; set; }
    public decimal InherentRiskScore { get; set; }
    public decimal ResidualRiskScore { get; set; }
    public List<Guid> RecommendedControls { get; set; }
    public Guid AssessedBy { get; set; }
    public DateTime AssessmentDate { get; set; }
    public bool RequiresImmediateAction { get; set; }
    public RiskAssessmentCompletedIntegrationEvent()
    {
        RecommendedControls = new List<Guid>();
    }
    public RiskAssessmentCompletedIntegrationEvent(
        Guid assessmentId,
        Guid riskId,
        string riskCode,
        string residualRiskLevel,
        decimal inherentRiskScore,
        decimal residualRiskScore,
        List<Guid> recommendedControls,
        Guid assessedBy,
        bool requiresImmediateAction)
    {
        AssessmentId = assessmentId;
        RiskId = riskId;
        RiskCode = riskCode;
        ResidualRiskLevel = residualRiskLevel;
        InherentRiskScore = inherentRiskScore;
        ResidualRiskScore = residualRiskScore;
        RecommendedControls = recommendedControls != null ? recommendedControls : new List<Guid>();
        AssessedBy = assessedBy;
        AssessmentDate = DateTime.UtcNow;
        RequiresImmediateAction = requiresImmediateAction;
    }
}