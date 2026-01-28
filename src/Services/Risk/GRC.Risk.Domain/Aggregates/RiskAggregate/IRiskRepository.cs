using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.RiskAggregate;

public interface IRiskRepository : IRepository<Risk>
{
    Task<Risk?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Risk?> GetByIdWithAssessmentsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Risk?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<IEnumerable<Risk>> GetByOwnerAsync(
        Guid ownerId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Risk>> GetByCategoryAsync(
        int categoryId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Risk>> GetByStatusAsync(
        int statusId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Risk>> GetHighRisksAsync(
        int inherentRiskThreshold,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Risk>> GetRisksForReviewAsync(
        DateTime reviewDate,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    Risk Add(Risk risk);

    void Update(Risk risk);

    void Delete(Risk risk);
}