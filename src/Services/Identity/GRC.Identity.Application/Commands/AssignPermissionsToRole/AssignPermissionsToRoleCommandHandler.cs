using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.AssignPermissionsToRole;

public class AssignPermissionsToRoleCommandHandler : IRequestHandler<AssignPermissionsToRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<AssignPermissionsToRoleCommandHandler> _logger;

    public AssignPermissionsToRoleCommandHandler(IRoleRepository roleRepository, ILogger<AssignPermissionsToRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Assigning {Count} permissions to role: {RoleId}", request.PermissionIds.Count, request.RoleId);

        var role = await _roleRepository.GetByIdWithPermissionsAsync(request.RoleId);
        if (role == null)
        {
            throw new InvalidOperationException($"Rol con ID {request.RoleId} no encontrado");
        }

        // Limpiar permisos actuales
        role.ClearPermissions();

        // Agregar nuevos permisos
        foreach (var permissionId in request.PermissionIds)
        {
            var permission = await _roleRepository.GetPermissionByIdAsync(permissionId);
            if (permission == null)
            {
                _logger.LogWarning("Permission not found: {PermissionId}", permissionId);
                continue;
            }

            role.AddPermission(permission);
        }

        await _roleRepository.UpdateAsync(role);
        await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Permissions assigned successfully to role: {RoleId}", request.RoleId);

        return Unit.Value;
    }
}