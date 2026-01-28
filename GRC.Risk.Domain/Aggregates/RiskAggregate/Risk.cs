using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Exceptions;
using GRC.Risk.Domain.DomainEvents;

namespace GRC.Risk.Domain.Aggregates.RiskAggregate;
public class Risk : Entity, IAggregateRoot, IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<RiskAssessment> _assessments = new();
    private readonly List<RiskIndicator> _indicators = new();

    public string Code { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public int CategoryId { get; private set; }
    public int TypeId { get; private set; }
    public int StatusId { get; private set; }
    public string? Source { get; private set; }
    public Guid OwnerId { get; private set; }
    public Guid IdentifiedBy { get; private set; }
    public DateTime IdentifiedDate { get; private set; }
    public DateTime? ReviewDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public IReadOnlyCollection<RiskAssessment> Assessments => _assessments.AsReadOnly();
    public IReadOnlyCollection<RiskIndicator> Indicators => _indicators.AsReadOnly();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // Latest active assessment
    public RiskAssessment? LatestAssessment => _assessments
        .Where(a => a.IsActive)
        .OrderByDescending(a => a.AssessmentDate)
        .FirstOrDefault();
    private Risk() { }
    public static Risk Create(
        string code,
        string title,
        string description,
        int categoryId,
        int typeId,
        string source,
        Guid identifiedBy,
        Guid ownerId,
        Guid? createdBy = null)
    {
        ValidateRiskData(code, title, categoryId);

        var risk = new Risk
        {
            Id = Guid.NewGuid(),
            Code = code,
            Title = title,
            Description = description,
            CategoryId = categoryId,
            TypeId = typeId,
            StatusId = RiskStatus.Identified.Id,
            Source = source,
            IdentifiedBy = identifiedBy,
            OwnerId = ownerId,
            IdentifiedDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        risk.AddDomainEvent(new RiskIdentifiedDomainEvent(
            risk.Id,
            risk.Code,
            risk.Title,
            risk.CategoryId,
            risk.IdentifiedBy));

        return risk;
    }
    public void UpdateDetails(
        string title,
        string description,
        int categoryId,
        int typeId,
        string source,
        Guid ownerId,
        Guid updatedBy)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new RiskDomainException("Risk title is required");

        Title = title;
        Description = description;
        CategoryId = categoryId;
        TypeId = typeId;
        Source = source;
        OwnerId = ownerId;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
    public void Assess(
        int probability,
        int impact,
        string probabilityJustification,
        string impactJustification,
        Guid assessedBy)
    {
        // Deactivate previous assessments
        foreach (var assessment in _assessments.Where(a => a.IsActive))
        {
            assessment.Deactivate();
        }

        var newAssessment = RiskAssessment.Create(
            Id,
            probability,
            impact,
            probabilityJustification,
            impactJustification,
            assessedBy);

        _assessments.Add(newAssessment);
        StatusId = RiskStatus.Assessed.Id;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new RiskAssessedDomainEvent(
            Id,
            newAssessment.Id,
            probability,
            impact,
            newAssessment.InherentRiskLevel,
            newAssessment.ResidualRiskLevel,
            assessedBy));
    }
    public void AddIndicator(
        string name,
        string description,
        string measurementUnit,
        int measurementFrequency,
        Guid ownerId)
    {
        var indicator = RiskIndicator.Create(
            Id,
            name,
            description,
            measurementUnit,
            measurementFrequency,
            ownerId);

        _indicators.Add(indicator);
        UpdatedAt = DateTime.UtcNow;
    }
    public void ScheduleReview(DateTime reviewDate)
    {
        if (reviewDate <= DateTime.UtcNow)
            throw new RiskDomainException("Review date must be in the future");

        ReviewDate = reviewDate;
        UpdatedAt = DateTime.UtcNow;
    }
    public void StartTreatment()
    {
        if (StatusId != RiskStatus.Assessed.Id)
            throw new RiskDomainException("Risk must be assessed before treatment");

        StatusId = RiskStatus.UnderTreatment.Id;
        UpdatedAt = DateTime.UtcNow;
    }
    public void MarkAsMitigated()
    {
        if (StatusId != RiskStatus.UnderTreatment.Id)
            throw new RiskDomainException("Risk must be under treatment to be mitigated");

        StatusId = RiskStatus.Mitigated.Id;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Accept(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new RiskDomainException("Reason for accepting risk is required");

        StatusId = RiskStatus.Accepted.Id;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Close()
    {
        StatusId = RiskStatus.Closed.Id;
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
    private static void ValidateRiskData(string code, string title, int categoryId)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new RiskDomainException("Risk code is required");

        if (string.IsNullOrWhiteSpace(title))
            throw new RiskDomainException("Risk title is required");

        if (categoryId <= 0)
            throw new RiskDomainException("Valid risk category is required");
    }
}