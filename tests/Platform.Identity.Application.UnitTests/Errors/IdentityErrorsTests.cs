using FluentAssertions;
using Platform.Identity.Application.Errors;
using Platform.Identity.Domain.ErrorCodes;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Errors;

/// <summary>
/// Unit tests for <see cref="IdentityErrors"/>.
/// </summary>
public sealed class IdentityErrorsTests
{
    /// <summary>
    /// Gets all defined application errors.
    /// </summary>
    public static TheoryData<Error, string, string, ErrorType> ErrorCases =>
        new()
        {
            // Authentication
            { IdentityErrors.InvalidCredentials, IdentityDomainErrorCodes.InvalidCredentials, "Invalid credentials.", ErrorType.Unauthorized },
            { IdentityErrors.UserLocked, IdentityDomainErrorCodes.UserLocked, "User account is locked.", ErrorType.Forbidden },
            { IdentityErrors.UserDisabled, IdentityDomainErrorCodes.UserDisabled, "User account is disabled.", ErrorType.Forbidden },
            { IdentityErrors.AccountVerificationRequired, IdentityDomainErrorCodes.ContactNotVerified, "Account verification is required before sign in.", ErrorType.Forbidden },
            { IdentityErrors.PasswordResetRequired, IdentityDomainErrorCodes.PasswordResetRequired, "Password reset is required before sign in.", ErrorType.Forbidden },
            { IdentityErrors.AuthenticationChallengeRequired, IdentityDomainErrorCodes.AuthenticationChallengeRequired, "Authentication challenge is required.", ErrorType.Forbidden },

            // User
            { IdentityErrors.UserNotFound, IdentityDomainErrorCodes.UserNotFound, "User was not found.", ErrorType.NotFound },
            { IdentityErrors.UsernameAlreadyExists, IdentityDomainErrorCodes.UsernameAlreadyExists, "Username already exists.", ErrorType.Conflict },
            { IdentityErrors.EmailAlreadyExists, IdentityDomainErrorCodes.EmailAlreadyExists, "Email address already exists.", ErrorType.Conflict },
            { IdentityErrors.PhoneAlreadyExists, IdentityDomainErrorCodes.PhoneAlreadyExists, "Phone number already exists.", ErrorType.Conflict },

            // Contact Verification
            { IdentityErrors.EmailNotVerified, IdentityDomainErrorCodes.EmailNotVerified, "Email address has not been verified.", ErrorType.Forbidden },
            { IdentityErrors.PhoneNotVerified, IdentityDomainErrorCodes.PhoneNotVerified, "Phone number has not been verified.", ErrorType.Forbidden },
            { IdentityErrors.ContactNotVerified, IdentityDomainErrorCodes.ContactNotVerified, "At least one contact method must be verified.", ErrorType.Forbidden },

            // Password
            { IdentityErrors.InvalidPassword, IdentityDomainErrorCodes.InvalidPassword, "Invalid password.", ErrorType.Unauthorized },
            { IdentityErrors.PasswordReuse, IdentityDomainErrorCodes.PasswordReuse, "Password reuse is not allowed.", ErrorType.Forbidden },
            { IdentityErrors.PasswordChangeNotAllowed, IdentityDomainErrorCodes.PasswordChangeNotAllowed, "Password change is not allowed.", ErrorType.Forbidden },

            // MFA
            { IdentityErrors.MfaAlreadyEnabled, IdentityDomainErrorCodes.MfaAlreadyEnabled, "Multi-factor authentication is already enabled.", ErrorType.Forbidden },
            { IdentityErrors.MfaNotEnabled, IdentityDomainErrorCodes.MfaNotEnabled, "Multi-factor authentication is not enabled.", ErrorType.Forbidden },
            { IdentityErrors.MfaConfigurationInvalid, IdentityDomainErrorCodes.MfaConfigurationInvalid, "Multi-factor authentication configuration is invalid.", ErrorType.Forbidden },
            { IdentityErrors.TotpRequired, IdentityDomainErrorCodes.TotpRequired, "TOTP secret is required.", ErrorType.Forbidden },
            { IdentityErrors.TotpSecretNotConfigured, IdentityDomainErrorCodes.TotpSecretNotConfigured, "TOTP secret has not been configured.", ErrorType.Internal },

            // Challenge
            { IdentityErrors.InvalidChallengeExpiration, IdentityDomainErrorCodes.InvalidChallengeExpiration, "Authentication challenge expiration is invalid.", ErrorType.Validation },
            { IdentityErrors.ChallengeExpired, IdentityDomainErrorCodes.ChallengeExpired, "Authentication challenge has expired.", ErrorType.Forbidden },
            { IdentityErrors.ChallengeLocked, IdentityDomainErrorCodes.ChallengeLocked, "Authentication challenge is locked.", ErrorType.Forbidden },
            { IdentityErrors.ChallengeCancelled, IdentityDomainErrorCodes.ChallengeCancelled, "Authentication challenge has been cancelled.", ErrorType.Forbidden },
            { IdentityErrors.ChallengeCompleted, IdentityDomainErrorCodes.ChallengeCompleted, "Authentication challenge has already been completed.", ErrorType.Conflict },

            // Role
            { IdentityErrors.RoleNotFound, IdentityDomainErrorCodes.RoleNotFound, "Role was not found.", ErrorType.NotFound },
            { IdentityErrors.RoleInactive, IdentityDomainErrorCodes.RoleInactive, "Role is inactive.", ErrorType.Forbidden },
            { IdentityErrors.RoleAlreadyAssigned, IdentityDomainErrorCodes.RoleAlreadyAssigned, "Role is already assigned.", ErrorType.Conflict },
            { IdentityErrors.RoleNotAssigned, IdentityDomainErrorCodes.RoleNotAssigned, "Role assignment does not exist.", ErrorType.Forbidden },

            // Validation
            { IdentityErrors.ValidationError, IdentityDomainErrorCodes.ValidationError, "Validation failed.", ErrorType.Validation },
            { IdentityErrors.InvalidEmail, IdentityDomainErrorCodes.InvalidEmail, "Email format is invalid.", ErrorType.Validation },
            { IdentityErrors.InvalidPhone, IdentityDomainErrorCodes.InvalidPhone, "Phone number format is invalid.", ErrorType.Validation },
            { IdentityErrors.InvalidVerificationCode, IdentityDomainErrorCodes.InvalidVerificationCode, "Verification code is invalid.", ErrorType.Validation },

            // Aggregate
            { IdentityErrors.InvalidState, IdentityDomainErrorCodes.InvalidState, "The requested operation is not allowed in the current state.", ErrorType.Forbidden },

            // General
            { IdentityErrors.Conflict, IdentityDomainErrorCodes.Conflict, "A conflict occurred.", ErrorType.Conflict },
            { IdentityErrors.Unknown, IdentityDomainErrorCodes.Unknown, "An unexpected identity error occurred.", ErrorType.Internal }
        };

    /// <summary>
    /// Verifies every predefined application error exposes the expected values.
    /// </summary>
    [Theory]
    [MemberData(nameof(ErrorCases))]
    public void Error_Should_Expose_Expected_Metadata(
        Error error,
        string expectedCode,
        string expectedMessage,
        ErrorType expectedType)
    {
        // Assert
        error.Code.Should().Be(expectedCode);
        error.Message.Should().Be(expectedMessage);
        error.Type.Should().Be(expectedType);
    }
}