namespace GRC.Risk.Application.DTOs;

public class MitigationPlanDto
{
    public Guid Id { get; set; }
    public Guid RiskId { get; set; }
    public string? RiskCode { get; set; }
    public string? RiskTitle { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StrategyId { get; set; }
    public string StrategyName { get; set; } = string.Empty;
    public int Priority { get; set; }
    public string PriorityText => GetPriorityText(Priority);
    public Guid OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public decimal? EstimatedCost { get; set; }
    public string? EstimatedBenefit { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? TargetCompletionDate { get; set; }
    public DateTime? ActualCompletionDate { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public int ProgressPercentage { get; set; }
    public string? Barriers { get; set; }
    public string? LessonsLearned { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int TotalActions { get; set; }
    public int CompletedActions { get; set; }
    public bool IsOverdue { get; set; }

    private static string GetPriorityText(int priority)
    {
        return priority switch
        {
            5 => "Critical",
            4 => "High",
            3 => "Medium",
            2 => "Low",
            1 => "Very Low",
            _ => "Unknown"
        };
    }
}

public class MitigationPlanDetailDto : MitigationPlanDto
{
    public List<MitigationActionDto> Actions { get; set; } = new();
}

public class MitigationActionDto
{
    public Guid Id { get; set; }
    public Guid MitigationPlanId { get; set; }
    public int ActionNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid ResponsibleId { get; set; }
    public string? ResponsibleName { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? Comments { get; set; }
    public bool IsOverdue { get; set; }
}