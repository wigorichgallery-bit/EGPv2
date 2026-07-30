// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Constants/SystemRoleIdsTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Constants;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Constants;

/// <summary>
/// Contains unit tests for the
/// <see cref="SystemRoleIds"/> constants.
/// </summary>
public sealed class SystemRoleIdsTests
{
    #region Constant Value Tests

    /// <summary>
    /// Verifies that the System Administrator
    /// identifier matches the expected value.
    /// </summary>
    [Fact]
    public void SystemAdministrator_ShouldMatchExpectedGuid()
    {
        // Arrange
        var expected = Guid.Parse(
            "A8A5C41E-9F42-4E55-BEE8-000000000001");

        // Act
        var actual = SystemRoleIds.SystemAdministrator;

        // Assert
        actual.Should().Be(expected);
    }

    /// <summary>
    /// Verifies that the Governance Administrator
    /// identifier matches the expected value.
    /// </summary>
    [Fact]
    public void GovernanceAdministrator_ShouldMatchExpectedGuid()
    {
        // Arrange
        var expected = Guid.Parse(
            "A8A5C41E-9F42-4E55-BEE8-000000000002");

        // Act
        var actual = SystemRoleIds.GovernanceAdministrator;

        // Assert
        actual.Should().Be(expected);
    }

    /// <summary>
    /// Verifies that the Security Administrator
    /// identifier matches the expected value.
    /// </summary>
    [Fact]
    public void SecurityAdministrator_ShouldMatchExpectedGuid()
    {
        // Arrange
        var expected = Guid.Parse(
            "A8A5C41E-9F42-4E55-BEE8-000000000003");

        // Act
        var actual = SystemRoleIds.SecurityAdministrator;

        // Assert
        actual.Should().Be(expected);
    }

    /// <summary>
    /// Verifies that the Auditor
    /// identifier matches the expected value.
    /// </summary>
    [Fact]
    public void Auditor_ShouldMatchExpectedGuid()
    {
        // Arrange
        var expected = Guid.Parse(
            "A8A5C41E-9F42-4E55-BEE8-000000000004");

        // Act
        var actual = SystemRoleIds.Auditor;

        // Assert
        actual.Should().Be(expected);
    }

    /// <summary>
    /// Verifies that the Operator
    /// identifier matches the expected value.
    /// </summary>
    [Fact]
    public void Operator_ShouldMatchExpectedGuid()
    {
        // Arrange
        var expected = Guid.Parse(
            "A8A5C41E-9F42-4E55-BEE8-000000000005");

        // Act
        var actual = SystemRoleIds.Operator;

        // Assert
        actual.Should().Be(expected);
    }

    #endregion

    #region Uniqueness Tests

    /// <summary>
    /// Verifies that every built-in role identifier
    /// is unique.
    /// </summary>
    [Fact]
    public void AllRoleIdentifiers_ShouldBeUnique()
    {
        // Arrange
        var roleIds = new[]
        {
            SystemRoleIds.SystemAdministrator,
            SystemRoleIds.GovernanceAdministrator,
            SystemRoleIds.SecurityAdministrator,
            SystemRoleIds.Auditor,
            SystemRoleIds.Operator
        };

        // Act
        var distinctCount = roleIds.Distinct().Count();

        // Assert
        distinctCount.Should().Be(roleIds.Length);
    }

    #endregion

    #region Non Empty Tests

    /// <summary>
    /// Verifies that every built-in role identifier
    /// is not <see cref="Guid.Empty"/>.
    /// </summary>
    [Fact]
    public void AllRoleIdentifiers_ShouldNotBeEmpty()
    {
        // Arrange
        var roleIds = new[]
        {
            SystemRoleIds.SystemAdministrator,
            SystemRoleIds.GovernanceAdministrator,
            SystemRoleIds.SecurityAdministrator,
            SystemRoleIds.Auditor,
            SystemRoleIds.Operator
        };

        // Act

        // Assert
        roleIds.Should().OnlyContain(
            id => id != Guid.Empty);
    }

    #endregion
}