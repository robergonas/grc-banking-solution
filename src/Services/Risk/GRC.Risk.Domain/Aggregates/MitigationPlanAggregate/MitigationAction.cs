using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Exceptions;

namespace GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;

public class MitigationAction : Entity
{
    public Guid MitigationPlanId { get; private set; }
    public int ActionNumber { get; private set; }
    public string Description { get; private set; }
    public Guid ResponsibleId { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public int StatusId { get; private set; }
    public string? Comments { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation property
    public MitigationPlan MitigationPlan { get; private set; }

    public bool IsOverdue => !CompletedDate.HasValue && DateTime.UtcNow > DueDate;

    private MitigationAction() { }

    public static MitigationAction Create(
        Guid mitigationPlanId,
        int actionNumber,
        string description,
        Guid responsibleId,
        DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new RiskDomainException("Action description is required");

        if (dueDate <= DateTime.UtcNow)
            throw new RiskDomainException("Due date must be in the future");

        return new MitigationAction
        {
            Id = Guid.NewGuid(),
            MitigationPlanId = mitigationPlanId,
            ActionNumber = actionNumber,
            Description = description,
            ResponsibleId = responsibleId,
            DueDate = dueDate,
            StatusId = MitigationActionStatus.NotStarted.Id,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new RiskDomainException("Action description is required");

        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateActionNumber(int actionNumber)
    {
        if (actionNumber <= 0)
            throw new RiskDomainException("Action number must be positive");

        ActionNumber = actionNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignTo(Guid responsibleId)
    {
        ResponsibleId = responsibleId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RescheduleDueDate(DateTime newDueDate)
    {
        if (newDueDate <= DateTime.UtcNow)
            throw new RiskDomainException("Due date must be in the future");

        DueDate = newDueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Start()
    {
        if (StatusId != MitigationActionStatus.NotStarted.Id)
            throw new RiskDomainException("Only not started actions can be started");

        StatusId = MitigationActionStatus.InProgress.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete(string comments = null)
    {
        if (StatusId == MitigationActionStatus.Completed.Id)
            throw new RiskDomainException("Action is already completed");

        StatusId = MitigationActionStatus.Completed.Id;
        CompletedDate = DateTime.UtcNow;
        Comments = comments;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Block(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new RiskDomainException("Reason for blocking is required");

        StatusId = MitigationActionStatus.Blocked.Id;
        Comments = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unblock()
    {
        if (StatusId != MitigationActionStatus.Blocked.Id)
            throw new RiskDomainException("Only blocked actions can be unblocked");

        StatusId = MitigationActionStatus.InProgress.Id;
        UpdatedAt = DateTime.UtcNow;
    }
}