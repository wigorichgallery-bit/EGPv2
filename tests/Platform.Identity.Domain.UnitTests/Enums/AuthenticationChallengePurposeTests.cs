// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Enums/AuthenticationChallengePurposeTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Enums;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengePurpose"/>.
/// </summary>
public sealed class AuthenticationChallengePurposeTests
{
    #region Enum Value Tests

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengePurpose.Login"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Login_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengePurpose.Login;

        // Assert
        value.Should().Be(AuthenticationChallengePurpose.Login);
        ((int)value).Should().Be(0);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengePurpose.PasswordReset"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void PasswordReset_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengePurpose.PasswordReset;

        // Assert
        ((int)value).Should().Be(1);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengePurpose.EmailVerification"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void EmailVerification_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengePurpose.EmailVerification;

        // Assert
        ((int)value).Should().Be(2);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengePurpose.PhoneVerification"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void PhoneVerification_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengePurpose.PhoneVerification;

        // Assert
        ((int)value).Should().Be(3);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengePurpose.SensitiveOperation"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void SensitiveOperation_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengePurpose.SensitiveOperation;

        // Assert
        ((int)value).Should().Be(4);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengePurpose.AccountRecovery"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void AccountRecovery_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengePurpose.AccountRecovery;

        // Assert
        ((int)value).Should().Be(5);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengePurpose.Custom"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Custom_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengePurpose.Custom;

        // Assert
        ((int)value).Should().Be(6);
    }

    #endregion

    #region Enum Definition Tests

    /// <summary>
    /// Verifies that the enumeration
    /// contains the expected number of values.
    /// </summary>
    [Fact]
    public void Enum_ShouldContainExpectedNumberOfValues()
    {
        // Arrange

        // Act
        var values =
            Enum.GetValues<AuthenticationChallengePurpose>();

        // Assert
        values.Should().HaveCount(7);
    }

    /// <summary>
    /// Verifies that every enumeration
    /// value is unique.
    /// </summary>
    [Fact]
    public void EnumValues_ShouldBeUnique()
    {
        // Arrange

        // Act
        var values =
            Enum.GetValues<AuthenticationChallengePurpose>()
                .Cast<int>()
                .ToArray();

        // Assert
        values.Distinct().Should().HaveCount(values.Length);
    }

    /// <summary>
    /// Verifies that the enumeration names
    /// exactly match the production source.
    /// </summary>
    [Fact]
    public void EnumNames_ShouldMatchProductionDefinition()
    {
        // Arrange
        var expected =
            new[]
            {
                nameof(AuthenticationChallengePurpose.Login),
                nameof(AuthenticationChallengePurpose.PasswordReset),
                nameof(AuthenticationChallengePurpose.EmailVerification),
                nameof(AuthenticationChallengePurpose.PhoneVerification),
                nameof(AuthenticationChallengePurpose.SensitiveOperation),
                nameof(AuthenticationChallengePurpose.AccountRecovery),
                nameof(AuthenticationChallengePurpose.Custom)
            };

        // Act
        var actual =
            Enum.GetNames<AuthenticationChallengePurpose>();

        // Assert
        actual.Should().Equal(expected);
    }

    /// <summary>
    /// Verifies that the numeric values are
    /// sequential beginning at zero.
    /// </summary>
    [Fact]
    public void EnumValues_ShouldBeSequential()
    {
        // Arrange
        var expected =
            Enumerable.Range(0, 7).ToArray();

        // Act
        var actual =
            Enum.GetValues<AuthenticationChallengePurpose>()
                .Cast<int>()
                .ToArray();

        // Assert
        actual.Should().Equal(expected);
    }

    #endregion
}