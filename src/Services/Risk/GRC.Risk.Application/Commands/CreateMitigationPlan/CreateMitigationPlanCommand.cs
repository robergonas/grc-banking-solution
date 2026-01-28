using MediatR;

namespace GRC.Risk.Application.Commands.CreateMitigationPlan;

public record CreateMitigationPlanCommand : IRequest<Guid>
{
    public Guid RiskId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int StrategyId { get; init; }
    public int Priority { get; init; }
    public Guid OwnerId { get; init; }
    public decimal? EstimatedCost { get; init; }
    public string? EstimatedBenefit { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? TargetCompletionDate { get; init; }
    public Guid CreatedBy { get; init; }
}