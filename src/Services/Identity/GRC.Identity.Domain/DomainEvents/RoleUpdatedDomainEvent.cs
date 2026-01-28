using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Identity.Domain.DomainEvents;

public class RoleUpdatedDomainEvent : IDomainEvent
{
    public Guid RoleId { get; }
    public string RoleName { get; }
    public DateTime OccurredOn { get; }

    public RoleUpdatedDomainEvent(Guid roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
        OccurredOn = DateTime.UtcNow;
    }
}