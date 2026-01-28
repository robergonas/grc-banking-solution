using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<CreateRoleCommandHandler> _logger;

    public CreateRoleCommandHandler(
        IRoleRepository roleRepository,
        ILogger<CreateRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating role: {RoleName}", request.Name);

        // Verificar si ya existe un rol con ese nombre
        var existingRole = await _roleRepository.GetByNameAsync(request.Name);
        if (existingRole != null)
        {
            throw new InvalidOperationException($"Ya existe un rol con el nombre '{request.Name}'");
        }

        // Crear el rol
        var role = Role.Create(request.Name, request.Description, request.IsSystemRole);

        // Guardar
        await _roleRepository.AddAsync(role);
        await _roleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Role created successfully: {RoleId} - {RoleName}", role.Id, role.Name);

        return role.Id;
    }
}