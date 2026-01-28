using MediatR;

namespace GRC.Identity.Application.Commands.DeletePermission;

public class DeletePermissionCommand : IRequest<Unit>
{
    public Guid PermissionId { get; set; }
}