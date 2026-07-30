// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Enums/AuthenticationChallengeStatusTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Enums;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengeStatus"/>.
/// </summary>
public sealed class AuthenticationChallengeStatusTests
{
    #region Enum Value Tests

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeStatus.Pending"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Pending_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengeStatus.Pending;

        // Assert
        value.Should().Be(AuthenticationChallengeStatus.Pending);
        ((int)value).Should().Be(0);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeStatus.Completed"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Completed_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengeStatus.Completed;

        // Assert
        ((int)value).Should().Be(1);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeStatus.Expired"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Expired_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengeStatus.Expired;

        // Assert
        ((int)value).Should().Be(2);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeStatus.Cancelled"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Cancelled_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengeStatus.Cancelled;

        // Assert
        ((int)value).Should().Be(3);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeStatus.Locked"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Locked_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengeStatus.Locked;

        // Assert
        ((int)value).Should().Be(4);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="AuthenticationChallengeStatus.Revoked"/>
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Revoked_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act
        var value = AuthenticationChallengeStatus.Revoked;

        // Assert
        ((int)value).Should().Be(5);
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
            Enum.GetValues<AuthenticationChallengeStatus>();

        // Assert
        values.Should().HaveCount(6);
    }

    /// <summary>
    /// Verifies that every enumeration value
    /// is unique.
    /// </summary>
    [Fact]
    public void EnumValues_ShouldBeUnique()
    {
        // Arrange

        // Act
        var values =
            Enum.GetValues<AuthenticationChallengeStatus>()
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
        // Arrange
        var expected = new[]
        {
            nameof(AuthenticationChallengeStatus.Pending),
            nameof(AuthenticationChallengeStatus.Completed),
            nameof(AuthenticationChallengeStatus.Expired),
            nameof(AuthenticationChallengeStatus.Cancelled),
            nameof(AuthenticationChallengeStatus.Locked),
            nameof(AuthenticationChallengeStatus.Revoked)
        };

        // Act
        var actual =
            Enum.GetNames<AuthenticationChallengeStatus>();

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
            Enumerable.Range(0, 6).ToArray();

        // Act
        var actual =
            Enum.GetValues<AuthenticationChallengeStatus>()
                .Cast<int>()
                .ToArray();

        // Assert
        actual.Should().Equal(expected);
    }

    #endregion
}