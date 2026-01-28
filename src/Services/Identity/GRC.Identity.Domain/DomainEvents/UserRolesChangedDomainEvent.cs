using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Identity.Domain.DomainEvents;

public class UserRolesChangedDomainEvent : IDomainEvent
{
    public Guid UserId { get; }
    public List<Guid> OldRoleIds { get; }
    public DateTime OccurredOn { get; }

    public UserRolesChangedDomainEvent(Guid userId, List<Guid> oldRoleIds)
    {
        UserId = userId;
        OldRoleIds = oldRoleIds;
        OccurredOn = DateTime.UtcNow;
    }
}