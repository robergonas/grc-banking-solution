using MediatR;

namespace GRC.Identity.Application.Commands.UpdateRole;

public class UpdateRoleCommand : IRequest<Unit>
{
    public Guid RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}