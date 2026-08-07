using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Actions;

/// <summary>
/// Unit tests for <see cref="LoginCommand"/>.
/// </summary>
public sealed class LoginCommandTests
{
    /// <summary>
    /// Verifies constructor initializes all values.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_Properties()
    {
        // Arrange
        const string identity = "john.doe";
        const string password = "Password123";

        // Act
        var command = new LoginCommand(
            identity,
            password);

        // Assert
        command.Identity.Should().Be(identity);
        command.Password.Should().Be(password);
    }

    /// <summary>
    /// Verifies the governance policy is correct.
    /// </summary>
    [Fact]
    public void GovernancePolicy_Should_Be_Correct()
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            "Password123");

        // Assert
        command.GovernancePolicy
            .Should()
            .Be("IDENTITY.AUTH.LOGIN");
    }

    /// <summary>
    /// Verifies the protected resource is correct.
    /// </summary>
    [Fact]
    public void Resource_Should_Be_Correct()
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            "Password123");

        // Assert
        command.Resource
            .Should()
            .Be("Authentication");
    }

    /// <summary>
    /// Verifies the requested action is correct.
    /// </summary>
    [Fact]
    public void Action_Should_Be_Correct()
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            "Password123");

        // Assert
        command.Action
            .Should()
            .Be("Login");
    }

    /// <summary>
    /// Verifies two commands having identical values are equal.
    /// </summary>
    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        // Arrange
        var first = new LoginCommand(
            "john.doe",
            "Password123");

        var second = new LoginCommand(
            "john.doe",
            "Password123");

        // Assert
        first.Should().Be(second);
        first.GetHashCode()
            .Should()
            .Be(second.GetHashCode());
    }

    /// <summary>
    /// Verifies two commands having different values are not equal.
    /// </summary>
    [Fact]
    public void Record_Should_Not_Be_Equal_When_Values_Differ()
    {
        // Arrange
        var first = new LoginCommand(
            "john.doe",
            "Password123");

        var second = new LoginCommand(
            "jane.doe",
            "Password123");

        // Assert
        first.Should().NotBe(second);
    }

    /// <summary>
    /// Verifies deconstruction preserves values.
    /// </summary>
    [Fact]
    public void Record_Should_Deconstruct_Correctly()
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            "Password123");

        // Act
        var (identity, password) = command;

        // Assert
        identity.Should().Be("john.doe");
        password.Should().Be("Password123");
    }

    /// <summary>
    /// Verifies the generated string representation contains
    /// record values.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Record_Values()
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            "Password123");

        // Act
        var text = command.ToString();

        // Assert
        text.Should().Contain(nameof(LoginCommand.Identity));
        text.Should().Contain(nameof(LoginCommand.Password));
        text.Should().Contain("john.doe");
        text.Should().Contain("Password123");
    }
}