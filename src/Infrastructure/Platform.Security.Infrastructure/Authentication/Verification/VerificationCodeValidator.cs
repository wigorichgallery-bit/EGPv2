// ===========================================
// File Location :
// src/Infrastructure/Platform.Security.Infrastructure/
// Verification/VerificationCodeValidator.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Security;

namespace Platform.Security.Infrastructure.Verification;

/// <summary>
/// Temporary verification code validator.
///
/// Responsibility:
/// - Validate verification codes used by
///   identity workflows.
/// - Provide a buildable implementation
///   of IVerificationCodeValidator.
///
/// Architectural Rules:
/// - Infrastructure implementation.
/// - No persistence dependency.
/// - No external provider dependency.
/// - No security provider dependency.
///
/// Current Behavior:
/// - Always returns true.
/// - Intended only for early platform
///   bootstrap and integration testing.
///
/// Future Evolution:
/// - SMS OTP validation.
/// - Email verification validation.
/// - Authenticator application validation.
/// - Database-backed verification codes.
///
/// Change Policy:
/// - Replacement requires Change Request.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Stateless.
/// - Safe for concurrent usage.
///
/// Complexity:
/// O(1)
/// </summary>
public sealed class VerificationCodeValidator
    : IVerificationCodeValidator
{
    /// <summary>
    /// Validates a verification code.
    ///
    /// Current Behavior:
    /// - Always returns true.
    ///
    /// Future Behavior:
    /// - Validate against configured
    ///   verification provider.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="verificationCode">
    /// Verification code supplied by caller.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Always true.
    /// </returns>
    public Task<bool> ValidateAsync(
        Guid userId,
        string verificationCode,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            verificationCode);

        return Task.FromResult(true);
    }
}