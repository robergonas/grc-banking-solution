using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Risk.Domain.DomainEvents;

public record RiskAssessedDomainEvent : IDomainEvent
{
    public Guid RiskId { get; }
    public Guid AssessmentId { get; }
    public int Probability { get; }
    public int Impact { get; }
    public int InherentRiskLevel { get; }
    public int? ResidualRiskLevel { get; }
    public Guid AssessedBy { get; }
    public DateTime OccurredOn { get; }

    public RiskAssessedDomainEvent(
        Guid riskId,
        Guid assessmentId,
        int probability,
        int impact,
        int inherentRiskLevel,
        int? residualRiskLevel,
        Guid assessedBy)
    {
        RiskId = riskId;
        AssessmentId = assessmentId;
        Probability = probability;
        Impact = impact;
        InherentRiskLevel = inherentRiskLevel;
        ResidualRiskLevel = residualRiskLevel;
        AssessedBy = assessedBy;
        OccurredOn = DateTime.UtcNow;
    }
}