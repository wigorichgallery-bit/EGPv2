// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Projections/UserProjection.cs
// ===========================================
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Identity.Domain.Aggregates;

namespace Platform.Persistence.Projections;

/// <summary>
/// Provides projection methods for converting
/// <see cref="UserAccount"/> aggregates into
/// <see cref="UserDto"/> read models.
///
/// Responsibility:
/// - Convert domain aggregates into DTOs.
/// - Flatten domain value objects.
/// - Preserve application boundary.
/// - Centralize projection logic.
/// - Prevent mapping duplication.
///
/// Architectural Rules:
/// - Infrastructure layer only.
/// - Pure projection.
/// - No business logic.
/// - No persistence logic.
/// - No EF Core dependency.
/// - Stateless.
///
/// Projection Strategy:
/// - Aggregate → DTO.
/// - Value Object → Primitive.
///
/// Thread Safety:
/// - Thread-safe.
/// </summary>
public static class UserProjection
{
    /// <summary>
    /// Converts a
    /// <see cref="UserAccount"/>
    /// aggregate into a
    /// <see cref="UserDto"/>.
    ///
    /// Algorithm:
    /// 1. Validate aggregate.
    /// 2. Flatten value objects.
    /// 3. Construct immutable DTO.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="user">
    /// User aggregate.
    /// </param>
    /// <returns>
    /// Immutable user DTO.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="user"/>
    /// is null.
    /// </exception>
    public static UserDto ToDto(
        UserAccount user)
    {
        ArgumentNullException.ThrowIfNull(
            user);

        return new UserDto(
            user.Id,
            user.Username,
            user.Email.Value,
            user.PhoneNumber.Value,
            user.EmailVerified,
            user.PhoneVerified,
            user.Status,
            user.MFAEnabled,
            user.MFAMethod);
    }
}