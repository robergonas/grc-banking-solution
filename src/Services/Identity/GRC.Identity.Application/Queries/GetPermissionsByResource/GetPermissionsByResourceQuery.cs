using GRC.Identity.Application.DTOs;
using MediatR;

namespace GRC.Identity.Application.Queries.GetPermissionsByResource;

public class GetPermissionsByResourceQuery : IRequest<IEnumerable<PermissionDto>>
{
    public string Resource { get; set; } = string.Empty;
}