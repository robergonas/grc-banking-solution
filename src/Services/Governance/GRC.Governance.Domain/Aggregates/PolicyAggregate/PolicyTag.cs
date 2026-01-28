using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Exceptions;
public class PolicyTag : ValueObject
{
    public string Name { get; private set; }

    private PolicyTag() { }

    public PolicyTag(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new GovernanceDomainException("Tag name cannot be empty");

        Name = name.ToLowerInvariant();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}