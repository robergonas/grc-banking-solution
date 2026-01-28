using GRC.Identity.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace GRC.Identity.Application.Queries.GetUsersByRole;

public class GetUsersByRoleQuery : IRequest<List<UserDto>>
{
    public Guid RoleId { get; set; }

    public GetUsersByRoleQuery(Guid roleId)
    {
        RoleId = roleId;
    }
}