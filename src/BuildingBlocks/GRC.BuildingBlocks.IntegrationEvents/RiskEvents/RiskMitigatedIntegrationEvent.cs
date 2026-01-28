using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.RiskEvents;

public class RiskMitigatedIntegrationEvent : IntegrationEvent
{
    public Guid RiskId { get; set; }
    public string RiskCode { get; set; }
    public string MitigationStrategy { get; set; }
    public List<string> ControlsApplied { get; set; }
    public string NewRiskLevel { get; set; }
    public decimal RiskReductionPercentage { get; set; }
    public DateTime MitigationDate { get; set; }
    public bool RequiresContinuousMonitoring { get; set; }

    public RiskMitigatedIntegrationEvent()
    {
        ControlsApplied = new List<string>();
    }

    public RiskMitigatedIntegrationEvent(
        Guid riskId,
        string riskCode,
        string mitigationStrategy,
        List<string> controlsApplied,
        string newRiskLevel,
        decimal riskReductionPercentage,
        bool requiresContinuousMonitoring)
    {
        RiskId = riskId;
        RiskCode = riskCode;
        MitigationStrategy = mitigationStrategy;
        ControlsApplied = controlsApplied ?? new List<string>();
        NewRiskLevel = newRiskLevel;
        RiskReductionPercentage = riskReductionPercentage;
        MitigationDate = DateTime.UtcNow;
        RequiresContinuousMonitoring = requiresContinuousMonitoring;
    }
}