using GRC.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.IntegrationEvents.IdentityEvents;

public class UserRegisteredIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public List<string> Roles { get; set; }
    public DateTime RegistrationDate { get; set; }

    public UserRegisteredIntegrationEvent()
    {
        Roles = new List<string>();
    }

    public UserRegisteredIntegrationEvent(
        Guid userId,
        string email,
        string fullName,
        List<string> roles)
    {
        UserId = userId;
        Email = email;
        FullName = fullName;
        Roles = roles ?? new List<string>();
        RegistrationDate = DateTime.UtcNow;
    }
}