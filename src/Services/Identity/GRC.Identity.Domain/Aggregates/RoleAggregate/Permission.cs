using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.Exceptions;

namespace GRC.Identity.Domain.Aggregates.RoleAggregate;

public class Permission : Entity
{
    public string Name { get; private set; }
    public string Resource { get; private set; }
    public string Action { get; private set; }
    public Guid RoleId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation property
    public Role Role { get; private set; }

    private Permission() { }

    private Permission(string name, string resource, string action)
    {
        Name = name;
        Resource = resource;
        Action = action;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Permission Create(string resource, string action)
    {
        ValidatePermissionData(resource, action);

        var name = $"{resource}.{action}";
        return new Permission(name, resource, action);
    }

    public void Update(string resource, string action)
    {
        ValidatePermissionData(resource, action);

        Resource = resource;
        Action = action;
        Name = $"{resource}.{action}";
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignToRole(Guid roleId)
    {
        if (roleId == Guid.Empty)
        {
            throw new IdentityDomainException("El ID del rol no puede estar vacío");
        }

        RoleId = roleId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidatePermissionData(string resource, string action)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new IdentityDomainException("El recurso del permiso no puede estar vacío");
        }

        if (resource.Length > 100)
        {
            throw new IdentityDomainException("El recurso del permiso no puede exceder 100 caracteres");
        }

        if (string.IsNullOrWhiteSpace(action))
        {
            throw new IdentityDomainException("La acción del permiso no puede estar vacía");
        }

        if (action.Length > 50)
        {
            throw new IdentityDomainException("La acción del permiso no puede exceder 50 caracteres");
        }

        // Validar que action sea una de las permitidas
        var validActions = new[] { "Create", "Read", "Update", "Delete", "Approve", "Reject", "Publish", "Execute" };
        if (!validActions.Contains(action, StringComparer.OrdinalIgnoreCase))
        {
            throw new IdentityDomainException(
                $"La acción '{action}' no es válida. Acciones permitidas: {string.Join(", ", validActions)}");
        }
    }

    public override string ToString() => Name;
}