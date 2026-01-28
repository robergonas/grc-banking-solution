using GRC.Risk.Application.DTOs;
using MediatR;

namespace GRC.Risk.Application.Queries.GetRisks;

public record GetRisksQuery : IRequest<PagedResult<RiskDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int? CategoryId { get; init; }
    public int? StatusId { get; init; }
    public Guid? OwnerId { get; init; }
    public string? SearchTerm { get; init; }
}