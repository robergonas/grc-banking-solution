using MediatR;

namespace GRC.Risk.Application.Commands.AssessRisk;

public record AssessRiskCommand : IRequest<Guid>
{
    public Guid RiskId { get; init; }
    public int Probability { get; init; }
    public string? ProbabilityJustification { get; init; }
    public int Impact { get; init; }
    public string? ImpactJustification { get; init; }
    public string? ExistingControls { get; init; }
    public int? ControlEffectiveness { get; init; }
    public int? ResidualRiskLevel { get; init; }
    public string? ResidualRiskJustification { get; init; }
    public int? RiskVelocity { get; init; }
    public int? RiskAppetite { get; init; }
    public string? Notes { get; init; }
    public DateTime? NextReviewDate { get; init; }
    public Guid AssessedBy { get; init; }
}