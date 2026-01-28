using GRC.Risk.Application.DTOs;
using MediatR;

namespace GRC.Risk.Application.Queries.GetControls;

public record GetControlsQuery : IRequest<PagedResult<ControlDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int? TypeId { get; init; }
    public int? StatusId { get; init; }
    public Guid? OwnerId { get; init; }
    public string? SearchTerm { get; init; }
}