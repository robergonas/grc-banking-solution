using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Identity.Domain.DomainEvents;

public class RoleCreatedDomainEvent : IDomainEvent
{
    public Guid RoleId { get; }
    public string RoleName { get; }
    public DateTime OccurredOn { get; }

    public RoleCreatedDomainEvent(Guid roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
        OccurredOn = DateTime.UtcNow;
    }
}