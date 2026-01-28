using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.ComplianceEvents;
public class RegulatoryReportGeneratedIntegrationEvent : IntegrationEvent
{
    public Guid ReportId { get; set; }
    public string ReportType { get; set; }
    public string RegulatoryBody { get; set; }
    public DateTime ReportingPeriodStart { get; set; }
    public DateTime ReportingPeriodEnd { get; set; }
    public DateTime GenerationDate { get; set; }
    public string ReportStatus { get; set; }
    public List<Guid> IncludedRisks { get; set; }
    public Guid GeneratedBy { get; set; }
    public RegulatoryReportGeneratedIntegrationEvent()
    {
        IncludedRisks = new List<Guid>();
    }
    public RegulatoryReportGeneratedIntegrationEvent(
        Guid reportId,
        string reportType,
        string regulatoryBody,
        DateTime reportingPeriodStart,
        DateTime reportingPeriodEnd,
        string reportStatus,
        List<Guid> includedRisks,
        Guid generatedBy)
    {
        ReportId = reportId;
        ReportType = reportType;
        RegulatoryBody = regulatoryBody;
        ReportingPeriodStart = reportingPeriodStart;
        ReportingPeriodEnd = reportingPeriodEnd;
        GenerationDate = DateTime.UtcNow;
        ReportStatus = reportStatus;
        IncludedRisks = includedRisks != null ? includedRisks : new List<Guid>();
        GeneratedBy = generatedBy;
    }
}