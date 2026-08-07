using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Authentication.Enums;

/// <summary>
/// Unit tests for <see cref="AuthenticationStatus"/>.
/// </summary>
public sealed class AuthenticationStatusTests
{
    /// <summary>
    /// Verifies all enum numeric values remain stable.
    /// </summary>
    [Fact]
    public void Values_Should_BeStable()
    {
        ((int)AuthenticationStatus.Success).Should().Be(0);
        ((int)AuthenticationStatus.ChallengeRequired).Should().Be(1);
        ((int)AuthenticationStatus.EmailVerificationRequired).Should().Be(2);
        ((int)AuthenticationStatus.PhoneVerificationRequired).Should().Be(3);
        ((int)AuthenticationStatus.PasswordExpired).Should().Be(4);
        ((int)AuthenticationStatus.Locked).Should().Be(5);
    }

    /// <summary>
    /// Verifies enum names remain stable.
    /// </summary>
    [Fact]
    public void Names_Should_BeStable()
    {
        Enum.GetNames(typeof(AuthenticationStatus))
            .Should()
            .Equal(
                nameof(AuthenticationStatus.Success),
                nameof(AuthenticationStatus.ChallengeRequired),
                nameof(AuthenticationStatus.EmailVerificationRequired),
                nameof(AuthenticationStatus.PhoneVerificationRequired),
                nameof(AuthenticationStatus.PasswordExpired),
                nameof(AuthenticationStatus.Locked));
    }

    /// <summary>
    /// Verifies all enum values are returned.
    /// </summary>
    [Fact]
    public void GetValues_Should_ReturnAllValues()
    {
        Enum.GetValues<AuthenticationStatus>()
            .Should()
            .Equal(
                AuthenticationStatus.Success,
                AuthenticationStatus.ChallengeRequired,
                AuthenticationStatus.EmailVerificationRequired,
                AuthenticationStatus.PhoneVerificationRequired,
                AuthenticationStatus.PasswordExpired,
                AuthenticationStatus.Locked);
    }

    /// <summary>
    /// Verifies the enum contains exactly six members.
    /// </summary>
    [Fact]
    public void Count_Should_BeSix()
    {
        Enum.GetValues<AuthenticationStatus>()
            .Should()
            .HaveCount(6);
    }

    /// <summary>
    /// Verifies every declared value is defined.
    /// </summary>
    [Theory]
    [InlineData(AuthenticationStatus.Success)]
    [InlineData(AuthenticationStatus.ChallengeRequired)]
    [InlineData(AuthenticationStatus.EmailVerificationRequired)]
    [InlineData(AuthenticationStatus.PhoneVerificationRequired)]
    [InlineData(AuthenticationStatus.PasswordExpired)]
    [InlineData(AuthenticationStatus.Locked)]
    public void Enum_Should_BeDefined(AuthenticationStatus value)
    {
        Enum.IsDefined(value).Should().BeTrue();
    }

    /// <summary>
    /// Verifies undefined values are rejected.
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    [InlineData(100)]
    public void Invalid_Value_Should_Not_BeDefined(int value)
    {
        Enum.IsDefined(typeof(AuthenticationStatus), value)
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies integer values map correctly.
    /// </summary>
    [Theory]
    [InlineData(0, AuthenticationStatus.Success)]
    [InlineData(1, AuthenticationStatus.ChallengeRequired)]
    [InlineData(2, AuthenticationStatus.EmailVerificationRequired)]
    [InlineData(3, AuthenticationStatus.PhoneVerificationRequired)]
    [InlineData(4, AuthenticationStatus.PasswordExpired)]
    [InlineData(5, AuthenticationStatus.Locked)]
    public void Cast_From_Int_Should_ReturnExpectedValue(
        int raw,
        AuthenticationStatus expected)
    {
        ((AuthenticationStatus)raw)
            .Should()
            .Be(expected);
    }

    /// <summary>
    /// Verifies string representations remain stable.
    /// </summary>
    [Theory]
    [InlineData(AuthenticationStatus.Success, "Success")]
    [InlineData(AuthenticationStatus.ChallengeRequired, "ChallengeRequired")]
    [InlineData(AuthenticationStatus.EmailVerificationRequired, "EmailVerificationRequired")]
    [InlineData(AuthenticationStatus.PhoneVerificationRequired, "PhoneVerificationRequired")]
    [InlineData(AuthenticationStatus.PasswordExpired, "PasswordExpired")]
    [InlineData(AuthenticationStatus.Locked, "Locked")]
    public void ToString_Should_ReturnExpectedName(
        AuthenticationStatus value,
        string expected)
    {
        value.ToString().Should().Be(expected);
    }
}