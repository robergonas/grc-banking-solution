using MediatR;

namespace GRC.Identity.Application.Commands.CreateRole;

public class CreateRoleCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSystemRole { get; set; } = false;
}