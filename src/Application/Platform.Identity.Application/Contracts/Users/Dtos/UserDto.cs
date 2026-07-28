// ===========================================
// File Location : src/Application/Platform.Identity.Application/Contracts/Users/Dtos/UserDto.cs
// ===========================================
using Platform.Identity.Domain.Enums;

namespace Platform.Identity.Application.Contracts.Users.Dtos;

/// <summary>
/// Represents a user read model.
///
/// Responsibility:
/// - Transfer user information outside the domain layer.
/// - Support query and administration scenarios.
/// - Prevent aggregate leakage.
///
/// Invariants:
/// - Immutable.
/// - Contains only data required by consumers.
/// - Does not expose domain behavior.
///
/// Side Effects:
/// - None.
///
/// Complexity:
/// O(1)
/// </summary>
/// <param name="UserId">Unique user identifier.</param>
/// <param name="Username">Unique username.</param>
/// <param name="Email">User email address.</param>
/// <param name="PhoneNumber">User phone number.</param>
/// <param name="EmailVerified">Email verification status.</param>
/// <param name="PhoneVerified">Phone verification status.</param>
/// <param name="Status">Current user lifecycle state.</param>
/// <param name="MfaEnabled">Indicates whether MFA is enabled.</param>
/// <param name="MfaMethod">Configured MFA method.</param>
public sealed record UserDto(
    Guid UserId,
    string Username,
    string Email,
    string PhoneNumber,
    bool EmailVerified,
    bool PhoneVerified,
    UserStatus Status,
    bool MfaEnabled,
    MFAMethod MfaMethod);