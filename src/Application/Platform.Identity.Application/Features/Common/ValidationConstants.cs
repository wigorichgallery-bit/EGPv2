// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Features/Common/ValidationConstants.cs
//
// STEP-7B
// LOCKED
// ===========================================
namespace Platform.Identity.Application.Features.Common;

/// <summary>
/// Centralized validation constants.
///
/// RESPONSIBILITY:
/// - Eliminate magic numbers.
/// - Provide a single source of truth for validation limits.
/// - Ensure consistency across validators.
///
/// SIDE EFFECTS:
/// - None.
///
/// COMPLEXITY:
/// - O(1)
/// </summary>
internal static class ValidationConstants
{
    /// <summary>
    /// Minimum username length.
    /// </summary>
    public const int UsernameMinLength = 3;

    /// <summary>
    /// Maximum username length.
    /// </summary>
    public const int UsernameMaxLength = 100;

    /// <summary>
    /// Minimum password length.
    /// </summary>
    public const int PasswordMinLength = 8;

    /// <summary>
    /// Maximum verification code length.
    /// </summary>
    public const int VerificationCodeMaxLength = 32;

    /// <summary>
    /// Maximum identity length.
    /// </summary>
    public const int MaximumIdentityLength = 256;

    /// <summary>
    /// Maximum password length.
    /// </summary>
    public const int MaximumPasswordLength = 256;
}