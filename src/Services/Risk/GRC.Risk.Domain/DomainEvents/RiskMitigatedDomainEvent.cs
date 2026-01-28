using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.DomainEvents;

public record RiskMitigatedDomainEvent : IDomainEvent
{
    public Guid RiskId { get; }
    public Guid MitigationPlanId { get; }
    public int StrategyId { get; }
    public DateTime OccurredOn { get; }

    public RiskMitigatedDomainEvent(
        Guid riskId,
        Guid mitigationPlanId,
        int strategyId)
    {
        RiskId = riskId;
        MitigationPlanId = mitigationPlanId;
        StrategyId = strategyId;
        OccurredOn = DateTime.UtcNow;
    }
}