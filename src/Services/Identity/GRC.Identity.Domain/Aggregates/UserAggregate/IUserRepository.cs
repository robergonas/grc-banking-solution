using GRC.BuildingBlocks.Domain.SeedWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRC.Identity.Domain.Aggregates.UserAggregate;

public interface IUserRepository : IRepository<User>
{
    IUnitOfWork UnitOfWork { get; }
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetByRoleAsync(Guid roleId);
    Task<(IEnumerable<User> Users, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? status = null);    
    Task<bool> ExistsByEmailAsync(string email);
    Task<User>AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}