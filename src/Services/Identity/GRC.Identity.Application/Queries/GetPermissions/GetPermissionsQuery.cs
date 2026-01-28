using GRC.Identity.Application.DTOs;
using MediatR;

namespace GRC.Identity.Application.Queries.GetPermissions;
public class GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
{
}