using System;
using System.Threading.Tasks;

namespace GRC.BuildingBlocks.Infrastructure.Identity;

public interface IIdentityService
{
    string GetUserIdentity();
    string GetUserName();
    Task IsInRoleAsync(string userId, string role);
    Task AuthorizeAsync(string userId, string policyName);
}