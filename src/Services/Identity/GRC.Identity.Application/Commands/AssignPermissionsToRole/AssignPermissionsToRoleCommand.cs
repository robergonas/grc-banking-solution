using MediatR;

namespace GRC.Identity.Application.Commands.AssignPermissionsToRole;

public class AssignPermissionsToRoleCommand : IRequest<Unit>
{
    public Guid RoleId { get; set; }
    public List<Guid> PermissionIds { get; set; } = new();
}