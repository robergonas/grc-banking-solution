using AutoMapper;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Queries.GetPermissionsByResource;
public class GetPermissionsByResourceQueryHandler : IRequestHandler<GetPermissionsByResourceQuery, IEnumerable<PermissionDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPermissionsByResourceQueryHandler> _logger;
    public GetPermissionsByResourceQueryHandler(
        IRoleRepository roleRepository,
        IMapper mapper,
        ILogger<GetPermissionsByResourceQueryHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByResourceQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting permissions for resource: {Resource}", request.Resource);

        var permissions = await _roleRepository.GetPermissionsByResourceAsync(request.Resource);
        var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);

        _logger.LogInformation("Retrieved {Count} permissions for resource {Resource}",
            permissionDtos.Count(), request.Resource);

        return permissionDtos;
    }
}