using AutoMapper;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleWithPermissionsDto?>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;

    public GetRoleByIdQueryHandler(
        IRoleRepository roleRepository,
        IMapper mapper,
        ILogger<GetRoleByIdQueryHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<RoleWithPermissionsDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting role by ID: {RoleId}", request.RoleId);

        var role = await _roleRepository.GetByIdWithPermissionsAsync(request.RoleId);

        if (role == null)
        {
            _logger.LogWarning("Role not found: {RoleId}", request.RoleId);
            return null;
        }

        var roleDto = _mapper.Map<RoleWithPermissionsDto>(role);

        _logger.LogInformation("Role found: {RoleName} with {PermissionCount} permissions",
            role.Name, role.Permissions.Count);

        return roleDto;
    }
}