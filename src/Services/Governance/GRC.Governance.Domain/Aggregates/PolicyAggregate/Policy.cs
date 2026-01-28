using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.DomainEvents;
using GRC.Governance.Domain.Exceptions;

namespace GRC.Governance.Domain.Aggregates.PolicyAggregate;

/// <summary>
/// Policy Aggregate Root - Representa una política corporativa de gobernanza
/// </summary>
public class Policy : Entity, IAggregateRoot
{
    public string Code { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public PolicyType Type { get; private set; }
    public PolicyStatus Status { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public int Version { get; private set; }
    public Guid? ParentPolicyId { get; private set; }

    // Owner info
    public Guid OwnerId { get; private set; }
    public string OwnerName { get; private set; }

    // Approval workflow
    public Guid? ApprovedById { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public string? ApprovalComments { get; private set; }

    // Review cycle
    public int ReviewCycleMonths { get; private set; }
    public DateTime? LastReviewDate { get; private set; }
    public DateTime? NextReviewDate { get; private set; }

    private readonly List<PolicyDocument> _documents;
    public IReadOnlyCollection<PolicyDocument> Documents => _documents.AsReadOnly();

    private readonly List<PolicyTag> _tags;
    public IReadOnlyCollection<PolicyTag> Tags => _tags.AsReadOnly();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    // EF Core constructor
    private Policy()
    {
        _documents = new List<PolicyDocument>();
        _tags = new List<PolicyTag>();
    }

    public Policy(
        string code,
        string title,
        string description,
        PolicyType type,
        Guid ownerId,
        string ownerName,
        DateTime effectiveDate,
        int reviewCycleMonths)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new GovernanceDomainException("Policy code cannot be empty");

        if (string.IsNullOrWhiteSpace(title))
            throw new GovernanceDomainException("Policy title cannot be empty");

        if (reviewCycleMonths < 1)
            throw new GovernanceDomainException("Review cycle must be at least 1 month");

        Id = Guid.NewGuid();
        Code = code.ToUpperInvariant();
        Title = title;
        Description = description;
        Type = type;
        Status = PolicyStatus.Draft;
        OwnerId = ownerId;
        OwnerName = ownerName;
        EffectiveDate = effectiveDate;
        ReviewCycleMonths = reviewCycleMonths;
        Version = 1;
        CreatedAt = DateTime.UtcNow;

        _documents = new List<PolicyDocument>();
        _tags = new List<PolicyTag>();

        AddDomainEvent(new PolicyCreatedDomainEvent(Id, Code, Title, Type));
    }

    public void UpdateDetails(string title, string description)
    {
        if (Status == PolicyStatus.Archived)
            throw new GovernanceDomainException("Cannot update archived policy");

        Title = title ?? Title;
        Description = description ?? Description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SubmitForApproval()
    {
        if (Status != PolicyStatus.Draft)
            throw new GovernanceDomainException($"Cannot submit policy in {Status} status");

        Status = PolicyStatus.PendingApproval;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve(Guid approvedById, string approverName, string comments = null)
    {
        if (Status != PolicyStatus.PendingApproval)
            throw new GovernanceDomainException($"Cannot approve policy in {Status} status");

        Status = PolicyStatus.Approved;
        ApprovedById = approvedById;
        ApprovedDate = DateTime.UtcNow;
        ApprovalComments = comments;
        CalculateNextReviewDate();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PolicyApprovedDomainEvent(Id, Code, approvedById, approverName));
    }

    public void Reject(string reason)
    {
        if (Status != PolicyStatus.PendingApproval)
            throw new GovernanceDomainException($"Cannot reject policy in {Status} status");

        Status = PolicyStatus.Draft;
        ApprovalComments = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        if (Status != PolicyStatus.Approved)
            throw new GovernanceDomainException("Can only publish approved policies");

        Status = PolicyStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkForReview()
    {
        if (Status != PolicyStatus.Active)
            throw new GovernanceDomainException("Can only review active policies");

        Status = PolicyStatus.UnderReview;
        LastReviewDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CompleteReview()
    {
        if (Status != PolicyStatus.UnderReview)
            throw new GovernanceDomainException("Policy is not under review");

        Status = PolicyStatus.Active;
        CalculateNextReviewDate();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status != PolicyStatus.Active)
            throw new GovernanceDomainException("Can only expire active policies");

        Status = PolicyStatus.Expired;
        ExpirationDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PolicyExpiredDomainEvent(Id, Code));
    }

    public void Archive(string reason)
    {
        Status = PolicyStatus.Archived;
        ApprovalComments = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CreateNewVersion()
    {
        if (Status == PolicyStatus.Draft)
            throw new GovernanceDomainException("Cannot version a draft policy");

        Version++;
        Status = PolicyStatus.Draft;
        ApprovedById = null;
        ApprovedDate = null;
        ApprovalComments = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddDocument(string fileName, string fileUrl, string documentType)
    {
        var document = new PolicyDocument(fileName, fileUrl, documentType);
        _documents.Add(document);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveDocument(Guid documentId)
    {
        var document = _documents.FirstOrDefault(d => d.Id == documentId);
        if (document != null)
        {
            _documents.Remove(document);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void AddTag(string tagName)
    {
        if (_tags.Any(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
            return;

        _tags.Add(new PolicyTag(tagName));
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveTag(string tagName)
    {
        var tag = _tags.FirstOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        if (tag != null)
        {
            _tags.Remove(tag);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public bool IsExpiringSoon(int daysThreshold = 30)
    {
        return NextReviewDate.HasValue &&
               NextReviewDate.Value <= DateTime.UtcNow.AddDays(daysThreshold);
    }

    private void CalculateNextReviewDate()
    {
        LastReviewDate = DateTime.UtcNow;
        NextReviewDate = LastReviewDate.Value.AddMonths(ReviewCycleMonths);
    }
}

