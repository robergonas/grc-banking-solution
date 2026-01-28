using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.DomainEvents;

public record ControlImplementedDomainEvent : IDomainEvent
{
    public Guid ControlId { get; }
    public string Code { get; }
    public string Name { get; }
    public int TypeId { get; }
    public Guid OwnerId { get; }
    public DateTime OccurredOn { get; }

    public ControlImplementedDomainEvent(
        Guid controlId,
        string code,
        string name,
        int typeId,
        Guid ownerId)
    {
        ControlId = controlId;
        Code = code;
        Name = name;
        TypeId = typeId;
        OwnerId = ownerId;
        OccurredOn = DateTime.UtcNow;
    }
}