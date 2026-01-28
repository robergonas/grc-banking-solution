using GRC.BuildingBlocks.Domain.Exceptions;
using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using GRC.Governance.Domain.Aggregates.PolicyAggregate;

namespace GRC.Governance.Domain.Exceptions;

public class GovernanceDomainException : DomainException
{
    public GovernanceDomainException(string message) : base(message) { }

    public GovernanceDomainException(string message, Exception innerException)
        : base(message, innerException) { }
}

