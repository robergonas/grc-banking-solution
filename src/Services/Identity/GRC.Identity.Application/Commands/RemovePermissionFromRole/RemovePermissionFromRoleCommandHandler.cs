using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.RemovePermissionFromRole;

public class RemovePermissionFromRoleCommandHandler : IRequestHandler<RemovePermissionFromRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<RemovePermissionFromRoleCommandHandler> _logger;

    public RemovePermissionFromRoleCommandHandler(IRoleRepository roleRepository, ILogger<RemovePermissionFromRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(RemovePermissionFromRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing permission {PermissionId} from role {RoleId}", request.PermissionId, request.RoleId);

        var role = await _roleRepository.GetByIdWithPermissionsAsync(request.RoleId);
        if (role == null)
        {
            throw new InvalidOperationException($"Rol con ID {request.RoleId} no encontrado");
        }

        role.RemovePermission(request.PermissionId);

        await _roleRepository.UpdateAsync(role);
        await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Permission removed successfully from role: {RoleId}", request.RoleId);

        return Unit.Value;
    }
}