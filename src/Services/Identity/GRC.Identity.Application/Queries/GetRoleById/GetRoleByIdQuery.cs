using GRC.Identity.Application.DTOs;
using MediatR;

namespace GRC.Identity.Application.Queries.GetRoleById;

public class GetRoleByIdQuery : IRequest<RoleWithPermissionsDto?>
{
    public Guid RoleId { get; set; }
}