using GRC.BuildingBlocks.EventBus.Events;
using System;

namespace GRC.BuildingBlocks.IntegrationEvents.RiskEvents;

public class ControlImplementedIntegrationEvent : IntegrationEvent
{
    public Guid ControlId { get; set; }
    public Guid RiskId { get; set; }
    public string ControlType { get; set; }
    public string ControlDescription { get; set; }
    public DateTime ImplementationDate { get; set; }
    public Guid ImplementedBy { get; set; }
    public decimal ExpectedRiskReduction { get; set; }

    public ControlImplementedIntegrationEvent()
    {
    }

    public ControlImplementedIntegrationEvent(
        Guid controlId,
        Guid riskId,
        string controlType,
        string controlDescription,
        Guid implementedBy,
        decimal expectedRiskReduction)
    {
        ControlId = controlId;
        RiskId = riskId;
        ControlType = controlType;
        ControlDescription = controlDescription;
        ImplementedBy = implementedBy;
        ImplementationDate = DateTime.UtcNow;
        ExpectedRiskReduction = expectedRiskReduction;
    }
}