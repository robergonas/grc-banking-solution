using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Aggregates.RiskAggregate;

namespace GRC.Risk.Domain.DomainEvents;

public record RiskIdentifiedDomainEvent : IDomainEvent
{
    public Guid RiskId { get; }
    public string Code { get; }
    public string Title { get; }
    public int CategoryId { get; }
    public Guid IdentifiedBy { get; }
    public DateTime OccurredOn { get; }

    public RiskIdentifiedDomainEvent(
        Guid riskId,
        string code,
        string title,
        int categoryId,
        Guid identifiedBy)
    {
        RiskId = riskId;
        Code = code;
        Title = title;
        CategoryId = categoryId;
        IdentifiedBy = identifiedBy;
        OccurredOn = DateTime.UtcNow;
    }
}