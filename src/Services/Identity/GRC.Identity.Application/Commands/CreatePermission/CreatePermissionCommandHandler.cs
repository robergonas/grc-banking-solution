using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.CreatePermission;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Guid>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<CreatePermissionCommandHandler> _logger;

    public CreatePermissionCommandHandler(IRoleRepository roleRepository, ILogger<CreatePermissionCommandHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Guid> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating permission: {Resource}.{Action}", request.Resource, request.Action);

        // Verificar si ya existe
        if (await _roleRepository.PermissionExistsAsync(request.Resource, request.Action))
        {
            throw new InvalidOperationException($"Ya existe el permiso {request.Resource}.{request.Action}");
        }

        var permission = Permission.Create(request.Resource, request.Action);

        // Nota: Los permisos se crean sin rol asignado inicialmente
        // Se asignarán a roles usando AssignPermissionsToRole

        _logger.LogInformation("Permission created: {PermissionId} - {PermissionName}", permission.Id, permission.Name);

        return permission.Id;
    }
}