using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.Aggregates.ControlAggregate;

public interface IControlRepository : IRepository<Control>
{
    Task<Control?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Control?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<IEnumerable<Control>> GetByOwnerAsync(
        Guid ownerId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Control>> GetByTypeAsync(
        int typeId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Control>> GetByStatusAsync(
        int statusId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Control>> GetControlsDueForTestingAsync(
        DateTime dueDate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Control>> GetLowEffectivenessControlsAsync(
        int threshold,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    Control Add(Control control);

    void Update(Control control);

    void Delete(Control control);
}