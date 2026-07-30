// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// ErrorCodes/IdentityDomainErrorCodesTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.ErrorCodes;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.ErrorCodes;

/// <summary>
/// Contains unit tests for
/// <see cref="IdentityDomainErrorCodes"/>.
/// </summary>
public sealed class IdentityDomainErrorCodesTests
{
    #region Constant Value Tests

    /// <summary>
    /// Verifies that every error code constant
    /// matches its canonical value.
    /// </summary>
    [Theory]
    [InlineData(nameof(IdentityDomainErrorCodes.ValidationError), "IDENTITY.VALIDATION_ERROR")]
    [InlineData(nameof(IdentityDomainErrorCodes.Conflict), "IDENTITY.CONFLICT")]
    [InlineData(nameof(IdentityDomainErrorCodes.Unknown), "IDENTITY.UNKNOWN")]

    [InlineData(nameof(IdentityDomainErrorCodes.InvalidCredentials), "IDENTITY.INVALID_CREDENTIALS")]
    [InlineData(nameof(IdentityDomainErrorCodes.PasswordResetRequired), "IDENTITY.PASSWORD_RESET_REQUIRED")]
    [InlineData(nameof(IdentityDomainErrorCodes.AuthenticationChallengeRequired), "IDENTITY.AUTHENTICATION_CHALLENGE_REQUIRED")]

    [InlineData(nameof(IdentityDomainErrorCodes.UserNotFound), "IDENTITY.USER_NOT_FOUND")]
    [InlineData(nameof(IdentityDomainErrorCodes.UserLocked), "IDENTITY.USER_LOCKED")]
    [InlineData(nameof(IdentityDomainErrorCodes.UserDisabled), "IDENTITY.USER_DISABLED")]
    [InlineData(nameof(IdentityDomainErrorCodes.UsernameAlreadyExists), "IDENTITY.USERNAME_ALREADY_EXISTS")]
    [InlineData(nameof(IdentityDomainErrorCodes.EmailAlreadyExists), "IDENTITY.EMAIL_ALREADY_EXISTS")]
    [InlineData(nameof(IdentityDomainErrorCodes.PhoneAlreadyExists), "IDENTITY.PHONE_ALREADY_EXISTS")]

    [InlineData(nameof(IdentityDomainErrorCodes.InvalidState), "IDENTITY.INVALID_STATE")]
    [InlineData(nameof(IdentityDomainErrorCodes.InvalidUtc), "IDENTITY.INVALID_UTC")]
    [InlineData(nameof(IdentityDomainErrorCodes.InvalidEmail), "IDENTITY.INVALID_EMAIL")]
    [InlineData(nameof(IdentityDomainErrorCodes.InvalidPhone), "IDENTITY.INVALID_PHONE")]
    [InlineData(nameof(IdentityDomainErrorCodes.InvalidVerificationCode), "IDENTITY.INVALID_VERIFICATION_CODE")]

    [InlineData(nameof(IdentityDomainErrorCodes.InvalidPassword), "IDENTITY.INVALID_PASSWORD")]
    [InlineData(nameof(IdentityDomainErrorCodes.PasswordReuse), "IDENTITY.PASSWORD_REUSE")]
    [InlineData(nameof(IdentityDomainErrorCodes.PasswordChangeNotAllowed), "IDENTITY.PASSWORD_CHANGE_NOT_ALLOWED")]

    [InlineData(nameof(IdentityDomainErrorCodes.EmailNotVerified), "IDENTITY.EMAIL_NOT_VERIFIED")]
    [InlineData(nameof(IdentityDomainErrorCodes.PhoneNotVerified), "IDENTITY.PHONE_NOT_VERIFIED")]
    [InlineData(nameof(IdentityDomainErrorCodes.ContactNotVerified), "IDENTITY.CONTACT_NOT_VERIFIED")]

    [InlineData(nameof(IdentityDomainErrorCodes.MfaAlreadyEnabled), "IDENTITY.MFA_ALREADY_ENABLED")]
    [InlineData(nameof(IdentityDomainErrorCodes.MfaNotEnabled), "IDENTITY.MFA_NOT_ENABLED")]
    [InlineData(nameof(IdentityDomainErrorCodes.MfaConfigurationInvalid), "IDENTITY.MFA_CONFIGURATION_INVALID")]
    [InlineData(nameof(IdentityDomainErrorCodes.TotpRequired), "IDENTITY.TOTP_REQUIRED")]
    [InlineData(nameof(IdentityDomainErrorCodes.TotpSecretNotConfigured), "IDENTITY.TOTP_SECRET_NOT_CONFIGURED")]

    [InlineData(nameof(IdentityDomainErrorCodes.RoleNotFound), "IDENTITY.ROLE_NOT_FOUND")]
    [InlineData(nameof(IdentityDomainErrorCodes.RoleInactive), "IDENTITY.ROLE_INACTIVE")]
    [InlineData(nameof(IdentityDomainErrorCodes.RoleAlreadyAssigned), "IDENTITY.ROLE_ALREADY_ASSIGNED")]
    [InlineData(nameof(IdentityDomainErrorCodes.RoleNotAssigned), "IDENTITY.ROLE_NOT_ASSIGNED")]

    [InlineData(nameof(IdentityDomainErrorCodes.InvalidChallengeExpiration), "IDENTITY.INVALID_CHALLENGE_EXPIRATION")]
    [InlineData(nameof(IdentityDomainErrorCodes.ChallengeLocked), "IDENTITY.CHALLENGE_LOCKED")]
    [InlineData(nameof(IdentityDomainErrorCodes.ChallengeExpired), "IDENTITY.CHALLENGE_EXPIRED")]
    [InlineData(nameof(IdentityDomainErrorCodes.ChallengeCancelled), "IDENTITY.CHALLENGE_CANCELLED")]
    [InlineData(nameof(IdentityDomainErrorCodes.ChallengeCompleted), "IDENTITY.CHALLENGE_COMPLETED")]
    public void Constant_ShouldMatchExpectedValue(
        string fieldName,
        string expected)
    {
        // Arrange
        var field = typeof(IdentityDomainErrorCodes)
            .GetField(fieldName);

        // Act
        var actual = field!.GetRawConstantValue();

        // Assert
        actual.Should().Be(expected);
    }

    #endregion

    #region Naming Convention Tests

    /// <summary>
    /// Verifies that every error code begins
    /// with the IDENTITY prefix.
    /// </summary>
    [Fact]
    public void AllErrorCodes_ShouldStartWithIdentityPrefix()
    {
        // Arrange
        var values = typeof(IdentityDomainErrorCodes)
            .GetFields(System.Reflection.BindingFlags.Public |
                       System.Reflection.BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetRawConstantValue()!)
            .ToArray();

        // Act

        // Assert
        values.Should()
            .OnlyContain(code => code.StartsWith("IDENTITY."));
    }

    /// <summary>
    /// Verifies that every error code is unique.
    /// </summary>
    [Fact]
    public void AllErrorCodes_ShouldBeUnique()
    {
        // Arrange
        var values = typeof(IdentityDomainErrorCodes)
            .GetFields(System.Reflection.BindingFlags.Public |
                       System.Reflection.BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetRawConstantValue()!)
            .ToArray();

        // Act
        var distinct = values.Distinct().Count();

        // Assert
        distinct.Should().Be(values.Length);
    }

    /// <summary>
    /// Verifies that every error code is uppercase.
    /// </summary>
    [Fact]
    public void AllErrorCodes_ShouldBeUppercase()
    {
        // Arrange
        var values = typeof(IdentityDomainErrorCodes)
            .GetFields(System.Reflection.BindingFlags.Public |
                       System.Reflection.BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetRawConstantValue()!)
            .ToArray();

        // Act

        // Assert
        values.Should()
            .OnlyContain(code => code == code.ToUpperInvariant());
    }

    /// <summary>
    /// Verifies that every error code is not empty.
    /// </summary>
    [Fact]
    public void AllErrorCodes_ShouldNotBeEmpty()
    {
        // Arrange
        var values = typeof(IdentityDomainErrorCodes)
            .GetFields(System.Reflection.BindingFlags.Public |
                       System.Reflection.BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetRawConstantValue()!)
            .ToArray();

        // Act

        // Assert
        values.Should()
            .OnlyContain(code => !string.IsNullOrWhiteSpace(code));
    }

    #endregion
}