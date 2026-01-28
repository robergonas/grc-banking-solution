using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using GRC.Governance.Domain.Aggregates.PolicyAggregate;

namespace GRC.Governance.Domain.Aggregates.PolicyAggregate;

public interface IPolicyRepository : IRepository<Policy>
{
    Task<Policy> GetByIdAsync(Guid id);
    Task<Policy> GetByCodeAsync(string code);
    Task<IEnumerable<Policy>> GetAllAsync();
    Task<IEnumerable<Policy>> GetByTypeAsync(PolicyType type);
    Task<IEnumerable<Policy>> GetByStatusAsync(PolicyStatus status);
    Task<IEnumerable<Policy>> GetByOwnerAsync(Guid ownerId);
    Task<IEnumerable<Policy>> GetPoliciesForReviewAsync();
    Task<IEnumerable<Policy>> GetExpiringSoonAsync(int daysThreshold = 30);
    Task<bool> CodeExistsAsync(string code);

    Policy Add(Policy policy);
    void Update(Policy policy);
    void Remove(Policy policy);
}

