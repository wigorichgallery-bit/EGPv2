// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Errors/IdentityErrorMapper.cs
// ===========================================

using Platform.Identity.Domain.ErrorCodes;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Errors;

/// <summary>
/// Maps <see cref="DomainException"/> instances raised by the
/// Identity domain into reusable application <see cref="Error"/>
/// objects.
///
/// Responsibility:
/// - Centralize DomainException translation.
/// - Eliminate magic string comparisons.
/// - Keep application use cases clean.
/// - Ensure consistent Result.Failure() responses.
///
/// Architectural Rules:
/// - Application layer only.
/// - Stateless.
/// - No infrastructure dependency.
/// - No business logic.
///
/// Complexity:
/// O(1)
/// </summary>
public static class IdentityErrorMapper
{
    /// <summary>
    /// Maps the specified domain exception into an application error.
    /// </summary>
    /// <param name="exception">
    /// Domain exception.
    /// </param>
    /// <returns>
    /// Corresponding application error.
    /// </returns>
    public static Error Map(
        DomainException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return exception.ErrorCode switch
        {
            // ============================================================
            // AUTHENTICATION
            // ============================================================

            IdentityDomainErrorCodes.InvalidCredentials
                => IdentityErrors.InvalidCredentials,

            IdentityDomainErrorCodes.PasswordResetRequired
                => IdentityErrors.PasswordResetRequired,

            IdentityDomainErrorCodes.AuthenticationChallengeRequired
                => IdentityErrors.AuthenticationChallengeRequired,

            // ============================================================
            // USER MANAGEMENT
            // ============================================================

            IdentityDomainErrorCodes.UserNotFound
                => IdentityErrors.UserNotFound,

            IdentityDomainErrorCodes.UserLocked
                => IdentityErrors.UserLocked,

            IdentityDomainErrorCodes.UserDisabled
                => IdentityErrors.UserDisabled,

            IdentityDomainErrorCodes.UsernameAlreadyExists
                => IdentityErrors.UsernameAlreadyExists,

            IdentityDomainErrorCodes.EmailAlreadyExists
                => IdentityErrors.EmailAlreadyExists,

            IdentityDomainErrorCodes.PhoneAlreadyExists
                => IdentityErrors.PhoneAlreadyExists,

            // ============================================================
            // CONTACT VERIFICATION
            // ============================================================

            IdentityDomainErrorCodes.EmailNotVerified
                => IdentityErrors.EmailNotVerified,

            IdentityDomainErrorCodes.PhoneNotVerified
                => IdentityErrors.PhoneNotVerified,

            IdentityDomainErrorCodes.ContactNotVerified
                => IdentityErrors.ContactNotVerified,

            // ============================================================
            // PASSWORD
            // ============================================================

            IdentityDomainErrorCodes.InvalidPassword
                => IdentityErrors.InvalidPassword,

            IdentityDomainErrorCodes.PasswordReuse
                => IdentityErrors.PasswordReuse,

            IdentityDomainErrorCodes.PasswordChangeNotAllowed
                => IdentityErrors.PasswordChangeNotAllowed,

            // ============================================================
            // MULTI-FACTOR AUTHENTICATION
            // ============================================================

            IdentityDomainErrorCodes.MfaAlreadyEnabled
                => IdentityErrors.MfaAlreadyEnabled,

            IdentityDomainErrorCodes.MfaNotEnabled
                => IdentityErrors.MfaNotEnabled,

            IdentityDomainErrorCodes.MfaConfigurationInvalid
                => IdentityErrors.MfaConfigurationInvalid,

            IdentityDomainErrorCodes.TotpRequired
                => IdentityErrors.TotpRequired,

            IdentityDomainErrorCodes.TotpSecretNotConfigured
                => IdentityErrors.TotpSecretNotConfigured,

            // ============================================================
            // AUTHENTICATION CHALLENGE
            // ============================================================

            IdentityDomainErrorCodes.InvalidChallengeExpiration
                => IdentityErrors.InvalidChallengeExpiration,

            IdentityDomainErrorCodes.ChallengeExpired
                => IdentityErrors.ChallengeExpired,

            IdentityDomainErrorCodes.ChallengeLocked
                => IdentityErrors.ChallengeLocked,

            IdentityDomainErrorCodes.ChallengeCancelled
                => IdentityErrors.ChallengeCancelled,

            IdentityDomainErrorCodes.ChallengeCompleted
                => IdentityErrors.ChallengeCompleted,

            // ============================================================
            // ROLE
            // ============================================================

            IdentityDomainErrorCodes.RoleNotFound
                => IdentityErrors.RoleNotFound,

            IdentityDomainErrorCodes.RoleInactive
                => IdentityErrors.RoleInactive,

            IdentityDomainErrorCodes.RoleAlreadyAssigned
                => IdentityErrors.RoleAlreadyAssigned,

            IdentityDomainErrorCodes.RoleNotAssigned
                => IdentityErrors.RoleNotAssigned,

            // ============================================================
            // VALIDATION
            // ============================================================

            IdentityDomainErrorCodes.InvalidState
                => IdentityErrors.InvalidState,

            IdentityDomainErrorCodes.InvalidEmail
                => IdentityErrors.InvalidEmail,

            IdentityDomainErrorCodes.InvalidPhone
                => IdentityErrors.InvalidPhone,

            IdentityDomainErrorCodes.InvalidVerificationCode
                => IdentityErrors.InvalidVerificationCode,

            // ============================================================
            // DEFAULT
            // ============================================================

            _ => IdentityErrors.Unknown
        };
    }
}