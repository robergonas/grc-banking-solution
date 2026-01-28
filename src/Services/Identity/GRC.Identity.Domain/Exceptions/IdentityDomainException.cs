using GRC.BuildingBlocks.Domain.Exceptions;

namespace GRC.Identity.Domain.Exceptions;

public class IdentityDomainException : DomainException
{
    public IdentityDomainException()
    { }

    public IdentityDomainException(string message)
        : base(message)
    { }

    public IdentityDomainException(string message, System.Exception innerException)
        : base(message, innerException)
    { }
}