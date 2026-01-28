using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.DomainEvents;
using GRC.Identity.Domain.Exceptions;

namespace GRC.Identity.Domain.Aggregates.UserAggregate;

public class User : Entity, IAggregateRoot
{
    public Email Email { get; private set; }
    public string FullName { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime? LastLoginDate { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }    

    private readonly List<UserRole> _userRoles = new List<UserRole>();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public string _passwordHash { get; set; }

    private User()
    {
    }
    public User(string email, string password, string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));

        Id = Guid.NewGuid();
        Email = new Email(email);
        _passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        FullName = fullName;
        Status = UserStatus.Active;
        CreatedAt = DateTime.UtcNow;
        FailedLoginAttempts = 0;

        AddDomainEvent(new UserCreatedDomainEvent(Id, Email.Value, FullName));
    }

    public bool ValidatePassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, _passwordHash);
    }

    public void RecordSuccessfulLogin()
    {
        LastLoginDate = DateTime.UtcNow;
        FailedLoginAttempts = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;
        UpdatedAt = DateTime.UtcNow;

        if (FailedLoginAttempts >= 5)
        {
            Lock();
        }
    }

    public void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Lock()
    {
        Status = UserStatus.Locked;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unlock()
    {
        Status = UserStatus.Active;
        FailedLoginAttempts = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddRole(Guid roleId)
    {
        if (_userRoles.Any(ur => ur.RoleId == roleId))
            throw new InvalidOperationException("User already has this role");

        _userRoles.Add(new UserRole(Id, roleId));
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveRole(Guid roleId)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId);
        if (userRole != null)
        {
            _userRoles.Remove(userRole);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    // Método para cambiar todos los roles de una vez (requerido por ChangeUserRolesCommandHandler)
    public void ChangeRoles(IEnumerable<Guid> roleIds)
    {
        var oldRoleIds = _userRoles.Select(ur => ur.RoleId).ToList();

        _userRoles.Clear();

        foreach (var roleId in roleIds)
        {
            _userRoles.Add(new UserRole(Id, roleId));
        }

        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserRolesChangedDomainEvent(Id, oldRoleIds));
    }

    public bool HasRole(Guid roleId)
    {
        return _userRoles.Any(ur => ur.RoleId == roleId);
    }

    public bool IsLocked()
    {
        return Status == UserStatus.Locked;
    }

    public void UpdateEmail(Email newEmail)
    {
        if (newEmail == null)
            throw new IdentityDomainException("El email no puede ser nulo");

        Email = newEmail;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new IdentityDomainException("El nombre completo no puede estar vacío");

        if (fullName.Length > 200)
            throw new IdentityDomainException("El nombre completo no puede exceder 200 caracteres");

        FullName = fullName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new IdentityDomainException("El hash de la contraseña no puede estar vacío");

        _passwordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
        FailedLoginAttempts = 0;
    }

    public bool IsActive()
    {
        return Status.Id == UserStatus.Active.Id;
    }
}