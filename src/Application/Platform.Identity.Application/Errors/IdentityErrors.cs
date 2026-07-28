// // ===========================================
// // File Location : src/Application/Platform.Identity.Application/Errors/IdentityErrors.cs
// // ===========================================
// using Platform.SharedKernel.Results;

// namespace Platform.Identity.Application.Errors;

// /// <summary>
// /// Centralized identity application error catalog.
// ///
// /// Responsibility:
// /// - Provide strongly typed Error instances.
// /// - Eliminate magic string error codes.
// /// - Standardize Result.Failure responses.
// /// - Ensure consistent error handling across all Identity use cases.
// ///
// /// Invariants:
// /// - Error codes are immutable.
// /// - Error codes follow IDENTITY.* convention.
// /// - Error instances are shared and reusable.
// ///
// /// Side Effects:
// /// - None.
// ///
// /// Algorithm:
// /// 1. Use predefined Error instances.
// /// 2. Return through Result.Failure().
// /// 3. Prevent duplicated error code declarations.
// ///
// /// Complexity:
// /// O(1)
// /// </summary>
// public static class IdentityErrors
// {
//     /// <summary>
//     /// User was not found.
//     /// </summary>
//     public static readonly Error UserNotFound =
//         new("IDENTITY.USER_NOT_FOUND", "User was not found.", ErrorType.NotFound);

//     /// <summary>
//     /// User account is locked.
//     /// </summary>
//     public static readonly Error UserLocked =
//         new("IDENTITY.USER_LOCKED", "User account is locked.", ErrorType.Forbidden );

//     /// <summary>
//     /// User account is disabled.
//     /// </summary>
//     public static readonly Error UserDisabled =
//         new("IDENTITY.USER_DISABLED", "User account is disabled.", ErrorType.Forbidden);

//     /// <summary>
//     /// Username already exists.
//     /// </summary>
//     public static readonly Error UsernameAlreadyExists =
//         new("IDENTITY.USERNAME_ALREADY_EXISTS", "Username already exists.", ErrorType.Conflict);

//     /// <summary>
//     /// Email address already exists.
//     /// </summary>
//     public static readonly Error EmailAlreadyExists =
//         new("IDENTITY.EMAIL_ALREADY_EXISTS", "Email address already exists.", ErrorType.Conflict);

//     /// <summary>
//     /// Email address has not been verified.
//     /// </summary>
//     public static readonly Error EmailNotVerified =
//         new("IDENTITY.EMAIL_NOT_VERIFIED", "Email address has not been verified.", ErrorType.Forbidden);

//     /// <summary>
//     /// Phone number has not been verified.
//     /// </summary>
//     public static readonly Error PhoneNotVerified =
//         new("IDENTITY.PHONE_NOT_VERIFIED", "Phone number has not been verified.", ErrorType.Forbidden);

//     /// <summary>
//     /// Invalid password supplied.
//     /// </summary>
//     public static readonly Error InvalidPassword =
//         new("IDENTITY.INVALID_PASSWORD", "Invalid password.", ErrorType.Unauthorized);

//     /// <summary>
//     /// Password reuse detected.
//     /// </summary>
//     public static readonly Error PasswordReuse =
//         new("IDENTITY.PASSWORD_REUSE", "Password reuse is not allowed.", ErrorType.Forbidden);

//     /// <summary>
//     /// Password change is not allowed.
//     /// </summary>
//     public static readonly Error PasswordChangeNotAllowed =
//         new("IDENTITY.PASSWORD_CHANGE_NOT_ALLOWED", "Password change is not allowed.", ErrorType.Forbidden);

//     /// <summary>
//     /// MFA is already enabled.
//     /// </summary>
//     public static readonly Error MfaAlreadyEnabled =
//         new("IDENTITY.MFA_ALREADY_ENABLED", "Multi-factor authentication is already enabled.", ErrorType.Forbidden);

//     /// <summary>
//     /// MFA is not enabled.
//     /// </summary>
//     public static readonly Error MfaNotEnabled =
//         new("IDENTITY.MFA_NOT_ENABLED", "Multi-factor authentication is not enabled.", ErrorType.Forbidden);

//     /// <summary>
//     /// MFA configuration is invalid.
//     /// </summary>
//     public static readonly Error MfaConfigurationInvalid =
//         new("IDENTITY.MFA_CONFIGURATION_INVALID", "Multi-factor authentication configuration is invalid.", ErrorType.Forbidden);

//     /// <summary>
//     /// TOTP secret has not been configured.
//     /// </summary>
//     public static readonly Error TotpSecretNotConfigured =
//         new("IDENTITY.TOTP_SECRET_NOT_CONFIGURED", "TOTP secret has not been configured.", ErrorType.Internal);

//     /// <summary>
//     /// Role was not found.
//     /// </summary>
//     public static readonly Error RoleNotFound =
//         new("IDENTITY.ROLE_NOT_FOUND", "Role was not found.", ErrorType.NotFound);

//     /// <summary>
//     /// Role is inactive.
//     /// </summary>
//     public static readonly Error RoleInactive =
//         new("IDENTITY.ROLE_INACTIVE", "Role is inactive.", ErrorType.Forbidden);

//     /// <summary>
//     /// Role is already assigned.
//     /// </summary>
//     public static readonly Error RoleAlreadyAssigned =
//         new("IDENTITY.ROLE_ALREADY_ASSIGNED", "Role is already assigned.", ErrorType.Conflict);

//     /// <summary>
//     /// Role assignment does not exist.
//     /// </summary>
//     public static readonly Error RoleNotAssigned =
//         new("IDENTITY.ROLE_NOT_ASSIGNED", "Role assignment does not exist.", ErrorType.Forbidden);

//     /// <summary>
//     /// Invalid credentials supplied.
//     /// </summary>
//     public static readonly Error InvalidCredentials =
//         new("IDENTITY.INVALID_CREDENTIALS", "Invalid credentials.", ErrorType.Unauthorized);

//     /// <summary>
//     /// Account is locked.
//     /// </summary>
//     public static readonly Error AccountLocked =
//         new("IDENTITY.ACCOUNT_LOCKED", "Account is locked.", ErrorType.Forbidden);

//     /// <summary>
//     /// Conflict detected.
//     /// </summary>
//     public static readonly Error Conflict =
//         new("IDENTITY.CONFLICT", "A conflict occurred.", ErrorType.Conflict);

//     /// <summary>
//     /// Validation failed.
//     /// </summary>
//     public static readonly Error ValidationError =
//         new("IDENTITY.VALIDATION_ERROR", "Validation failed.", ErrorType.Validation);

//     /// <summary>
//     /// Email format is invalid.
//     /// </summary>
//     public static readonly Error InvalidEmail =
//         new("IDENTITY.INVALID_EMAIL", "Email format is invalid.", ErrorType.Validation);

//     /// <summary>
//     /// Phone number format is invalid.
//     /// </summary>
//     public static readonly Error InvalidPhone =
//         new("IDENTITY.INVALID_PHONE", "Phone number format is invalid.", ErrorType.Validation);

//     /// <summary>
//     /// Aggregate state transition is invalid.
//     /// </summary>
//     public static readonly Error InvalidState =
//         new("IDENTITY.INVALID_STATE", "The requested operation is not allowed in the current state.", ErrorType.Forbidden);

//     /// <summary>
//     /// TOTP secret must be configured before enabling TOTP MFA.
//     /// </summary>
//     public static readonly Error TotpRequired =
//         new("IDENTITY.TOTP_REQUIRED", "TOTP secret is required.", ErrorType.Forbidden);

//     /// <summary>
//     /// At least one verified contact method is required.
//     /// </summary>
//     public static readonly Error ContactNotVerified =
//         new("IDENTITY.CONTACT_NOT_VERIFIED", "At least one contact method must be verified.", ErrorType.Forbidden);   

//     /// <summary>
//     /// Phone number already exists.
//     /// </summary>
//     public static readonly Error PhoneAlreadyExists =
//         new(
//             "IDENTITY.PHONE_ALREADY_EXISTS",
//             "Phone number already exists.", ErrorType.Conflict);

//     /// <summary>
//     /// Unexpected identity error.
//     /// </summary>
//     public static readonly Error Unknown =
//         new(
//             "IDENTITY.UNKNOWN",
//             "An unexpected identity error occurred.", ErrorType.Internal);

//     /// <summary>
//     /// Verification code is invalid.
//     /// </summary>
//     public static readonly Error InvalidVerificationCode =
//         new(
//             "IDENTITY.INVALID_VERIFICATION_CODE",
//             "Verification code is invalid.", ErrorType.Validation);
// }

// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Errors/IdentityErrors.cs
// ===========================================

using Platform.Identity.Domain.ErrorCodes;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Errors;

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

    public static readonly Error AccountLocked =
        new(
            IdentityDomainErrorCodes.UserLocked,
            "Account is locked.",
            ErrorType.Forbidden);

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

    public static readonly Error TotpRequired =
        new(
            IdentityDomainErrorCodes.TotpRequired,
            "TOTP secret is required.",
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

    public static readonly Error TotpSecretNotConfigured =
        new(
            IdentityDomainErrorCodes.TotpSecretNotConfigured,
            "TOTP secret has not been configured.",
            ErrorType.Internal);

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
            "IDENTITY.VALIDATION_ERROR",
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
            "IDENTITY.CONFLICT",
            "A conflict occurred.",
            ErrorType.Conflict);

    public static readonly Error Unknown =
        new(
            "IDENTITY.UNKNOWN",
            "An unexpected identity error occurred.",
            ErrorType.Internal);
}