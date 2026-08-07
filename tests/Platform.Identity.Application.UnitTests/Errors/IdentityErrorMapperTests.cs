using FluentAssertions;
using Platform.Identity.Application.Errors;
using Platform.Identity.Domain.ErrorCodes;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Errors;

/// <summary>
/// Unit tests for <see cref="IdentityErrorMapper"/>.
/// </summary>
public sealed class IdentityErrorMapperTests
{
    /// <summary>
    /// Gets all supported mapping cases.
    /// </summary>
    public static TheoryData<string, Error> MapCases =>
        new()
        {
            { IdentityDomainErrorCodes.InvalidCredentials, IdentityErrors.InvalidCredentials },
            { IdentityDomainErrorCodes.PasswordResetRequired, IdentityErrors.PasswordResetRequired },
            { IdentityDomainErrorCodes.AuthenticationChallengeRequired, IdentityErrors.AuthenticationChallengeRequired },

            { IdentityDomainErrorCodes.UserNotFound, IdentityErrors.UserNotFound },
            { IdentityDomainErrorCodes.UserLocked, IdentityErrors.UserLocked },
            { IdentityDomainErrorCodes.UserDisabled, IdentityErrors.UserDisabled },
            { IdentityDomainErrorCodes.UsernameAlreadyExists, IdentityErrors.UsernameAlreadyExists },
            { IdentityDomainErrorCodes.EmailAlreadyExists, IdentityErrors.EmailAlreadyExists },
            { IdentityDomainErrorCodes.PhoneAlreadyExists, IdentityErrors.PhoneAlreadyExists },

            { IdentityDomainErrorCodes.EmailNotVerified, IdentityErrors.EmailNotVerified },
            { IdentityDomainErrorCodes.PhoneNotVerified, IdentityErrors.PhoneNotVerified },
            { IdentityDomainErrorCodes.ContactNotVerified, IdentityErrors.ContactNotVerified },

            { IdentityDomainErrorCodes.InvalidPassword, IdentityErrors.InvalidPassword },
            { IdentityDomainErrorCodes.PasswordReuse, IdentityErrors.PasswordReuse },
            { IdentityDomainErrorCodes.PasswordChangeNotAllowed, IdentityErrors.PasswordChangeNotAllowed },

            { IdentityDomainErrorCodes.MfaAlreadyEnabled, IdentityErrors.MfaAlreadyEnabled },
            { IdentityDomainErrorCodes.MfaNotEnabled, IdentityErrors.MfaNotEnabled },
            { IdentityDomainErrorCodes.MfaConfigurationInvalid, IdentityErrors.MfaConfigurationInvalid },
            { IdentityDomainErrorCodes.TotpRequired, IdentityErrors.TotpRequired },
            { IdentityDomainErrorCodes.TotpSecretNotConfigured, IdentityErrors.TotpSecretNotConfigured },

            { IdentityDomainErrorCodes.InvalidChallengeExpiration, IdentityErrors.InvalidChallengeExpiration },
            { IdentityDomainErrorCodes.ChallengeExpired, IdentityErrors.ChallengeExpired },
            { IdentityDomainErrorCodes.ChallengeLocked, IdentityErrors.ChallengeLocked },
            { IdentityDomainErrorCodes.ChallengeCancelled, IdentityErrors.ChallengeCancelled },
            { IdentityDomainErrorCodes.ChallengeCompleted, IdentityErrors.ChallengeCompleted },

            { IdentityDomainErrorCodes.RoleNotFound, IdentityErrors.RoleNotFound },
            { IdentityDomainErrorCodes.RoleInactive, IdentityErrors.RoleInactive },
            { IdentityDomainErrorCodes.RoleAlreadyAssigned, IdentityErrors.RoleAlreadyAssigned },
            { IdentityDomainErrorCodes.RoleNotAssigned, IdentityErrors.RoleNotAssigned },

            { IdentityDomainErrorCodes.InvalidState, IdentityErrors.InvalidState },
            { IdentityDomainErrorCodes.InvalidEmail, IdentityErrors.InvalidEmail },
            { IdentityDomainErrorCodes.InvalidPhone, IdentityErrors.InvalidPhone },
            { IdentityDomainErrorCodes.InvalidVerificationCode, IdentityErrors.InvalidVerificationCode }
        };

    /// <summary>
    /// Verifies known domain error codes are mapped correctly.
    /// </summary>
    [Theory]
    [MemberData(nameof(MapCases))]
    public void Map_Should_Return_Expected_Error(
        string errorCode,
        Error expected)
    {
        // Arrange
        var exception = new DomainException(errorCode, "Domain error");

        // Act
        var result = IdentityErrorMapper.Map(exception);

        // Assert
        result.Should().BeSameAs(expected);
    }

    /// <summary>
    /// Verifies unknown error codes are mapped to the Unknown error.
    /// </summary>
    [Fact]
    public void Map_Should_Return_Unknown_For_Unknown_Error_Code()
    {
        // Arrange
        var exception = new DomainException("UNKNOWN_ERROR_CODE", "Unknown");

        // Act
        var result = IdentityErrorMapper.Map(exception);

        // Assert
        result.Should().BeSameAs(IdentityErrors.Unknown);
    }

    /// <summary>
    /// Verifies a null exception throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [Fact]
    public void Map_Should_Throw_ArgumentNullException_When_Exception_Is_Null()
    {
        // Arrange
        DomainException? exception = null;

        // Act
        var action = () => IdentityErrorMapper.Map(exception!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}