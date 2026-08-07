// ===========================================
// File Location:
// tests/Application/
// Platform.Identity.Application.UnitTests/
// Fixtures/
// RoleFixture.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Identity.Application.UnitTests.Fixtures;

/// <summary>
/// Provides reusable <see cref="Role"/> aggregates
/// for application unit tests.
/// </summary>
public static class RoleFixture
{
    /// <summary>
    /// Creates a default active role.
    /// </summary>
    public static Role Create(
        Guid? roleId = null,
        string name = "Administrator",
        bool isSystemRole = false,
        RoleScope? scope = null,
        DateTime? createdAtUtc = null)
    {
        return new Role(
            roleId ?? Guid.NewGuid(),
            name,
            isSystemRole,
            scope ?? new RoleScope("GLOBAL"),
            createdAtUtc ?? DateTime.UtcNow);
    }

    /// <summary>
    /// Creates an inactive role.
    /// </summary>
    public static Role CreateInactive(
        Guid? roleId = null,
        string name = "Administrator",
        bool isSystemRole = false,
        RoleScope? scope = null,
        DateTime? createdAtUtc = null)
    {
        var role = Create(
            roleId,
            name,
            isSystemRole,
            scope,
            createdAtUtc);

        role.Deactivate();

        return role;
    }

    /// <summary>
    /// Creates a system role.
    /// </summary>
    public static Role CreateSystemRole(
        Guid? roleId = null,
        string name = "System Administrator",
        RoleScope? scope = null,
        DateTime? createdAtUtc = null)
    {
        return Create(
            roleId,
            name,
            true,
            scope,
            createdAtUtc);
    }

    /// <summary>
    /// Creates an active role populated with permissions.
    /// </summary>
    public static Role CreateWithPermissions(
        IEnumerable<string> permissionIds,
        Guid? roleId = null,
        string name = "Administrator",
        bool isSystemRole = false,
        RoleScope? scope = null,
        DateTime? createdAtUtc = null)
    {
        ArgumentNullException.ThrowIfNull(permissionIds);

        var role = Create(
            roleId,
            name,
            isSystemRole,
            scope,
            createdAtUtc);

        foreach (var permissionId in permissionIds)
        {
            role.AddPermission(
                new PermissionId(permissionId));
        }

        return role;
    }
}