using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Exceptions;
using GRC.Risk.Domain.DomainEvents;

namespace GRC.Risk.Domain.Aggregates.ControlAggregate;

public class Control : Entity, IAggregateRoot, IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int TypeId { get; private set; }
    public int NatureId { get; private set; }
    public int FrequencyId { get; private set; }
    public Guid OwnerId { get; private set; }
    public DateTime? ImplementationDate { get; private set; }
    public int StatusId { get; private set; }
    public int? EffectivenessRating { get; private set; }
    public DateTime? LastTestedDate { get; private set; }
    public DateTime? NextTestDate { get; private set; }
    public decimal? Cost { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public Guid? UpdatedBy { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Control() { }

    public static Control Create(
        string code,
        string name,
        string description,
        int typeId,
        int natureId,
        int frequencyId,
        Guid ownerId,
        Guid? createdBy = null)
    {
        ValidateControlData(code, name, typeId, natureId, frequencyId);

        var control = new Control
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            Description = description,
            TypeId = typeId,
            NatureId = natureId,
            FrequencyId = frequencyId,
            OwnerId = ownerId,
            StatusId = ControlStatus.Planned.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        control.AddDomainEvent(new ControlImplementedDomainEvent(
            control.Id,
            control.Code,
            control.Name,
            control.TypeId,
            control.OwnerId));

        return control;
    }

    public void UpdateDetails(
        string name,
        string description,
        int typeId,
        int natureId,
        int frequencyId,
        Guid ownerId,
        Guid updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new RiskDomainException("Control name is required");

        Name = name;
        Description = description;
        TypeId = typeId;
        NatureId = natureId;
        FrequencyId = frequencyId;
        OwnerId = ownerId;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Implement(DateTime implementationDate)
    {
        if (implementationDate > DateTime.UtcNow)
            throw new RiskDomainException("Implementation date cannot be in the future");

        ImplementationDate = implementationDate;
        StatusId = ControlStatus.Implemented.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetEffectivenessRating(int rating)
    {
        if (rating < 1 || rating > 5)
            throw new RiskDomainException("Effectiveness rating must be between 1 and 5");

        EffectivenessRating = rating;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordTest(DateTime testDate, int effectivenessRating)
    {
        if (testDate > DateTime.UtcNow)
            throw new RiskDomainException("Test date cannot be in the future");

        SetEffectivenessRating(effectivenessRating);
        LastTestedDate = testDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ScheduleNextTest(DateTime nextTestDate)
    {
        if (nextTestDate <= DateTime.UtcNow)
            throw new RiskDomainException("Next test date must be in the future");

        NextTestDate = nextTestDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCost(decimal cost)
    {
        if (cost < 0)
            throw new RiskDomainException("Cost cannot be negative");

        Cost = cost;
        UpdatedAt = DateTime.UtcNow;
    }

    public void StartReview()
    {
        if (StatusId != ControlStatus.Implemented.Id)
            throw new RiskDomainException("Only implemented controls can be reviewed");

        StatusId = ControlStatus.UnderReview.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        StatusId = ControlStatus.Inactive.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        if (StatusId != ControlStatus.Inactive.Id)
            throw new RiskDomainException("Only inactive controls can be reactivated");

        StatusId = ControlStatus.Planned.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    private static void ValidateControlData(
        string code,
        string name,
        int typeId,
        int natureId,
        int frequencyId)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new RiskDomainException("Control code is required");

        if (string.IsNullOrWhiteSpace(name))
            throw new RiskDomainException("Control name is required");

        if (typeId <= 0)
            throw new RiskDomainException("Valid control type is required");

        if (natureId <= 0)
            throw new RiskDomainException("Valid control nature is required");

        if (frequencyId <= 0)
            throw new RiskDomainException("Valid control frequency is required");
    }
}