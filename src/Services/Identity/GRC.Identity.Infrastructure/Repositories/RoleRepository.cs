using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.Aggregates.RoleAggregate;
using GRC.Identity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GRC.Identity.Infrastructure.Repositories;
public class RoleRepository : IRoleRepository
{
    private readonly IdentityContext _context;
    public IUnitOfWork UnitOfWork => (IUnitOfWork)_context;
    public RoleRepository(IdentityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<Role> GetByIdAsync(Guid id)
    {
        return await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    public async Task<Role?> GetByIdWithPermissionsAsync(Guid id)
    {
        return await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    public async Task<Role> GetByNameAsync(string name)
    {
        return await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Name == name);
    }
    public async Task<IEnumerable<Role>> GetAllAsync()//int pageNumber, int pageSize
    {
        return await _context.Roles
            .Include(r => r.Permissions)
            .OrderBy(r => r.Name)
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            .ToListAsync();
    }
    public async Task<IEnumerable<Role>> GetActiveRolesAsync()
    {
        return await _context.Roles
            .Include(r => r.Permissions)
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }
    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Roles.CountAsync();
    }
    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeRoleId = null)
    {
        var query = _context.Roles.Where(r => r.Name.ToLower() == name.ToLower());

        if (excludeRoleId.HasValue)
        {
            query = query.Where(r => r.Id != excludeRoleId.Value);
        }

        return await query.AnyAsync();
    }
    public Role Add(Role role)
    {
        return _context.Roles.Add(role).Entity;
    }
    public void Update(Role role)
    {
        _context.Entry(role).State = EntityState.Modified;
    }
    public void Remove(Role role)
    {
        _context.Roles.Remove(role);
    }
    public async Task<Role> AddAsync(Role role)
    {
        var result = await _context.Roles.AddAsync(role);
        return result.Entity;
    }
    public Task UpdateAsync(Role role)
    {
        _context.Entry(role).State = EntityState.Modified;
        _context.Roles.Update(role);
        return Task.CompletedTask;
    }
    public Task DeleteAsync(Role role)
    {
        _context.Roles.Remove(role);
        return Task.CompletedTask;
    }
    public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
    {
        return await _context.Permissions
            .Where(p => p.RoleId == roleId)
            .OrderBy(p => p.Resource)
            .ThenBy(p => p.Action)
            .ToListAsync();
    }
    public async Task<Permission?> GetPermissionByIdAsync(Guid permissionId)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(p => p.Id == permissionId);
    }
    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return await _context.Permissions
            .OrderBy(p => p.Resource)
            .ThenBy(p => p.Action)
            .ToListAsync();
    }
    public async Task<IEnumerable<Permission>> GetPermissionsByResourceAsync(string resource)
    {
        return await _context.Permissions
            .Where(p => p.Resource.ToLower() == resource.ToLower())
            .OrderBy(p => p.Action)
            .ToListAsync();
    }
    public async Task<Permission?> GetPermissionByNameAsync(string name)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
    }
    public async Task<bool> PermissionExistsAsync(string resource, string action, Guid? excludePermissionId = null)
    {
        var query = _context.Permissions
            .Where(p => p.Resource.ToLower() == resource.ToLower()
                     && p.Action.ToLower() == action.ToLower());

        if (excludePermissionId.HasValue)
        {
            query = query.Where(p => p.Id != excludePermissionId.Value);
        }

        return await query.AnyAsync();
    }
}