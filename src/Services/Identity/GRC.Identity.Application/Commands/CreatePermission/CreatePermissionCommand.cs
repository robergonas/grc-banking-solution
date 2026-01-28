using MediatR;

namespace GRC.Identity.Application.Commands.CreatePermission;

public class CreatePermissionCommand : IRequest<Guid>
{
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
}