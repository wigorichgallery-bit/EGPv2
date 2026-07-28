// // ===========================================
// // File Location : src/Application/Platform.Identity.Application/Errors/IdentityErrorMapper.cs
// // ===========================================
// using Platform.SharedKernel.Exceptions;
// using Platform.SharedKernel.Results;

// namespace Platform.Identity.Application.Errors;

// /// <summary>
// /// Maps domain exception codes to application errors.
// ///
// /// Responsibility:
// /// - Eliminate magic string comparisons.
// /// - Centralize DomainException translation.
// /// - Keep UseCases clean.
// ///
// /// Architectural Rules:
// /// - Application layer only.
// /// - No infrastructure dependency.
// /// - No domain mutation.
// ///
// /// Side Effects:
// /// - None.
// ///
// /// Complexity:
// /// O(1)
// /// </summary>
// public static class IdentityErrorMapper
// {
//     /// <summary>
//     /// Converts a domain exception into a Result error.
//     /// </summary>
//     /// <param name="exception">
//     /// Domain exception.
//     /// </param>
//     /// <returns>
//     /// Matching application error.
//     /// </returns>
//     public static Error Map(DomainException exception)
//     {
//         ArgumentNullException.ThrowIfNull(exception);

//         return exception.ErrorCode switch
//         {
//             "IDENTITY.USER_NOT_FOUND"
//                 => IdentityErrors.UserNotFound,

//             "IDENTITY.INVALID_VERIFICATION_CODE"
//                 => IdentityErrors.InvalidVerificationCode,

//             "IDENTITY.USER_LOCKED"
//                 => IdentityErrors.UserLocked,

//             "IDENTITY.USER_DISABLED"
//                 => IdentityErrors.UserDisabled,

//             "IDENTITY.USERNAME_ALREADY_EXISTS"
//                 => IdentityErrors.UsernameAlreadyExists,

//             "IDENTITY.EMAIL_ALREADY_EXISTS"
//                 => IdentityErrors.EmailAlreadyExists,

//             "IDENTITY.EMAIL_NOT_VERIFIED"
//                 => IdentityErrors.EmailNotVerified,

//             "IDENTITY.PHONE_NOT_VERIFIED"
//                 => IdentityErrors.PhoneNotVerified,

//             "IDENTITY.INVALID_PASSWORD"
//                 => IdentityErrors.InvalidPassword,

//             "IDENTITY.PASSWORD_REUSE"
//                 => IdentityErrors.PasswordReuse,

//             "IDENTITY.PASSWORD_CHANGE_NOT_ALLOWED"
//                 => IdentityErrors.PasswordChangeNotAllowed,

//             "IDENTITY.MFA_ALREADY_ENABLED"
//                 => IdentityErrors.MfaAlreadyEnabled,

//             "IDENTITY.MFA_NOT_ENABLED"
//                 => IdentityErrors.MfaNotEnabled,

//             "IDENTITY.MFA_CONFIGURATION_INVALID"
//                 => IdentityErrors.MfaConfigurationInvalid,

//             "IDENTITY.TOTP_SECRET_NOT_CONFIGURED"
//                 => IdentityErrors.TotpSecretNotConfigured,

//             "IDENTITY.TOTP_REQUIRED"
//                 => IdentityErrors.TotpRequired,

//             "IDENTITY.CONTACT_NOT_VERIFIED"
//                 => IdentityErrors.ContactNotVerified,

//             "IDENTITY.INVALID_EMAIL"
//                 => IdentityErrors.InvalidEmail,

//             "IDENTITY.INVALID_PHONE"
//                 => IdentityErrors.InvalidPhone,

//             "IDENTITY.INVALID_STATE"
//                 => IdentityErrors.InvalidState,

//             "IDENTITY.ROLE_NOT_FOUND"
//                 => IdentityErrors.RoleNotFound,

//             "IDENTITY.ROLE_INACTIVE"
//                 => IdentityErrors.RoleInactive,

//             "IDENTITY.ROLE_ALREADY_ASSIGNED"
//                 => IdentityErrors.RoleAlreadyAssigned,

//             "IDENTITY.ROLE_NOT_ASSIGNED"
//                 => IdentityErrors.RoleNotAssigned,

//             // _ => new Error(
//             //     exception.ErrorCode,
//             //     exception.Message)
//             _ => IdentityErrors.Unknown
//         };
//     }
// }

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
/// Maps identity domain exceptions to application errors.
/// </summary>
public static class IdentityErrorMapper
{
    /// <summary>
    /// Maps the specified domain exception to an application error.
    /// </summary>
    /// <param name="exception">
    /// The domain exception.
    /// </param>
    /// <returns>
    /// The mapped application error.
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

            // ============================================================
            // USER
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
            // PASSWORD
            // ============================================================

            IdentityDomainErrorCodes.InvalidPassword
                => IdentityErrors.InvalidPassword,

            IdentityDomainErrorCodes.PasswordReuse
                => IdentityErrors.PasswordReuse,

            IdentityDomainErrorCodes.PasswordChangeNotAllowed
                => IdentityErrors.PasswordChangeNotAllowed,

            // ============================================================
            // EMAIL / PHONE
            // ============================================================

            IdentityDomainErrorCodes.EmailNotVerified
                => IdentityErrors.EmailNotVerified,

            IdentityDomainErrorCodes.PhoneNotVerified
                => IdentityErrors.PhoneNotVerified,

            IdentityDomainErrorCodes.ContactNotVerified
                => IdentityErrors.ContactNotVerified,

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
            // AUTHENTICATION CHALLENGE
            // ============================================================

            IdentityDomainErrorCodes.InvalidChallengeExpiration
                => IdentityErrors.InvalidState,

            // ============================================================
            // DEFAULT
            // ============================================================

            _ => IdentityErrors.Unknown
        };
    }
}