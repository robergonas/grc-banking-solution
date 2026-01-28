using AutoMapper;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Queries.GetPermissionById;
public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto?>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPermissionByIdQueryHandler> _logger;
    public GetPermissionByIdQueryHandler(
        IRoleRepository roleRepository,
        IMapper mapper,
        ILogger<GetPermissionByIdQueryHandler> logger)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<PermissionDto?> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting permission by ID: {PermissionId}", request.PermissionId);

        var permission = await _roleRepository.GetPermissionByIdAsync(request.PermissionId);

        if (permission == null)
        {
            _logger.LogWarning("Permission not found: {PermissionId}", request.PermissionId);
            return null;
        }

        return _mapper.Map<PermissionDto>(permission);
    }
}