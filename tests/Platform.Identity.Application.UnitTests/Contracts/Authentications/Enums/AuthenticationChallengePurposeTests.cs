using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Authentication.Enums;

/// <summary>
/// Unit tests for <see cref="AuthenticationChallengePurpose"/>.
/// </summary>
public sealed class AuthenticationChallengePurposeTests
{
    /// <summary>
    /// Verifies all enum values remain stable.
    /// </summary>
    [Fact]
    public void Values_Should_BeStable()
    {
        ((int)AuthenticationChallengePurpose.Login).Should().Be(0);
        ((int)AuthenticationChallengePurpose.PasswordReset).Should().Be(1);
        ((int)AuthenticationChallengePurpose.EmailVerification).Should().Be(2);
        ((int)AuthenticationChallengePurpose.PhoneVerification).Should().Be(3);
        ((int)AuthenticationChallengePurpose.SensitiveOperation).Should().Be(4);
        ((int)AuthenticationChallengePurpose.AccountRecovery).Should().Be(5);
        ((int)AuthenticationChallengePurpose.Custom).Should().Be(6);
    }

    /// <summary>
    /// Verifies enum names remain stable.
    /// </summary>
    [Fact]
    public void Names_Should_BeStable()
    {
        Enum.GetNames(typeof(AuthenticationChallengePurpose))
            .Should()
            .Equal(
                nameof(AuthenticationChallengePurpose.Login),
                nameof(AuthenticationChallengePurpose.PasswordReset),
                nameof(AuthenticationChallengePurpose.EmailVerification),
                nameof(AuthenticationChallengePurpose.PhoneVerification),
                nameof(AuthenticationChallengePurpose.SensitiveOperation),
                nameof(AuthenticationChallengePurpose.AccountRecovery),
                nameof(AuthenticationChallengePurpose.Custom));
    }

    /// <summary>
    /// Verifies enum contains expected values.
    /// </summary>
    [Fact]
    public void GetValues_Should_ReturnAllValues()
    {
        Enum.GetValues<AuthenticationChallengePurpose>()
            .Should()
            .BeEquivalentTo(
            [
                AuthenticationChallengePurpose.Login,
                AuthenticationChallengePurpose.PasswordReset,
                AuthenticationChallengePurpose.EmailVerification,
                AuthenticationChallengePurpose.PhoneVerification,
                AuthenticationChallengePurpose.SensitiveOperation,
                AuthenticationChallengePurpose.AccountRecovery,
                AuthenticationChallengePurpose.Custom
            ]);
    }

    /// <summary>
    /// Verifies every declared value is defined.
    /// </summary>
    [Theory]
    [InlineData(AuthenticationChallengePurpose.Login)]
    [InlineData(AuthenticationChallengePurpose.PasswordReset)]
    [InlineData(AuthenticationChallengePurpose.EmailVerification)]
    [InlineData(AuthenticationChallengePurpose.PhoneVerification)]
    [InlineData(AuthenticationChallengePurpose.SensitiveOperation)]
    [InlineData(AuthenticationChallengePurpose.AccountRecovery)]
    [InlineData(AuthenticationChallengePurpose.Custom)]
    public void Enum_Should_BeDefined(AuthenticationChallengePurpose value)
    {
        Enum.IsDefined(value).Should().BeTrue();
    }

    /// <summary>
    /// Verifies integer values map correctly.
    /// </summary>
    [Theory]
    [InlineData(0, AuthenticationChallengePurpose.Login)]
    [InlineData(1, AuthenticationChallengePurpose.PasswordReset)]
    [InlineData(2, AuthenticationChallengePurpose.EmailVerification)]
    [InlineData(3, AuthenticationChallengePurpose.PhoneVerification)]
    [InlineData(4, AuthenticationChallengePurpose.SensitiveOperation)]
    [InlineData(5, AuthenticationChallengePurpose.AccountRecovery)]
    [InlineData(6, AuthenticationChallengePurpose.Custom)]
    public void Cast_From_Int_Should_ReturnExpectedValue(
        int raw,
        AuthenticationChallengePurpose expected)
    {
        ((AuthenticationChallengePurpose)raw)
            .Should()
            .Be(expected);
    }

    /// <summary>
    /// Verifies string representation remains stable.
    /// </summary>
    [Theory]
    [InlineData(AuthenticationChallengePurpose.Login, "Login")]
    [InlineData(AuthenticationChallengePurpose.PasswordReset, "PasswordReset")]
    [InlineData(AuthenticationChallengePurpose.EmailVerification, "EmailVerification")]
    [InlineData(AuthenticationChallengePurpose.PhoneVerification, "PhoneVerification")]
    [InlineData(AuthenticationChallengePurpose.SensitiveOperation, "SensitiveOperation")]
    [InlineData(AuthenticationChallengePurpose.AccountRecovery, "AccountRecovery")]
    [InlineData(AuthenticationChallengePurpose.Custom, "Custom")]
    public void ToString_Should_ReturnExpectedName(
        AuthenticationChallengePurpose value,
        string expected)
    {
        value.ToString().Should().Be(expected);
    }
}