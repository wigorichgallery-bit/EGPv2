// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Projections/RoleProjection.cs
// ===========================================
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Identity.Domain.Aggregates;

namespace Platform.Persistence.Projections;

/// <summary>
/// Maps <see cref="Role"/> aggregates
/// into <see cref="RoleDto"/> read models.
///
/// Responsibility:
/// - Convert domain aggregate into DTO.
/// - Centralize query mapping.
/// - Prevent mapping duplication.
///
/// Architectural Rules:
/// - Pure mapping.
/// - No EF Core.
/// - No infrastructure.
/// - No business logic.
/// - Stateless.
///
/// Thread Safety:
/// - Thread-safe.
/// </summary>
public static class RoleProjection
{
    /// <summary>
    // - Convert Value Objects into DTO primitives.
    // - Convert permission collection.
    // - Preserve domain encapsulation.
    /// </summary>
    /// <param name="role">
    /// Role aggregate.
    /// </param>
    /// <returns>
    /// Role DTO.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when role is null.
    /// </exception>
    public static RoleDto ToDto(
        Role role)
    {
        ArgumentNullException.ThrowIfNull(
            role);

        return new RoleDto(
            role.Id,
            role.Name,
            role.IsSystemRole,
            role.Scope.Value,
            role.IsActive,
            [.. role.PermissionIds.Select(
                x => x.Value)]);
    }
}