using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="CreateUserCommand"/>.
/// </summary>
public sealed class CreateUserCommandTests
{
    /// <summary>
    /// Verifies constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange

        const string username =
            "john.doe";

        const string email =
            "john.doe@example.com";

        const string phoneNumber =
            "+6281234567890";

        const string password =
            "Password123!";

        // Act

        var command =
            new CreateUserCommand(
                username,
                email,
                phoneNumber,
                password);

        // Assert

        command.Username
            .Should()
            .Be(username);

        command.Email
            .Should()
            .Be(email);

        command.PhoneNumber
            .Should()
            .Be(phoneNumber);

        command.Password
            .Should()
            .Be(password);
    }

    /// <summary>
    /// Verifies governance policy is correct.
    /// </summary>
    [Fact]
    public void GovernancePolicy_Should_Return_Expected_Value()
    {
        // Arrange

        var command =
            new CreateUserCommand(
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                "Password123!");

        // Assert

        command.GovernancePolicy
            .Should()
            .Be("IDENTITY.USER.CREATE");
    }

    /// <summary>
    /// Verifies protected resource is correct.
    /// </summary>
    [Fact]
    public void Resource_Should_Return_Expected_Value()
    {
        // Arrange

        var command =
            new CreateUserCommand(
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                "Password123!");

        // Assert

        command.Resource
            .Should()
            .Be("User");
    }

    /// <summary>
    /// Verifies requested action is correct.
    /// </summary>
    [Fact]
    public void Action_Should_Return_Expected_Value()
    {
        // Arrange

        var command =
            new CreateUserCommand(
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                "Password123!");

        // Assert

        command.Action
            .Should()
            .Be("Create");
    }

    /// <summary>
    /// Verifies commands with identical values
    /// are value-equal.
    /// </summary>
    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        // Arrange

        var left =
            new CreateUserCommand(
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                "Password123!");

        var right =
            new CreateUserCommand(
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                "Password123!");

        // Assert

        left.Should()
            .Be(right);

        left.GetHashCode()
            .Should()
            .Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies commands with different values
    /// are not equal.
    /// </summary>
    [Fact]
    public void Record_Should_Not_Be_Equal_When_Values_Are_Different()
    {
        // Arrange

        var left =
            new CreateUserCommand(
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                "Password123!");

        var right =
            new CreateUserCommand(
                "jane.doe",
                "jane.doe@example.com",
                "+6289876543210",
                "AnotherPassword123!");

        // Assert

        left.Should()
            .NotBe(right);
    }
}