using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Identity.Application.Queries.GetUsersByRole;

public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersByRoleQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetByRoleAsync(request.RoleId);

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email.Value,
            FullName = u.FullName,
            Status = u.Status.Name,
            LastLoginDate = u.LastLoginDate,
            CreatedAt = u.CreatedAt,
            Roles = u.UserRoles
            .Where(ur => ur.Role != null)
            .Select(ur => new RoleDto
            {
                Id = ur.Role.Id,
                Name = ur.Role.Name,
                Description = ur.Role.Description
            }).ToList()
        });
    }
}