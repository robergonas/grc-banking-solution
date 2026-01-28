//using GRC.Identity.Domain.Services;
using System;

namespace GRC.Identity.Infrastructure.Services;
public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch
        {
            return false;
        }
    }

    public (string hash, string salt) GenerateHashAndSalt(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        // BCrypt maneja el salt internamente, pero para compatibilidad con la interfaz
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return (hash, string.Empty); // BCrypt incluye el salt en el hash
    }
}