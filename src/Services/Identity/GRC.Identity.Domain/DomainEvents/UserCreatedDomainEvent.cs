using GRC.BuildingBlocks.Domain.SeedWork;
using MediatR;
using System;

namespace GRC.Identity.Domain.DomainEvents;

public class UserCreatedDomainEvent : IDomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public string FullName { get; }
    public DateTime OccurredOn { get; }

    public UserCreatedDomainEvent(Guid userId, string email, string fullName)
    {
        UserId = userId;
        Email = email;
        FullName = fullName;
        OccurredOn = DateTime.UtcNow;
    }
}