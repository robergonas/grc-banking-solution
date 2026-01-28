using MediatR;

namespace GRC.Identity.Application.Commands.RemovePermissionFromRole;

public class RemovePermissionFromRoleCommand : IRequest<Unit>
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}