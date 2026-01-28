using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Exceptions;

namespace GRC.Risk.Domain.Aggregates.RiskAggregate;

public class RiskAssessment : Entity
{
    public Guid RiskId { get; private set; }
    public DateTime AssessmentDate { get; private set; }
    public Guid AssessedBy { get; private set; }

    // Probability (1-5)
    public int Probability { get; private set; }
    public string? ProbabilityJustification { get; private set; }

    // Impact (1-5)
    public int Impact { get; private set; }
    public string? ImpactJustification { get; private set; }

    // Inherent Risk Level (Probability x Impact)
    public int InherentRiskLevel { get; private set; }

    // Existing Controls
    public string? ExistingControls { get; private set; }
    public int? ControlEffectiveness { get; private set; }

    // Residual Risk
    public int? ResidualRiskLevel { get; private set; }
    public string? ResidualRiskJustification { get; private set; }

    // Risk Velocity
    public int? RiskVelocity { get; private set; }

    // Risk Appetite
    public int? RiskAppetite { get; private set; }
    public bool WithinAppetite => ResidualRiskLevel.HasValue &&
                                   RiskAppetite.HasValue &&
                                   ResidualRiskLevel.Value <= RiskAppetite.Value;

    public string? Notes { get; private set; }
    public DateTime? NextReviewDate { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation property
    public Risk Risk { get; private set; }

    private RiskAssessment() { }

    public static RiskAssessment Create(
        Guid riskId,
        int probability,
        int impact,
        string probabilityJustification,
        string impactJustification,
        Guid assessedBy)
    {
        ValidateRiskLevel(probability, "Probability");
        ValidateRiskLevel(impact, "Impact");

        return new RiskAssessment
        {
            Id = Guid.NewGuid(),
            RiskId = riskId,
            Probability = probability,
            Impact = impact,
            ProbabilityJustification = probabilityJustification,
            ImpactJustification = impactJustification,
            InherentRiskLevel = probability * impact,
            AssessedBy = assessedBy,
            AssessmentDate = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetResidualRisk(int residualRiskLevel, string justification)
    {
        if (residualRiskLevel < 1 || residualRiskLevel > 25)
            throw new RiskDomainException("Residual risk level must be between 1 and 25");

        ResidualRiskLevel = residualRiskLevel;
        ResidualRiskJustification = justification;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetControlEffectiveness(int effectiveness, string existingControls)
    {
        ValidateRiskLevel(effectiveness, "Control Effectiveness");
        ControlEffectiveness = effectiveness;
        ExistingControls = existingControls;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRiskVelocity(int velocity)
    {
        ValidateRiskLevel(velocity, "Risk Velocity");
        RiskVelocity = velocity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRiskAppetite(int appetite)
    {
        if (appetite < 1 || appetite > 25)
            throw new RiskDomainException("Risk appetite must be between 1 and 25");

        RiskAppetite = appetite;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ScheduleNextReview(DateTime nextReviewDate)
    {
        if (nextReviewDate <= DateTime.UtcNow)
            throw new RiskDomainException("Next review date must be in the future");

        NextReviewDate = nextReviewDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateRiskLevel(int level, string fieldName)
    {
        if (level < 1 || level > 5)
            throw new RiskDomainException($"{fieldName} must be between 1 and 5");
    }
}