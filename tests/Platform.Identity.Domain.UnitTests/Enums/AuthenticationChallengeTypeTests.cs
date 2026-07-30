// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Enums/AuthenticationChallengeTypeTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Enums;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengeType"/>.
/// </summary>
public sealed class AuthenticationChallengeTypeTests
{
    #region Enum Value Tests

    [Theory]
    [InlineData(AuthenticationChallengeType.None, 0)]
    [InlineData(AuthenticationChallengeType.Totp, 1)]
    [InlineData(AuthenticationChallengeType.EmailOtp, 2)]
    [InlineData(AuthenticationChallengeType.SmsOtp, 3)]
    [InlineData(AuthenticationChallengeType.WhatsAppOtp, 4)]
    [InlineData(AuthenticationChallengeType.Passkey, 5)]
    [InlineData(AuthenticationChallengeType.RecoveryCode, 6)]
    [InlineData(AuthenticationChallengeType.MagicLink, 7)]
    [InlineData(AuthenticationChallengeType.Custom, 8)]
    public void EnumMember_ShouldHaveExpectedValue(
        AuthenticationChallengeType value,
        int expected)
    {
        // Arrange

        // Act

        // Assert
        ((int)value).Should().Be(expected);
    }

    #endregion

    #region Enum Definition Tests

    [Fact]
    public void Enum_ShouldContainExpectedNumberOfValues()
    {
        // Arrange

        // Act
        var values =
            Enum.GetValues<AuthenticationChallengeType>();

        // Assert
        values.Should().HaveCount(9);
    }

    [Fact]
    public void EnumValues_ShouldBeUnique()
    {
        // Arrange

        // Act
        var values =
            Enum.GetValues<AuthenticationChallengeType>()
                .Cast<int>()
                .ToArray();

        // Assert
        values.Distinct().Should().HaveCount(values.Length);
    }

    [Fact]
    public void EnumNames_ShouldMatchProductionDefinition()
    {
        // Arrange
        var expected = new[]
        {
            nameof(AuthenticationChallengeType.None),
            nameof(AuthenticationChallengeType.Totp),
            nameof(AuthenticationChallengeType.EmailOtp),
            nameof(AuthenticationChallengeType.SmsOtp),
            nameof(AuthenticationChallengeType.WhatsAppOtp),
            nameof(AuthenticationChallengeType.Passkey),
            nameof(AuthenticationChallengeType.RecoveryCode),
            nameof(AuthenticationChallengeType.MagicLink),
            nameof(AuthenticationChallengeType.Custom)
        };

        // Act
        var actual =
            Enum.GetNames<AuthenticationChallengeType>();

        // Assert
        actual.Should().Equal(expected);
    }

    [Fact]
    public void EnumValues_ShouldBeSequential()
    {
        // Arrange
        var expected =
            Enumerable.Range(0, 9).ToArray();

        // Act
        var actual =
            Enum.GetValues<AuthenticationChallengeType>()
                .Cast<int>()
                .ToArray();

        // Assert
        actual.Should().Equal(expected);
    }

    #endregion
}