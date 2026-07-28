// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Aggregates/Role.cs
// ===========================================
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.Aggregates;

/// <summary>
/// Role aggregate root.
/// 
/// RESPONSIBILITY:
/// - Represents permission grouping.
/// - Protects system role from destructive operations.
/// - Controls permission lifecycle.
/// 
/// EF CORE COMPATIBILITY:
/// - Constructor binding aligned with property names.
/// - Private parameterless constructor for materialization.
/// </summary>
public sealed class Role : AggregateRoot
{   
    /// <summary>
    /// Gets role name.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// Gets whether role is system protected.
    /// </summary>
    public bool IsSystemRole { get; private set; }

    /// <summary>
    /// Gets role scope.
    ///
    /// Supported Scopes:
    /// - GLOBAL
    /// - TENANT
    /// - ORGANIZATION
    /// - BUSINESS_UNIT
    /// - DEPARTMENT
    /// </summary>
    public RoleScope Scope { get; private set; } = default!;

    /// <summary>
    /// Gets active flag.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets creation timestamp (UTC).
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Backs permission identifiers with case-insensitive hash set for efficient lookups and modifications. 
    /// </summary>
    private readonly HashSet<PermissionId> _permissionIds = [];

    /// <summary>
    /// Gets permission identifiers.
    /// </summary>
    public IReadOnlyCollection<PermissionId> PermissionIds => _permissionIds;

    /// <summary>
    /// EF Core constructor.
    /// DO NOT USE DIRECTLY.
    /// </summary>
    private Role(): base(){}

    /// <summary>
    /// Creates new Role aggregate.
    /// </summary>
    public Role(
        Guid id,
        string name,
        bool isSystemRole,
        RoleScope scope,
        DateTime createdAt)
        : base(id)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstNull(scope, nameof(scope));

        Name = name;
        IsSystemRole = isSystemRole;
        Scope = scope;
        CreatedAt = createdAt;
        IsActive = true;

    }

    // ============================================================
    // PERMISSION MANAGEMENT
    // ============================================================

    /// <summary>
    /// Adds a permission to the role.
    ///
    /// Responsibility:
    /// - Validate aggregate state.
    /// - Prevent modification of inactive roles.
    /// - Add permission to the permission set.
    ///
    /// Side Effects:
    /// - Updates permission collection.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="permissionId">
    /// Permission identifier.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when permission identifier is null.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the role is inactive.
    /// </exception>
    public void AddPermission(
        PermissionId permissionId)
    {
        Guard.AgainstNull(
            permissionId,
            nameof(permissionId));

        if (!IsActive)
        {
            throw new DomainException(
                "ROLE.INACTIVE",
                "Cannot modify permissions of an inactive role.");
        }

        _permissionIds.Add(
            permissionId);
    }

    /// <summary>
    /// Removes a permission from the role.
    ///
    /// Responsibility:
    /// - Validate aggregate state.
    /// - Protect system roles.
    /// - Prevent modification of inactive roles.
    /// - Remove permission.
    ///
    /// Side Effects:
    /// - Updates permission collection.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="permissionId">
    /// Permission identifier.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when permission identifier is null.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the role cannot be modified.
    /// </exception>
    public void RemovePermission(
        PermissionId permissionId)
    {
        Guard.AgainstNull(
            permissionId,
            nameof(permissionId));

        if (!IsActive)
        {
            throw new DomainException(
                "ROLE.INACTIVE",
                "Cannot modify permissions of an inactive role.");
        }

        if (IsSystemRole)
        {
            throw new DomainException(
                "ROLE.SYSTEM_PROTECTED",
                "System role permissions cannot be modified.");
        }

        _permissionIds.Remove(
            permissionId);
    }

    /// <summary>
    /// Determines whether the role
    /// contains the specified permission.
    ///
    /// Responsibility:
    /// - Check permission membership.
    /// - Support authorization engine.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    public bool HasPermission(
        PermissionId permissionId)
    {
        Guard.AgainstNull(
            permissionId,
            nameof(permissionId));

        return _permissionIds.Contains(
            permissionId);
    }

    /// <summary>
    /// Removes all assigned permissions.
    ///
    /// Responsibility:
    /// - Validate aggregate state.
    /// - Protect system roles.
    /// - Remove all permissions.
    ///
    /// Side Effects:
    /// - Clears permission collection.
    ///
    /// Complexity:
    /// O(n)
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown when the role cannot be modified.
    /// </exception>
    public void ClearPermissions()
    {
        if (!IsActive)
        {
            throw new DomainException(
                "ROLE.INACTIVE",
                "Cannot modify permissions of an inactive role.");
        }

        if (IsSystemRole)
        {
            throw new DomainException(
                "ROLE.SYSTEM_PROTECTED",
                "System role permissions cannot be modified.");
        }

        _permissionIds.Clear();
    }

    // ============================================================
    // LIFECYCLE
    // ============================================================

    /// <summary>
    /// Deactivates role.
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive) return;
        if (IsSystemRole)
        {
            throw new DomainException(
                "ROLE.SYSTEM_PROTECTED",
                "System role cannot be deactivated.");
        }

        IsActive = false;
    }

    /// <summary>
    /// Activates role.
    /// </summary>
    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
    }

    /// <summary>
    /// Renames the role.
    ///
    /// Responsibility:
    /// - Validate new role name.
    /// - Protect system roles.
    /// - Prevent modification of inactive roles.
    /// - Update role name.
    ///
    /// Side Effects:
    /// - Changes role name.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="newName">
    /// New role name.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when role name is null or empty.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the role cannot be modified.
    /// </exception>
    public void Rename(
        string newName)
    {
        Guard.AgainstNullOrWhiteSpace(
            newName,
            nameof(newName));

        if (!IsActive)
        {
            throw new DomainException(
                "ROLE.INACTIVE",
                "Cannot rename an inactive role.");
        }

        if (IsSystemRole)
        {
            throw new DomainException(
                "ROLE.SYSTEM_PROTECTED",
                "System role cannot be renamed.");
        }

        Name = newName;
    }
}