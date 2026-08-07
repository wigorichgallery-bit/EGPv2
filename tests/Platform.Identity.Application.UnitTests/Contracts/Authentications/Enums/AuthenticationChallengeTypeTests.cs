using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Authentication.Enums;

/// <summary>
/// Unit tests for <see cref="AuthenticationChallengeType"/>.
/// </summary>
public sealed class AuthenticationChallengeTypeTests
{
    /// <summary>
    /// Verifies all enum numeric values remain stable.
    /// </summary>
    [Fact]
    public void Values_Should_BeStable()
    {
        ((int)AuthenticationChallengeType.None).Should().Be(0);
        ((int)AuthenticationChallengeType.Totp).Should().Be(1);
        ((int)AuthenticationChallengeType.EmailOtp).Should().Be(2);
        ((int)AuthenticationChallengeType.SmsOtp).Should().Be(3);
        ((int)AuthenticationChallengeType.WhatsAppOtp).Should().Be(4);
        ((int)AuthenticationChallengeType.Passkey).Should().Be(5);
        ((int)AuthenticationChallengeType.RecoveryCode).Should().Be(6);
        ((int)AuthenticationChallengeType.MagicLink).Should().Be(7);
        ((int)AuthenticationChallengeType.Custom).Should().Be(8);
    }

    /// <summary>
    /// Verifies enum names remain stable.
    /// </summary>
    [Fact]
    public void Names_Should_BeStable()
    {
        Enum.GetNames(typeof(AuthenticationChallengeType))
            .Should()
            .Equal(
                nameof(AuthenticationChallengeType.None),
                nameof(AuthenticationChallengeType.Totp),
                nameof(AuthenticationChallengeType.EmailOtp),
                nameof(AuthenticationChallengeType.SmsOtp),
                nameof(AuthenticationChallengeType.WhatsAppOtp),
                nameof(AuthenticationChallengeType.Passkey),
                nameof(AuthenticationChallengeType.RecoveryCode),
                nameof(AuthenticationChallengeType.MagicLink),
                nameof(AuthenticationChallengeType.Custom));
    }

    /// <summary>
    /// Verifies all enum values are returned.
    /// </summary>
    [Fact]
    public void GetValues_Should_ReturnAllValues()
    {
        Enum.GetValues<AuthenticationChallengeType>()
            .Should()
            .Equal(
                AuthenticationChallengeType.None,
                AuthenticationChallengeType.Totp,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengeType.SmsOtp,
                AuthenticationChallengeType.WhatsAppOtp,
                AuthenticationChallengeType.Passkey,
                AuthenticationChallengeType.RecoveryCode,
                AuthenticationChallengeType.MagicLink,
                AuthenticationChallengeType.Custom);
    }

    /// <summary>
    /// Verifies the enum contains exactly nine members.
    /// </summary>
    [Fact]
    public void Count_Should_BeNine()
    {
        Enum.GetValues<AuthenticationChallengeType>()
            .Should()
            .HaveCount(9);
    }

    /// <summary>
    /// Verifies every declared value is defined.
    /// </summary>
    [Theory]
    [InlineData(AuthenticationChallengeType.None)]
    [InlineData(AuthenticationChallengeType.Totp)]
    [InlineData(AuthenticationChallengeType.EmailOtp)]
    [InlineData(AuthenticationChallengeType.SmsOtp)]
    [InlineData(AuthenticationChallengeType.WhatsAppOtp)]
    [InlineData(AuthenticationChallengeType.Passkey)]
    [InlineData(AuthenticationChallengeType.RecoveryCode)]
    [InlineData(AuthenticationChallengeType.MagicLink)]
    [InlineData(AuthenticationChallengeType.Custom)]
    public void Enum_Should_BeDefined(AuthenticationChallengeType value)
    {
        Enum.IsDefined(value).Should().BeTrue();
    }

    /// <summary>
    /// Verifies undefined values are rejected.
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(9)]
    [InlineData(100)]
    public void Invalid_Value_Should_Not_BeDefined(int value)
    {
        Enum.IsDefined(typeof(AuthenticationChallengeType), value)
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies integer values map correctly.
    /// </summary>
    [Theory]
    [InlineData(0, AuthenticationChallengeType.None)]
    [InlineData(1, AuthenticationChallengeType.Totp)]
    [InlineData(2, AuthenticationChallengeType.EmailOtp)]
    [InlineData(3, AuthenticationChallengeType.SmsOtp)]
    [InlineData(4, AuthenticationChallengeType.WhatsAppOtp)]
    [InlineData(5, AuthenticationChallengeType.Passkey)]
    [InlineData(6, AuthenticationChallengeType.RecoveryCode)]
    [InlineData(7, AuthenticationChallengeType.MagicLink)]
    [InlineData(8, AuthenticationChallengeType.Custom)]
    public void Cast_From_Int_Should_ReturnExpectedValue(
        int raw,
        AuthenticationChallengeType expected)
    {
        ((AuthenticationChallengeType)raw)
            .Should()
            .Be(expected);
    }

    /// <summary>
    /// Verifies string representations remain stable.
    /// </summary>
    [Theory]
    [InlineData(AuthenticationChallengeType.None, "None")]
    [InlineData(AuthenticationChallengeType.Totp, "Totp")]
    [InlineData(AuthenticationChallengeType.EmailOtp, "EmailOtp")]
    [InlineData(AuthenticationChallengeType.SmsOtp, "SmsOtp")]
    [InlineData(AuthenticationChallengeType.WhatsAppOtp, "WhatsAppOtp")]
    [InlineData(AuthenticationChallengeType.Passkey, "Passkey")]
    [InlineData(AuthenticationChallengeType.RecoveryCode, "RecoveryCode")]
    [InlineData(AuthenticationChallengeType.MagicLink, "MagicLink")]
    [InlineData(AuthenticationChallengeType.Custom, "Custom")]
    public void ToString_Should_ReturnExpectedName(
        AuthenticationChallengeType value,
        string expected)
    {
        value.ToString().Should().Be(expected);
    }
}