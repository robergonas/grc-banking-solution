using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Identity.Domain.DomainEvents;

public class PermissionRemovedDomainEvent : IDomainEvent
{
    public Guid RoleId { get; }
    public Guid PermissionId { get; }
    public DateTime OccurredOn { get; }

    public PermissionRemovedDomainEvent(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        OccurredOn = DateTime.UtcNow;
    }
}