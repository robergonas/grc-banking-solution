namespace GRC.Risk.Application.DTOs;

public class RiskAssessmentDto
{
    public Guid Id { get; set; }
    public Guid RiskId { get; set; }
    public DateTime AssessmentDate { get; set; }
    public Guid AssessedBy { get; set; }
    public string? AssessedByName { get; set; }

    public int Probability { get; set; }
    public string? ProbabilityJustification { get; set; }

    public int Impact { get; set; }
    public string? ImpactJustification { get; set; }

    public int InherentRiskLevel { get; set; }
    public string InherentRiskLevelText => GetRiskLevelText(InherentRiskLevel);

    public string? ExistingControls { get; set; }
    public int? ControlEffectiveness { get; set; }

    public int? ResidualRiskLevel { get; set; }
    public string? ResidualRiskLevelText => ResidualRiskLevel.HasValue
        ? GetRiskLevelText(ResidualRiskLevel.Value)
        : null;
    public string? ResidualRiskJustification { get; set; }

    public int? RiskVelocity { get; set; }
    public int? RiskAppetite { get; set; }
    public bool WithinAppetite { get; set; }

    public string? Notes { get; set; }
    public DateTime? NextReviewDate { get; set; }
    public bool IsActive { get; set; }

    private static string GetRiskLevelText(int level)
    {
        return level switch
        {
            >= 20 => "Critical",
            >= 15 => "High",
            >= 10 => "Medium",
            >= 5 => "Low",
            _ => "Very Low"
        };
    }
}

public class RiskIndicatorDto
{
    public Guid Id { get; set; }
    public Guid RiskId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MeasurementUnit { get; set; } = string.Empty;
    public string? DataSource { get; set; }
    public int MeasurementFrequency { get; set; }
    public decimal? ThresholdGreen { get; set; }
    public decimal? ThresholdYellow { get; set; }
    public decimal? ThresholdRed { get; set; }
    public Guid OwnerId { get; set; }
    public bool IsActive { get; set; }
}