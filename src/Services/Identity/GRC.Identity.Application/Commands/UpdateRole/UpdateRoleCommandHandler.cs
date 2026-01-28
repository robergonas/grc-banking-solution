using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.UpdateRole;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UpdateRoleCommandHandler> _logger;

    public UpdateRoleCommandHandler(IRoleRepository roleRepository, ILogger<UpdateRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating role: {RoleId}", request.RoleId);

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new InvalidOperationException($"Rol con ID {request.RoleId} no encontrado");
        }

        // Verificar si el nuevo nombre ya existe en otro rol
        if (await _roleRepository.ExistsByNameAsync(request.Name, request.RoleId))
        {
            throw new InvalidOperationException($"Ya existe otro rol con el nombre '{request.Name}'");
        }

        role.Update(request.Name, request.Description);

        await _roleRepository.UpdateAsync(role);
        await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Role updated successfully: {RoleId}", request.RoleId);

        return Unit.Value;
    }
}