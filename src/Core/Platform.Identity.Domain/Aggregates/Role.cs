// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Aggregates/Role.cs
// ===========================================

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
    /// Gets scope type (Global, Subsidiary, etc).
    /// </summary>
    public string ScopeType { get; private set; } = default!;

    /// <summary>
    /// Gets active flag.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets creation timestamp (UTC).
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    private readonly HashSet<string> _permissionIds = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets permission identifiers.
    /// </summary>
    public IReadOnlyCollection<string> PermissionIds
        => _permissionIds.ToList().AsReadOnly();

    /// <summary>
    /// EF Core constructor.
    /// DO NOT USE DIRECTLY.
    /// </summary>
    private Role()
        : base(Guid.Empty)
    {
    }

    /// <summary>
    /// Creates new Role aggregate.
    /// </summary>
    public Role(
        Guid id,
        string name,
        bool isSystemRole,
        string scopeType,
        DateTime createdAt)
        : base(id)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstNullOrWhiteSpace(scopeType, nameof(scopeType));

        Name = name;
        IsSystemRole = isSystemRole;
        ScopeType = scopeType;
        CreatedAt = createdAt;
        IsActive = true;

        _permissionIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    // ============================================================
    // PERMISSION MANAGEMENT
    // ============================================================

    /// <summary>
    /// Adds permission to role.
    /// </summary>
    public void AddPermission(string permissionId)
    {
        Guard.AgainstNullOrWhiteSpace(permissionId, nameof(permissionId));

        if (!IsActive)
        {
            throw new DomainException(
                "ROLE.INACTIVE",
                "Cannot modify permissions of inactive role.");
        }

        _permissionIds.Add(permissionId);
    }

    /// <summary>
    /// Removes permission from role.
    /// </summary>
    public void RemovePermission(string permissionId)
    {
        Guard.AgainstNullOrWhiteSpace(permissionId, nameof(permissionId));

        if (IsSystemRole)
        {
            throw new DomainException(
                "ROLE.SYSTEM_PROTECTED",
                "System role permissions cannot be modified.");
        }

        _permissionIds.Remove(permissionId);
    }

    // ============================================================
    // LIFECYCLE
    // ============================================================

    /// <summary>
    /// Deactivates role.
    /// </summary>
    public void Deactivate()
    {
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
        IsActive = true;
    }

    /// <summary>
    /// Renames role.
    /// </summary>
    public void Rename(string newName)
    {
        Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
        Name = newName;
    }
}