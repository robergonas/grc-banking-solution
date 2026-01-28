using GRC.Identity.Application.DTOs;
using MediatR;

namespace GRC.Identity.Application.Queries.GetPermissionById;

public class GetPermissionByIdQuery : IRequest<PermissionDto?>
{
    public Guid PermissionId { get; set; }
}