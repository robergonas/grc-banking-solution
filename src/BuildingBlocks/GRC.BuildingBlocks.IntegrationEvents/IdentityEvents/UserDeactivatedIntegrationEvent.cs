using GRC.BuildingBlocks.EventBus.Events;
using System;

namespace GRC.BuildingBlocks.IntegrationEvents.IdentityEvents;

public class UserDeactivatedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public string Reason { get; set; }
    public DateTime DeactivationDate { get; set; }
    public string DeactivatedBy { get; set; }

    public UserDeactivatedIntegrationEvent()
    {
    }

    public UserDeactivatedIntegrationEvent(
        Guid userId,
        string userEmail,
        string reason,
        string deactivatedBy)
    {
        UserId = userId;
        UserEmail = userEmail;
        Reason = reason;
        DeactivatedBy = deactivatedBy;
        DeactivationDate = DateTime.UtcNow;
    }
}