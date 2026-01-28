using MediatR;

namespace GRC.Identity.Application.Commands.UpdatePermission;

public class UpdatePermissionCommand : IRequest<Unit>
{
    public Guid PermissionId { get; set; }
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
}