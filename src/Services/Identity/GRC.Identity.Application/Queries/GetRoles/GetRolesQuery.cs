using GRC.Identity.Application.DTOs;
using MediatR;

namespace GRC.Identity.Application.Queries.GetRoles;

public class GetRolesQuery : IRequest<PagedResult<RoleDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool? IsActive { get; set; }
    public bool IncludeSystemRoles { get; set; } = true;
}