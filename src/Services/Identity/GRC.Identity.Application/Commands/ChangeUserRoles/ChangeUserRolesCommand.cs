using MediatR;
using System;
using System.Collections.Generic;

namespace GRC.Identity.Application.Commands.ChangeUserRoles;

public class ChangeUserRolesCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public List<string> Roles { get; set; }
    public IEnumerable<Guid> RolesIds { get; private set; }

    public ChangeUserRolesCommand()
    {
        Roles = new List<string>();
    }

    public ChangeUserRolesCommand(Guid userId, List<string> roles)
    {
        UserId = userId;
        Roles = roles != null ? roles : new List<string>();
    }
}