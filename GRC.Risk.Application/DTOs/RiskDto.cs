namespace GRC.Risk.Application.DTOs;

public class RiskDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? Source { get; set; }
    public Guid OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public Guid IdentifiedBy { get; set; }
    public string? IdentifiedByName { get; set; }
    public DateTime IdentifiedDate { get; set; }
    public DateTime? ReviewDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Latest Assessment
    public RiskAssessmentDto? LatestAssessment { get; set; }

    // Calculated fields
    public int? CurrentRiskLevel => LatestAssessment?.InherentRiskLevel;
    public int? ResidualRiskLevel => LatestAssessment?.ResidualRiskLevel;
    public bool? WithinAppetite => LatestAssessment?.WithinAppetite;
}

public class RiskDetailDto : RiskDto
{
    public List<RiskAssessmentDto> Assessments { get; set; } = new();
    public List<RiskIndicatorDto> Indicators { get; set; } = new();
}