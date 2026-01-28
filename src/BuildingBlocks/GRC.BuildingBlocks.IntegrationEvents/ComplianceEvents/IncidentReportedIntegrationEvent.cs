using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.ComplianceEvents;

public class IncidentReportedIntegrationEvent : IntegrationEvent
{
    public Guid IncidentId { get; set; }
    public string IncidentCode { get; set; }
    public string IncidentType { get; set; }
    public string Severity { get; set; }
    public string Description { get; set; }
    public DateTime OccurrenceDate { get; set; }
    public Guid ReportedBy { get; set; }
    public List<string> AffectedRegulations { get; set; }
    public decimal EstimatedImpact { get; set; }
    public bool RequiresRegulatoryNotification { get; set; }

    public IncidentReportedIntegrationEvent()
    {
        AffectedRegulations = new List<string>();
    }

    public IncidentReportedIntegrationEvent(
        Guid incidentId,
        string incidentCode,
        string incidentType,
        string severity,
        string description,
        DateTime occurrenceDate,
        Guid reportedBy,
        List<string> affectedRegulations,
        decimal estimatedImpact,
        bool requiresRegulatoryNotification)
    {
        IncidentId = incidentId;
        IncidentCode = incidentCode;
        IncidentType = incidentType;
        Severity = severity;
        Description = description;
        OccurrenceDate = occurrenceDate;
        ReportedBy = reportedBy;
        AffectedRegulations = affectedRegulations ?? new List<string>();
        EstimatedImpact = estimatedImpact;
        RequiresRegulatoryNotification = requiresRegulatoryNotification;
    }
}