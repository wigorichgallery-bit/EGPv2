// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Enums/AuthenticationChallengeCancellationReasonTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Enums;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengeCancellationReason"/>.
/// </summary>
public sealed class AuthenticationChallengeCancellationReasonTests
{
    #region Enum Value Tests

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeCancellationReason.UserCancelled"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void UserCancelled_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value =
            AuthenticationChallengeCancellationReason.UserCancelled;

        // Assert
        value.Should()
            .Be(AuthenticationChallengeCancellationReason.UserCancelled);

        ((int)value).Should().Be(0);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeCancellationReason.SystemCancelled"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void SystemCancelled_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value =
            AuthenticationChallengeCancellationReason.SystemCancelled;

        // Assert
        ((int)value).Should().Be(1);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeCancellationReason.Superseded"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Superseded_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value =
            AuthenticationChallengeCancellationReason.Superseded;

        // Assert
        ((int)value).Should().Be(2);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeCancellationReason.SessionEnded"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void SessionEnded_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value =
            AuthenticationChallengeCancellationReason.SessionEnded;

        // Assert
        ((int)value).Should().Be(3);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeCancellationReason.AdministratorCancelled"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void AdministratorCancelled_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value =
            AuthenticationChallengeCancellationReason.AdministratorCancelled;

        // Assert
        ((int)value).Should().Be(4);
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
        var values = Enum.GetValues<AuthenticationChallengeCancellationReason>();

        // Assert
        values.Should().HaveCount(5);
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
        var values = Enum
            .GetValues<AuthenticationChallengeCancellationReason>()
            .Cast<int>()
            .ToArray();

        // Assert
        values.Distinct().Should().HaveCount(values.Length);
    }

    /// <summary>
    /// Verifies that the enumeration names
    /// match the production source.
    /// </summary>
    [Fact]
    public void EnumNames_ShouldMatchProductionDefinition()
    {
        // Arrange
        var expected =
            new[]
            {
                nameof(AuthenticationChallengeCancellationReason.UserCancelled),
                nameof(AuthenticationChallengeCancellationReason.SystemCancelled),
                nameof(AuthenticationChallengeCancellationReason.Superseded),
                nameof(AuthenticationChallengeCancellationReason.SessionEnded),
                nameof(AuthenticationChallengeCancellationReason.AdministratorCancelled)
            };

        // Act
        var actual =
            Enum.GetNames<AuthenticationChallengeCancellationReason>();

        // Assert
        actual.Should().Equal(expected);
    }

    #endregion
}