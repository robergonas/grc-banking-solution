using Microsoft.EntityFrameworkCore;
using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.BuildingBlocks.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}