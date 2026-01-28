using GRC.BuildingBlocks.Domain.SeedWork;

public class VoteResult : ValueObject
{
    public int VotesFor { get; private set; }
    public int VotesAgainst { get; private set; }
    public int Abstentions { get; private set; }
    public bool Passed { get; private set; }

    private VoteResult() { }

    public VoteResult(int votesFor, int votesAgainst, int abstentions)
    {
        VotesFor = votesFor;
        VotesAgainst = votesAgainst;
        Abstentions = abstentions;
        Passed = votesFor > votesAgainst;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return VotesFor;
        yield return VotesAgainst;
        yield return Abstentions;
    }
}