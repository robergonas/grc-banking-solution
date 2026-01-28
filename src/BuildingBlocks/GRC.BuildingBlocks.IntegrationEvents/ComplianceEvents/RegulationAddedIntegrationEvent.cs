using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.ComplianceEvents;

public class RegulationAddedIntegrationEvent : IntegrationEvent
{
    public Guid RegulationId { get; set; }
    public string RegulationCode { get; set; }
    public string RegulationName { get; set; }
    public string RegulatoryBody { get; set; }
    public DateTime EffectiveDate { get; set; }
    public List<string> ApplicableCategories { get; set; }
    public int RequiredControls { get; set; }
    public string Severity { get; set; }
    public RegulationAddedIntegrationEvent()
    {
        ApplicableCategories = new List<string>();
    }
    public RegulationAddedIntegrationEvent(
        Guid regulationId,
        string regulationCode,
        string regulationName,
        string regulatoryBody,
        DateTime effectiveDate,
        List<string> applicableCategories,
        int requiredControls,
        string severity)
    {
        RegulationId = regulationId;
        RegulationCode = regulationCode;
        RegulationName = regulationName;
        RegulatoryBody = regulatoryBody;
        EffectiveDate = effectiveDate;
        ApplicableCategories = applicableCategories ?? new List<string>();
        RequiredControls = requiredControls;
        Severity = severity;
    }
}