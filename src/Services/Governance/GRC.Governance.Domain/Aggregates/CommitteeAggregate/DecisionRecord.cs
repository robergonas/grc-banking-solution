using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;

public class DecisionRecord : Entity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DecisionType Type { get; private set; }
    public string Rationale { get; private set; }
    public DateTime DecisionDate { get; private set; }
    public Guid DecidedById { get; private set; }
    public string DecidedByName { get; private set; }
    public VoteResult VoteResult { get; private set; }
    public DateTime CreatedAt { get; set; }

    private DecisionRecord() { }

    public DecisionRecord(
        string title,
        string description,
        DecisionType type,
        string rationale,
        Guid decidedById,
        string decidedByName,
        VoteResult voteResult = null)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Type = type;
        Rationale = rationale;
        DecisionDate = DateTime.UtcNow;
        DecidedById = decidedById;
        DecidedByName = decidedByName;
        VoteResult = voteResult;
        CreatedAt = DateTime.UtcNow;
    }
}