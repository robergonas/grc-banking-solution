using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.ComplianceEvents;

public class ComplianceCheckFailedIntegrationEvent : IntegrationEvent
{
    public Guid CheckId { get; set; }
    public Guid RegulationId { get; set; }
    public string RegulationCode { get; set; }
    public string FailureReason { get; set; }
    public string Severity { get; set; }
    public List<string> AffectedAreas { get; set; }
    public DateTime FailureDate { get; set; }
    public Guid DetectedBy { get; set; }
    public bool RequiresRegulatoryReporting { get; set; }
    public int DaysToRemediate { get; set; }

    public ComplianceCheckFailedIntegrationEvent()
    {
        AffectedAreas = new List<string>();
    }

    public ComplianceCheckFailedIntegrationEvent(
        Guid checkId,
        Guid regulationId,
        string regulationCode,
        string failureReason,
        string severity,
        List<string> affectedAreas,
        Guid detectedBy,
        bool requiresRegulatoryReporting,
        int daysToRemediate)
    {
        CheckId = checkId;
        RegulationId = regulationId;
        RegulationCode = regulationCode;
        FailureReason = failureReason;
        Severity = severity;
        AffectedAreas = affectedAreas ?? new List<string>();
        FailureDate = DateTime.UtcNow;
        DetectedBy = detectedBy;
        RequiresRegulatoryReporting = requiresRegulatoryReporting;
        DaysToRemediate = daysToRemediate;
    }
}