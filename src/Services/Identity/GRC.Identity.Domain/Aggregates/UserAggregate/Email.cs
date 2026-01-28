using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GRC.Identity.Domain.Aggregates.UserAggregate;

public class Email : ValueObject
{
    public string Value { get; private set; }
    private Email() { }
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format", nameof(value));

        Value = value.ToLowerInvariant().Trim();
    }
    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new IdentityDomainException("El email no puede estar vacío");
        }

        email = email.Trim().ToLowerInvariant();

        if (!IsValidEmail(email))
        {
            throw new IdentityDomainException("El formato del email no es válido");
        }

        if (email.Length > 256)
        {
            throw new IdentityDomainException("El email no puede exceder 256 caracteres");
        }

        return new Email(email);
    }
    private static bool IsValidEmail(string email)
    {
        var emailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        return emailRegex.IsMatch(email);
    }   
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value;
    public static implicit operator string(Email email) => email.Value;
}