using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using GRC.Identity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GRC.Identity.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly IdentityContext _context;
    public UserRepository(IdentityContext context)
    {
        _context = context;
    }
    public IUnitOfWork UnitOfWork => _context;
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Email.Value.ToLower() == email.ToLower());
    }
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
             .Include(u => u.UserRoles)
                 .ThenInclude(ur => ur.Role)
             .OrderBy(u => u.FullName)
             .ToListAsync();
    }
    public async Task<IEnumerable<User>> GetByRoleAsync(Guid roleId)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }
    public async Task<(IEnumerable<User> Users, int TotalCount)> GetPagedAsync(
    int pageNumber,int pageSize,string? searchTerm = null,string? status = null)
    {
        var query = _context.Users
             .Include(u => u.UserRoles)
                 .ThenInclude(ur => ur.Role)
             .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLower = searchTerm.ToLower();
            query = query.Where(u =>
                u.Email.Value.ToLower().Contains(searchTermLower) ||
                u.FullName.ToLower().Contains(searchTermLower));
        }

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(status) && int.TryParse(status, out var statusValue))
        {
            query = query.Where(u => u.Status.Id == statusValue);
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return await _context.Users
            .AnyAsync(u => u.Email.Value.ToLower() == email.ToLower());
    }
    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }
    public async Task<User> AddAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        await _context.Users.AddAsync(user);
        return user;
    }
    public Task UpdateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        _context.Entry(user).State = EntityState.Modified;
        return Task.CompletedTask;
    }
    public Task DeleteAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        _context.Users.Remove(user);
        return Task.CompletedTask;
    }
    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }
}