using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRC.Identity.Domain.Aggregates.RoleAggregate;
public interface IRoleRepository : IRepository<Role>
{
    Task<Role> GetByIdAsync(Guid id);
    Task<Role?> GetByIdWithPermissionsAsync(Guid id);
    Task<Role> GetByNameAsync(string name);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<IEnumerable<Role>> GetActiveRolesAsync();
    Task<int> GetTotalCountAsync();
    Task<bool> ExistsByNameAsync(string name, Guid? excludeRoleId = null);
    Task<Role> AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(Role role);
    Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);
    Task<Permission?> GetPermissionByIdAsync(Guid permissionId);
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();
    Task<IEnumerable<Permission>> GetPermissionsByResourceAsync(string resource);
    Task<Permission?> GetPermissionByNameAsync(string name);
    Task<bool> PermissionExistsAsync(string resource, string action, Guid? excludePermissionId = null);
}