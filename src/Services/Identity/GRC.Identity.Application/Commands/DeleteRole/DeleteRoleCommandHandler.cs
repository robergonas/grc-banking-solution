using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.DeleteRole;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<DeleteRoleCommandHandler> _logger;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository, ILogger<DeleteRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting role: {RoleId}", request.RoleId);

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new InvalidOperationException($"Rol con ID {request.RoleId} no encontrado");
        }

        if (role.IsSystemRole)
        {
            throw new InvalidOperationException("No se puede eliminar un rol del sistema");
        }

        await _roleRepository.DeleteAsync(role);
        await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Role deleted successfully: {RoleId}", request.RoleId);

        return Unit.Value;
    }
}