using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.Exceptions;
//using GRC.Identity.Domain.Services;
using GRC.Identity.Infrastructure.Services;

namespace GRC.Identity.Domain.Aggregates.UserAggregate;

public class Password : ValueObject
{
    public string HashedValue { get; private set; }

    private Password() { }

    private Password(string hashedValue)
    {
        HashedValue = hashedValue;
    }

    public static Password Create(string plainPassword, IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
        {
            throw new IdentityDomainException("La contraseña no puede estar vacía");
        }

        if (plainPassword.Length < 6)
        {
            throw new IdentityDomainException("La contraseña debe tener al menos 6 caracteres");
        }

        var hashedPassword = passwordHasher.HashPassword(plainPassword);
        return new Password(hashedPassword);
    }

    // Método para crear desde un hash ya existente (útil para seed data)
    public static Password CreateFromHash(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new IdentityDomainException("El hash de contraseña no puede estar vacío");
        }

        return new Password(hashedPassword);
    }

    public bool Verify(string plainPassword, IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
        {
            return false;
        }

        return passwordHasher.VerifyPassword(HashedValue, plainPassword);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return HashedValue;
    }
}