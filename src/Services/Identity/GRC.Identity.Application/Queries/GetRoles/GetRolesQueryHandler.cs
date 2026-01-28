using AutoMapper;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, PagedResult<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRolesQueryHandler> _logger;

    public GetRolesQueryHandler(
        IRoleRepository roleRepository,
        IMapper mapper,
        ILogger<GetRolesQueryHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PagedResult<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting roles - Page: {PageNumber}, Size: {PageSize}",
            request.PageNumber, request.PageSize);

        var roles = await _roleRepository.GetAllAsync();//request.PageNumber, request.PageSize
        var totalCount = await _roleRepository.GetTotalCountAsync();

        // Filtrar según parámetros
        var filteredRoles = roles.AsEnumerable();

        if (request.IsActive.HasValue)
        {
            filteredRoles = filteredRoles.Where(r => r.IsActive == request.IsActive.Value);
        }

        if (!request.IncludeSystemRoles)
        {
            filteredRoles = filteredRoles.Where(r => !r.IsSystemRole);
        }

        var roleDtos = filteredRoles.Select(role =>
        {
            var dto = _mapper.Map<RoleDto>(role);
            dto.PermissionCount = role.Permissions.Count;
            return dto;
        }).ToList();

        _logger.LogInformation("Retrieved {Count} roles", roleDtos.Count);

        return new PagedResult<RoleDto>
        {
            Items = roleDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}