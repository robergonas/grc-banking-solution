using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.IdentityEvents;

public class UserRolesChangedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public List<string> OldRoles { get; set; }
    public List<string> NewRoles { get; set; }
    public string ChangedBy { get; set; }
    public DateTime ChangeDate { get; set; }

    public UserRolesChangedIntegrationEvent()
    {
        OldRoles = new List<string>();
        NewRoles = new List<string>();
    }

    public UserRolesChangedIntegrationEvent(
        Guid userId,
        string userEmail,
        List<string> oldRoles,
        List<string> newRoles,
        string changedBy)
    {
        UserId = userId;
        UserEmail = userEmail;
        OldRoles = oldRoles ?? new List<string>();
        NewRoles = newRoles ?? new List<string>();
        ChangedBy = changedBy;
        ChangeDate = DateTime.UtcNow;
    }
}