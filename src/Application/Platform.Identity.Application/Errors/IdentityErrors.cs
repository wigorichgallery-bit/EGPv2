// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Errors/IdentityErrors.cs
// ===========================================

using Platform.Identity.Domain.ErrorCodes;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Errors;

/// <summary>
/// Centralized application error catalog for the Identity bounded context.
///
/// Responsibility:
/// - Maps domain error codes to reusable application errors.
/// - Eliminates magic strings.
/// - Standardizes Result.Failure responses.
///
/// Rules:
/// - Domain is the single source of truth for error codes.
/// - Application owns user-facing messages.
/// - No business logic.
/// </summary>
public static class IdentityErrors
{
    // ============================================================
    // AUTHENTICATION
    // ============================================================

    public static readonly Error InvalidCredentials =
        new(
            IdentityDomainErrorCodes.InvalidCredentials,
            "Invalid credentials.",
            ErrorType.Unauthorized);

    public static readonly Error UserLocked =
        new(
            IdentityDomainErrorCodes.UserLocked,
            "User account is locked.",
            ErrorType.Forbidden);

    public static readonly Error UserDisabled =
        new(
            IdentityDomainErrorCodes.UserDisabled,
            "User account is disabled.",
            ErrorType.Forbidden);

    public static readonly Error AccountVerificationRequired =
        new(
            IdentityDomainErrorCodes.ContactNotVerified,
            "Account verification is required before sign in.",
            ErrorType.Forbidden);

    public static readonly Error PasswordResetRequired =
        new(
            IdentityDomainErrorCodes.PasswordResetRequired,
            "Password reset is required before sign in.",
            ErrorType.Forbidden);

    public static readonly Error AuthenticationChallengeRequired =
        new(
            IdentityDomainErrorCodes.AuthenticationChallengeRequired,
            "Authentication challenge is required.",
            ErrorType.Forbidden);

    // ============================================================
    // USER MANAGEMENT
    // ============================================================

    public static readonly Error UserNotFound =
        new(
            IdentityDomainErrorCodes.UserNotFound,
            "User was not found.",
            ErrorType.NotFound);

    public static readonly Error UsernameAlreadyExists =
        new(
            IdentityDomainErrorCodes.UsernameAlreadyExists,
            "Username already exists.",
            ErrorType.Conflict);

    public static readonly Error EmailAlreadyExists =
        new(
            IdentityDomainErrorCodes.EmailAlreadyExists,
            "Email address already exists.",
            ErrorType.Conflict);

    public static readonly Error PhoneAlreadyExists =
        new(
            IdentityDomainErrorCodes.PhoneAlreadyExists,
            "Phone number already exists.",
            ErrorType.Conflict);

    // ============================================================
    // CONTACT VERIFICATION
    // ============================================================

    public static readonly Error EmailNotVerified =
        new(
            IdentityDomainErrorCodes.EmailNotVerified,
            "Email address has not been verified.",
            ErrorType.Forbidden);

    public static readonly Error PhoneNotVerified =
        new(
            IdentityDomainErrorCodes.PhoneNotVerified,
            "Phone number has not been verified.",
            ErrorType.Forbidden);

    public static readonly Error ContactNotVerified =
        new(
            IdentityDomainErrorCodes.ContactNotVerified,
            "At least one contact method must be verified.",
            ErrorType.Forbidden);

    // ============================================================
    // PASSWORD
    // ============================================================

    public static readonly Error InvalidPassword =
        new(
            IdentityDomainErrorCodes.InvalidPassword,
            "Invalid password.",
            ErrorType.Unauthorized);

    public static readonly Error PasswordReuse =
        new(
            IdentityDomainErrorCodes.PasswordReuse,
            "Password reuse is not allowed.",
            ErrorType.Forbidden);

    public static readonly Error PasswordChangeNotAllowed =
        new(
            IdentityDomainErrorCodes.PasswordChangeNotAllowed,
            "Password change is not allowed.",
            ErrorType.Forbidden);

    // ============================================================
    // MULTI FACTOR AUTHENTICATION
    // ============================================================

    public static readonly Error MfaAlreadyEnabled =
        new(
            IdentityDomainErrorCodes.MfaAlreadyEnabled,
            "Multi-factor authentication is already enabled.",
            ErrorType.Forbidden);

    public static readonly Error MfaNotEnabled =
        new(
            IdentityDomainErrorCodes.MfaNotEnabled,
            "Multi-factor authentication is not enabled.",
            ErrorType.Forbidden);

    public static readonly Error MfaConfigurationInvalid =
        new(
            IdentityDomainErrorCodes.MfaConfigurationInvalid,
            "Multi-factor authentication configuration is invalid.",
            ErrorType.Forbidden);

    public static readonly Error TotpRequired =
        new(
            IdentityDomainErrorCodes.TotpRequired,
            "TOTP secret is required.",
            ErrorType.Forbidden);

    public static readonly Error TotpSecretNotConfigured =
        new(
            IdentityDomainErrorCodes.TotpSecretNotConfigured,
            "TOTP secret has not been configured.",
            ErrorType.Internal);

    // ============================================================
    // AUTHENTICATION CHALLENGE
    // ============================================================

    public static readonly Error InvalidChallengeExpiration =
        new(
            IdentityDomainErrorCodes.InvalidChallengeExpiration,
            "Authentication challenge expiration is invalid.",
            ErrorType.Validation);

    public static readonly Error ChallengeExpired =
        new(
            IdentityDomainErrorCodes.ChallengeExpired,
            "Authentication challenge has expired.",
            ErrorType.Forbidden);

    public static readonly Error ChallengeLocked =
        new(
            IdentityDomainErrorCodes.ChallengeLocked,
            "Authentication challenge is locked.",
            ErrorType.Forbidden);

    public static readonly Error ChallengeCancelled =
        new(
            IdentityDomainErrorCodes.ChallengeCancelled,
            "Authentication challenge has been cancelled.",
            ErrorType.Forbidden);

    public static readonly Error ChallengeCompleted =
        new(
            IdentityDomainErrorCodes.ChallengeCompleted,
            "Authentication challenge has already been completed.",
            ErrorType.Conflict);

    // ============================================================
    // ROLE
    // ============================================================

    public static readonly Error RoleNotFound =
        new(
            IdentityDomainErrorCodes.RoleNotFound,
            "Role was not found.",
            ErrorType.NotFound);

    public static readonly Error RoleInactive =
        new(
            IdentityDomainErrorCodes.RoleInactive,
            "Role is inactive.",
            ErrorType.Forbidden);

    public static readonly Error RoleAlreadyAssigned =
        new(
            IdentityDomainErrorCodes.RoleAlreadyAssigned,
            "Role is already assigned.",
            ErrorType.Conflict);

    public static readonly Error RoleNotAssigned =
        new(
            IdentityDomainErrorCodes.RoleNotAssigned,
            "Role assignment does not exist.",
            ErrorType.Forbidden);

    // ============================================================
    // VALIDATION
    // ============================================================

    public static readonly Error ValidationError =
        new(
            IdentityDomainErrorCodes.ValidationError,
            "Validation failed.",
            ErrorType.Validation);

    public static readonly Error InvalidEmail =
        new(
            IdentityDomainErrorCodes.InvalidEmail,
            "Email format is invalid.",
            ErrorType.Validation);

    public static readonly Error InvalidPhone =
        new(
            IdentityDomainErrorCodes.InvalidPhone,
            "Phone number format is invalid.",
            ErrorType.Validation);

    public static readonly Error InvalidVerificationCode =
        new(
            IdentityDomainErrorCodes.InvalidVerificationCode,
            "Verification code is invalid.",
            ErrorType.Validation);

    // ============================================================
    // AGGREGATE
    // ============================================================

    public static readonly Error InvalidState =
        new(
            IdentityDomainErrorCodes.InvalidState,
            "The requested operation is not allowed in the current state.",
            ErrorType.Forbidden);

    // ============================================================
    // GENERAL
    // ============================================================

    public static readonly Error Conflict =
        new(
            IdentityDomainErrorCodes.Conflict,
            "A conflict occurred.",
            ErrorType.Conflict);

    public static readonly Error Unknown =
        new(
            IdentityDomainErrorCodes.Unknown,
            "An unexpected identity error occurred.",
            ErrorType.Internal);
}