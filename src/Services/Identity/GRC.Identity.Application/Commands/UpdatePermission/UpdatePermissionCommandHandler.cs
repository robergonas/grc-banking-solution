using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.UpdatePermission;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UpdatePermissionCommandHandler> _logger;

    public UpdatePermissionCommandHandler(IRoleRepository roleRepository, ILogger<UpdatePermissionCommandHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating permission: {PermissionId}", request.PermissionId);

        var permission = await _roleRepository.GetPermissionByIdAsync(request.PermissionId);
        if (permission == null)
        {
            throw new InvalidOperationException($"Permiso con ID {request.PermissionId} no encontrado");
        }

        if (await _roleRepository.PermissionExistsAsync(request.Resource, request.Action, request.PermissionId))
        {
            throw new InvalidOperationException($"Ya existe el permiso {request.Resource}.{request.Action}");
        }

        permission.Update(request.Resource, request.Action);

        _logger.LogInformation("Permission updated: {PermissionId}", request.PermissionId);

        return Unit.Value;
    }
}