using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;

public interface IMitigationPlanRepository : IRepository<MitigationPlan>
{
    Task<MitigationPlan?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<MitigationPlan?> GetByIdWithActionsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<MitigationPlan>> GetByRiskIdAsync(
        Guid riskId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<MitigationPlan>> GetByOwnerAsync(
        Guid ownerId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<MitigationPlan>> GetByStatusAsync(
        int statusId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<MitigationPlan>> GetByStrategyAsync(
        int strategyId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<MitigationPlan>> GetOverduePlansAsync(
        CancellationToken cancellationToken = default);

    Task<IEnumerable<MitigationPlan>> GetHighPriorityPlansAsync(
        int priorityThreshold,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<MitigationPlan>> GetInProgressPlansAsync(
        CancellationToken cancellationToken = default);

    MitigationPlan Add(MitigationPlan mitigationPlan);

    void Update(MitigationPlan mitigationPlan);

    void Delete(MitigationPlan mitigationPlan);
}