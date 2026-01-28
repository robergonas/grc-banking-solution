using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.DomainEvents;
using GRC.Identity.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;

namespace GRC.Identity.Domain.Aggregates.RoleAggregate;

public class Role : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsSystemRole { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private readonly List<Permission> _permissions;
    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    private Role()
    {
        _permissions = new List<Permission>();
    }

    public Role(string name, string description, bool isSystemRole = false):this()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        _permissions = new List<Permission>();
    }
    //public Role(string name, string description, bool isSystemRole=false)
    //    : this(name, description)
    //{
    //    IsSystemRole = isSystemRole;
    //}

    public static Role Create(string name, string description, bool isSystemRole = false)
    {
        ValidateRoleData(name, description);
        return new Role(name, description, isSystemRole);
    }
    public void Update(string name, string description)
    {
        if (IsSystemRole)
        {
            throw new IdentityDomainException("No se puede modificar un rol del sistema");
        }

        ValidateRoleData(name, description);

        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new RoleUpdatedDomainEvent(Id, Name));
    }
    //public void UpdateDescription(string description)
    //{
    //    Description = description;
    //    UpdatedAt = DateTime.UtcNow;
    //}
    public void Activate()
    {
        if (IsActive)
        {
            throw new IdentityDomainException("El rol ya está activo");
        }

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    //public void Activate()
    //{
    //    IsActive = true;
    //    UpdatedAt = DateTime.UtcNow;
    //}
    public void Deactivate()
    {
        if (IsSystemRole)
        {
            throw new IdentityDomainException("No se puede desactivar un rol del sistema");
        }

        if (!IsActive)
        {
            throw new IdentityDomainException("El rol ya está inactivo");
        }

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    //public void Deactivate()
    //{
    //    IsActive = false;
    //    UpdatedAt = DateTime.UtcNow;
    //}
    public void AddPermission(Permission permission)
    {
        if (permission == null)
        {
            throw new ArgumentNullException(nameof(permission));
        }

        if (_permissions.Any(p => p.Id == permission.Id))
        {
            throw new IdentityDomainException($"El permiso '{permission.Name}' ya está asignado a este rol");
        }

        permission.AssignToRole(Id);
        _permissions.Add(permission);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PermissionAssignedDomainEvent(Id, permission.Id));
    }
    //public void AddPermission(Permission permission)
    //{
    //    if (_permissions.Exists(p => p.Name == permission.Name))
    //        throw new InvalidOperationException("Permission already exists in this role");

    //    _permissions.Add(permission);
    //    UpdatedAt = DateTime.UtcNow;
    //}

    //public void RemovePermission(string permissionName)
    //{
    //    var permission = _permissions.Find(p => p.Name == permissionName);
    //    if (permission != null)
    //    {
    //        _permissions.Remove(permission);
    //        UpdatedAt = DateTime.UtcNow;
    //    }
    //}
    public void RemovePermission(Guid permissionId)
    {
        var permission = _permissions.FirstOrDefault(p => p.Id == permissionId);
        if (permission == null)
        {
            throw new IdentityDomainException("El permiso no está asignado a este rol");
        }

        _permissions.Remove(permission);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PermissionRemovedDomainEvent(Id, permissionId));
    }

    public void ClearPermissions()
    {
        _permissions.Clear();
        UpdatedAt = DateTime.UtcNow;
    }
    public bool HasPermission(string resource, string action)
    {
        return _permissions.Any(p =>
            p.Resource.Equals(resource, StringComparison.OrdinalIgnoreCase) &&
            p.Action.Equals(action, StringComparison.OrdinalIgnoreCase));
    }
    public bool HasPermission(Guid permissionId)
    {
        return _permissions.Any(p => p.Id == permissionId);
    }
    private static void ValidateRoleData(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new IdentityDomainException("El nombre del rol no puede estar vacío");
        }

        if (name.Length > 100)
        {
            throw new IdentityDomainException("El nombre del rol no puede exceder 100 caracteres");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new IdentityDomainException("La descripción del rol no puede estar vacía");
        }

        if (description.Length > 500)
        {
            throw new IdentityDomainException("La descripción del rol no puede exceder 500 caracteres");
        }
    }
}