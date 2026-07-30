// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Enums/UserStatusTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Enums;

/// <summary>
/// Contains unit tests for
/// <see cref="UserStatus"/>.
/// </summary>
public sealed class UserStatusTests
{
    #region Enum Value Tests

    [Theory]
    [InlineData(UserStatus.Active, 1)]
    [InlineData(UserStatus.Locked, 2)]
    [InlineData(UserStatus.Disabled, 3)]
    public void EnumMember_ShouldHaveExpectedValue(
        UserStatus value,
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
        Enum.GetValues<UserStatus>()
            .Should()
            .HaveCount(3);
    }

    [Fact]
    public void EnumValues_ShouldBeUnique()
    {
        var values =
            Enum.GetValues<UserStatus>()
                .Cast<int>()
                .ToArray();

        values.Distinct().Should().HaveCount(values.Length);
    }

    [Fact]
    public void EnumNames_ShouldMatchProductionDefinition()
    {
        var expected = new[]
        {
            nameof(UserStatus.Active),
            nameof(UserStatus.Locked),
            nameof(UserStatus.Disabled)
        };

        Enum.GetNames<UserStatus>()
            .Should()
            .Equal(expected);
    }

    [Fact]
    public void EnumValues_ShouldBeSequentialStartingAtOne()
    {
        // Arrange
        var expected = new[] { 1, 2, 3 };

        // Act
        var actual =
            Enum.GetValues<UserStatus>()
                .Cast<int>()
                .ToArray();

        // Assert
        actual.Should().Equal(expected);
    }

    #endregion
}