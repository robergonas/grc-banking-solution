using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Identity.Domain.DomainEvents;

public class PermissionAssignedDomainEvent : IDomainEvent
{
    public Guid RoleId { get; }
    public Guid PermissionId { get; }
    public DateTime OccurredOn { get; }

    public PermissionAssignedDomainEvent(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        OccurredOn = DateTime.UtcNow;
    }
}