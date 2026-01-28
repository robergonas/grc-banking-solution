using MediatR;

namespace GRC.Identity.Application.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<Unit>
{
    public Guid RoleId { get; set; }
}