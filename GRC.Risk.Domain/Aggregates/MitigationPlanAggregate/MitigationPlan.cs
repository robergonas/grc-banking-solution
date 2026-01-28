using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Exceptions;
using GRC.Risk.Domain.DomainEvents;

namespace GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;

public class MitigationPlan : Entity, IAggregateRoot, IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<MitigationAction> _actions = new();

    public Guid RiskId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int StrategyId { get; private set; }
    public int Priority { get; private set; }
    public Guid OwnerId { get; private set; }
    public decimal? EstimatedCost { get; private set; }
    public string? EstimatedBenefit { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? TargetCompletionDate { get; private set; }
    public DateTime? ActualCompletionDate { get; private set; }
    public int StatusId { get; private set; }
    public int ProgressPercentage { get; private set; }
    public string? Barriers { get; private set; }
    public string? LessonsLearned { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public Guid? UpdatedBy { get; private set; }

    public IReadOnlyCollection<MitigationAction> Actions => _actions.AsReadOnly();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public int CompletedActionsCount => _actions.Count(a => a.StatusId == MitigationActionStatus.Completed.Id);
    public int TotalActionsCount => _actions.Count;
    public bool IsOverdue => !ActualCompletionDate.HasValue &&
                             TargetCompletionDate.HasValue &&
                             DateTime.UtcNow > TargetCompletionDate.Value;

    private MitigationPlan() { }

    public static MitigationPlan Create(
        Guid riskId,
        string title,
        string description,
        int strategyId,
        int priority,
        Guid ownerId,
        Guid? createdBy = null)
    {
        ValidateMitigationPlanData(title, description, strategyId, priority);

        var plan = new MitigationPlan
        {
            Id = Guid.NewGuid(),
            RiskId = riskId,
            Title = title,
            Description = description,
            StrategyId = strategyId,
            Priority = priority,
            OwnerId = ownerId,
            StatusId = MitigationPlanStatus.Planned.Id,
            ProgressPercentage = 0,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        plan.AddDomainEvent(new RiskMitigatedDomainEvent(
            riskId,
            plan.Id,
            strategyId));

        return plan;
    }

    public void UpdateDetails(
        string title,
        string description,
        int strategyId,
        int priority,
        Guid ownerId,
        Guid updatedBy)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new RiskDomainException("Mitigation plan title is required");

        Title = title;
        Description = description;
        StrategyId = strategyId;
        Priority = priority;
        OwnerId = ownerId;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void SetCostAndBenefit(decimal? estimatedCost, string estimatedBenefit)
    {
        if (estimatedCost.HasValue && estimatedCost < 0)
            throw new RiskDomainException("Estimated cost cannot be negative");

        EstimatedCost = estimatedCost;
        EstimatedBenefit = estimatedBenefit;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ScheduleDates(DateTime startDate, DateTime targetCompletionDate)
    {
        if (startDate >= targetCompletionDate)
            throw new RiskDomainException("Start date must be before target completion date");

        StartDate = startDate;
        TargetCompletionDate = targetCompletionDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddAction(
        string description,
        Guid responsibleId,
        DateTime dueDate)
    {
        var actionNumber = _actions.Count + 1;
        var action = MitigationAction.Create(
            Id,
            actionNumber,
            description,
            responsibleId,
            dueDate);

        _actions.Add(action);
        UpdateProgress();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveAction(Guid actionId)
    {
        var action = _actions.FirstOrDefault(a => a.Id == actionId);
        if (action == null)
            throw new RiskDomainException("Action not found");

        _actions.Remove(action);

        // Renumber remaining actions
        RenumberActions();
        UpdateProgress();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Start()
    {
        if (StatusId != MitigationPlanStatus.Planned.Id)
            throw new RiskDomainException("Only planned mitigation plans can be started");

        if (!StartDate.HasValue)
            throw new RiskDomainException("Start date must be set before starting");

        StatusId = MitigationPlanStatus.InProgress.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete(string lessonsLearned = null)
    {
        if (StatusId != MitigationPlanStatus.InProgress.Id)
            throw new RiskDomainException("Only in-progress mitigation plans can be completed");

        StatusId = MitigationPlanStatus.Completed.Id;
        ActualCompletionDate = DateTime.UtcNow;
        ProgressPercentage = 100;
        LessonsLearned = lessonsLearned;
        UpdatedAt = DateTime.UtcNow;
    }

    public void PutOnHold(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new RiskDomainException("Reason for putting on hold is required");

        if (StatusId != MitigationPlanStatus.InProgress.Id)
            throw new RiskDomainException("Only in-progress mitigation plans can be put on hold");

        StatusId = MitigationPlanStatus.OnHold.Id;
        Barriers = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resume()
    {
        if (StatusId != MitigationPlanStatus.OnHold.Id)
            throw new RiskDomainException("Only on-hold mitigation plans can be resumed");

        StatusId = MitigationPlanStatus.InProgress.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new RiskDomainException("Reason for cancellation is required");

        if (StatusId == MitigationPlanStatus.Completed.Id)
            throw new RiskDomainException("Cannot cancel a completed mitigation plan");

        StatusId = MitigationPlanStatus.Cancelled.Id;
        Barriers = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordBarrier(string barrier)
    {
        Barriers = string.IsNullOrEmpty(Barriers)
            ? barrier
            : $"{Barriers}; {barrier}";
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProgress()
    {
        if (TotalActionsCount == 0)
        {
            ProgressPercentage = 0;
            return;
        }

        ProgressPercentage = (CompletedActionsCount * 100) / TotalActionsCount;
        UpdatedAt = DateTime.UtcNow;
    }

    private void RenumberActions()
    {
        var orderedActions = _actions.OrderBy(a => a.ActionNumber).ToList();
        for (int i = 0; i < orderedActions.Count; i++)
        {
            orderedActions[i].UpdateActionNumber(i + 1);
        }
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private static void ValidateMitigationPlanData(
        string title,
        string description,
        int strategyId,
        int priority)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new RiskDomainException("Mitigation plan title is required");

        if (string.IsNullOrWhiteSpace(description))
            throw new RiskDomainException("Mitigation plan description is required");

        if (strategyId <= 0)
            throw new RiskDomainException("Valid mitigation strategy is required");

        if (priority < 1 || priority > 5)
            throw new RiskDomainException("Priority must be between 1 and 5");
    }
}