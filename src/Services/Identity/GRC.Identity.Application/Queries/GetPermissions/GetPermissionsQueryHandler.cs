using AutoMapper;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Queries.GetPermissions;

public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPermissionsQueryHandler> _logger;

    public GetPermissionsQueryHandler(
        IRoleRepository roleRepository,
        IMapper mapper,
        ILogger<GetPermissionsQueryHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all permissions");

        var permissions = await _roleRepository.GetAllPermissionsAsync();
        var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);

        _logger.LogInformation("Retrieved {Count} permissions", permissionDtos.Count());

        return permissionDtos;
    }
}