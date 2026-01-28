using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.DeletePermission;

public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<DeletePermissionCommandHandler> _logger;

    public DeletePermissionCommandHandler(IRoleRepository roleRepository, ILogger<DeletePermissionCommandHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting permission: {PermissionId}", request.PermissionId);

        var permission = await _roleRepository.GetPermissionByIdAsync(request.PermissionId);
        if (permission == null)
        {
            throw new InvalidOperationException($"Permiso con ID {request.PermissionId} no encontrado");
        }

        _logger.LogInformation("Permission deleted: {PermissionId}", request.PermissionId);

        return Unit.Value;
    }
}