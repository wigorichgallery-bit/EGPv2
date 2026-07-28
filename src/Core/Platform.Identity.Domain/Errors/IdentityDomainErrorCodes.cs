// // ===========================================
// // File Location :
// // src/Core/Platform.Identity.Domain/
// // ErrorCodes/IdentityDomainErrorCodes.cs
// // ===========================================

// namespace Platform.Identity.Domain.ErrorCodes;

// /// <summary>
// /// Defines the canonical domain error codes for the Identity bounded context.
// ///
// /// Responsibility:
// /// - Centralize all domain error identifiers.
// /// - Eliminate magic string literals.
// /// - Provide compile-time safety.
// /// - Support consistent DomainException creation.
// /// - Enable stable mapping to application errors.
// ///
// /// Architectural Rules:
// /// - Domain layer only.
// /// - Contains constants only.
// /// - No dependencies.
// /// - No business logic.
// ///
// /// Side Effects:
// /// - None.
// ///
// /// Thread Safety:
// /// - Immutable.
// /// </summary>
// public static class IdentityDomainErrorCodes
// {
//     // ============================================================
//     // USER
//     // ============================================================

//     /// <summary>
//     /// User was not found.
//     /// </summary>
//     public const string UserNotFound =
//         "IDENTITY.USER_NOT_FOUND";

//     /// <summary>
//     /// User account is locked.
//     /// </summary>
//     public const string UserLocked =
//         "IDENTITY.USER_LOCKED";

//     /// <summary>
//     /// User account is disabled.
//     /// </summary>
//     public const string UserDisabled =
//         "IDENTITY.USER_DISABLED";

//     // ============================================================
//     // VALIDATION
//     // ============================================================

//     /// <summary>
//     /// Invalid state transition.
//     /// </summary>
//     public const string InvalidState =
//         "IDENTITY.INVALID_STATE";

//     /// <summary>
//     /// Invalid UTC timestamp.
//     /// </summary>
//     public const string InvalidUtc =
//         "IDENTITY.INVALID_UTC";

//     /// <summary>
//     /// Invalid email format.
//     /// </summary>
//     public const string InvalidEmail =
//         "IDENTITY.INVALID_EMAIL";

//     /// <summary>
//     /// Invalid phone number format.
//     /// </summary>
//     public const string InvalidPhone =
//         "IDENTITY.INVALID_PHONE";

//     // ============================================================
//     // PASSWORD
//     // ============================================================

//     /// <summary>
//     /// Password reuse is not allowed.
//     /// </summary>
//     public const string PasswordReuse =
//         "IDENTITY.PASSWORD_REUSE";

//     // ============================================================
//     // EMAIL / PHONE
//     // ============================================================

//     /// <summary>
//     /// Email must be verified.
//     /// </summary>
//     public const string EmailNotVerified =
//         "IDENTITY.EMAIL_NOT_VERIFIED";

//     /// <summary>
//     /// Phone must be verified.
//     /// </summary>
//     public const string PhoneNotVerified =
//         "IDENTITY.PHONE_NOT_VERIFIED";

//     // ============================================================
//     // MFA
//     // ============================================================

//     /// <summary>
//     /// TOTP secret is required.
//     /// </summary>
//     public const string TotpRequired =
//         "IDENTITY.TOTP_REQUIRED";

//     /// <summary>
//     /// At least one verified contact is required.
//     /// </summary>
//     public const string ContactNotVerified =
//         "IDENTITY.CONTACT_NOT_VERIFIED";

//     // ============================================================
//     // ROLE
//     // ============================================================

//     /// <summary>
//     /// Role already assigned.
//     /// </summary>
//     public const string RoleAlreadyAssigned =
//         "IDENTITY.ROLE_ALREADY_ASSIGNED";

//     /// <summary>
//     /// Role is not assigned.
//     /// </summary>
//     public const string RoleNotAssigned =
//         "IDENTITY.ROLE_NOT_ASSIGNED";

//     // ============================================================
//     // AUTHENTICATION CHALLENGE
//     // ============================================================

//     /// <summary>
//     /// Authentication challenge expiration is invalid.
//     /// </summary>
//     public const string InvalidChallengeExpiration =
//         "IDENTITY.INVALID_CHALLENGE_EXPIRATION";
// }

// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// ErrorCodes/IdentityDomainErrorCodes.cs
// ===========================================

namespace Platform.Identity.Domain.ErrorCodes;

public static class IdentityDomainErrorCodes
{
    // ============================================================
    // GENERAL
    // ============================================================

    public const string ValidationError =
        "IDENTITY.VALIDATION_ERROR";

    public const string Conflict =
        "IDENTITY.CONFLICT";

    public const string Unknown =
        "IDENTITY.UNKNOWN";
        
    // ============================================================
    // AUTHENTICATION
    // ============================================================

    public const string InvalidCredentials =
        "IDENTITY.INVALID_CREDENTIALS";

    public const string PasswordResetRequired =
    "IDENTITY.PASSWORD_RESET_REQUIRED";

    public const string AuthenticationChallengeRequired =
        "IDENTITY.AUTHENTICATION_CHALLENGE_REQUIRED";

        
    // ============================================================
    // USER
    // ============================================================

    public const string UserNotFound =
        "IDENTITY.USER_NOT_FOUND";

    public const string UserLocked =
        "IDENTITY.USER_LOCKED";

    public const string UserDisabled =
        "IDENTITY.USER_DISABLED";

    public const string UsernameAlreadyExists =
        "IDENTITY.USERNAME_ALREADY_EXISTS";

    public const string EmailAlreadyExists =
        "IDENTITY.EMAIL_ALREADY_EXISTS";

    public const string PhoneAlreadyExists =
        "IDENTITY.PHONE_ALREADY_EXISTS";

    // ============================================================
    // VALIDATION
    // ============================================================

    public const string InvalidState =
        "IDENTITY.INVALID_STATE";

    public const string InvalidUtc =
        "IDENTITY.INVALID_UTC";

    public const string InvalidEmail =
        "IDENTITY.INVALID_EMAIL";

    public const string InvalidPhone =
        "IDENTITY.INVALID_PHONE";

    public const string InvalidVerificationCode =
        "IDENTITY.INVALID_VERIFICATION_CODE";

    // ============================================================
    // PASSWORD
    // ============================================================

    public const string InvalidPassword =
        "IDENTITY.INVALID_PASSWORD";

    public const string PasswordReuse =
        "IDENTITY.PASSWORD_REUSE";

    public const string PasswordChangeNotAllowed =
        "IDENTITY.PASSWORD_CHANGE_NOT_ALLOWED";

    // ============================================================
    // EMAIL / PHONE
    // ============================================================

    public const string EmailNotVerified =
        "IDENTITY.EMAIL_NOT_VERIFIED";

    public const string PhoneNotVerified =
        "IDENTITY.PHONE_NOT_VERIFIED";

    public const string ContactNotVerified =
        "IDENTITY.CONTACT_NOT_VERIFIED";

    // ============================================================
    // MULTI-FACTOR AUTHENTICATION (MFA)
    // ============================================================

    public const string MfaAlreadyEnabled =
        "IDENTITY.MFA_ALREADY_ENABLED";

    public const string MfaNotEnabled =
        "IDENTITY.MFA_NOT_ENABLED";

    public const string MfaConfigurationInvalid =
        "IDENTITY.MFA_CONFIGURATION_INVALID";

    public const string TotpRequired =
        "IDENTITY.TOTP_REQUIRED";

    public const string TotpSecretNotConfigured =
        "IDENTITY.TOTP_SECRET_NOT_CONFIGURED";

    // ============================================================
    // ROLE
    // ============================================================

    public const string RoleNotFound =
        "IDENTITY.ROLE_NOT_FOUND";

    public const string RoleInactive =
        "IDENTITY.ROLE_INACTIVE";

    public const string RoleAlreadyAssigned =
        "IDENTITY.ROLE_ALREADY_ASSIGNED";

    public const string RoleNotAssigned =
        "IDENTITY.ROLE_NOT_ASSIGNED";

    // ============================================================
    // AUTHENTICATION CHALLENGE
    // ============================================================

    public const string InvalidChallengeExpiration =
        "IDENTITY.INVALID_CHALLENGE_EXPIRATION";

    public const string ChallengeLocked =
        "IDENTITY.CHALLENGE_LOCKED";

    public const string ChallengeExpired =
        "IDENTITY.CHALLENGE_EXPIRED";

    public const string ChallengeCancelled =
        "IDENTITY.CHALLENGE_CANCELLED";

    public const string ChallengeCompleted =
        "IDENTITY.CHALLENGE_COMPLETED";
}