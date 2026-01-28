using GRC.BuildingBlocks.Domain.Exceptions;

namespace GRC.Risk.Domain.Exceptions;

public class RiskDomainException : DomainException
{
    public RiskDomainException()
    { }

    public RiskDomainException(string message)
        : base(message)
    { }

    public RiskDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}