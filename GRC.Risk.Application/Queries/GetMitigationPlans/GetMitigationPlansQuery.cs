using GRC.Risk.Application.DTOs;
using MediatR;

namespace GRC.Risk.Application.Queries.GetMitigationPlans;

public record GetMitigationPlansQuery : IRequest<PagedResult<MitigationPlanDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid? RiskId { get; init; }
    public int? StatusId { get; init; }
    public Guid? OwnerId { get; init; }
    public bool? OnlyOverdue { get; init; }
}