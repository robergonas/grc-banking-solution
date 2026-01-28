using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.RiskEvents;

public class RiskLevelEscalatedIntegrationEvent : IntegrationEvent
{
    public Guid RiskId { get; set; }
    public string RiskCode { get; set; }
    public string RiskName { get; set; }
    public string PreviousLevel { get; set; }
    public string CurrentLevel { get; set; }
    public string EscalationReason { get; set; }
    public List<string> NotifyUsers { get; set; }
    public DateTime EscalationDate { get; set; }
    public bool RequiresExecutiveReview { get; set; }

    public RiskLevelEscalatedIntegrationEvent()
    {
        NotifyUsers = new List<string>();
    }

    public RiskLevelEscalatedIntegrationEvent(
        Guid riskId,
        string riskCode,
        string riskName,
        string previousLevel,
        string currentLevel,
        string escalationReason,
        List<string> notifyUsers,
        bool requiresExecutiveReview)
    {
        RiskId = riskId;
        RiskCode = riskCode;
        RiskName = riskName;
        PreviousLevel = previousLevel;
        CurrentLevel = currentLevel;
        EscalationReason = escalationReason;
        NotifyUsers = notifyUsers ?? new List<string>();
        EscalationDate = DateTime.UtcNow;
        RequiresExecutiveReview = requiresExecutiveReview;
    }
}