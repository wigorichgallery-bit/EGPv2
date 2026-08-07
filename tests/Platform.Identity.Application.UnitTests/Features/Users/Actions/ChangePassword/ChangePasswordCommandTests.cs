using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Pipeline.Abstractions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="ChangePasswordCommand"/>.
/// </summary>
public sealed class ChangePasswordCommandTests
{
    /// <summary>
    /// Verifies constructor initializes
    /// all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_Properties()
    {
        // Arrange

        var userId =
            Guid.NewGuid();

        // Act

        var command =
            new ChangePasswordCommand(
                userId,
                "CurrentPassword123!",
                "NewPassword123!");

        // Assert

        command.UserId
            .Should()
            .Be(userId);

        command.CurrentPassword
            .Should()
            .Be("CurrentPassword123!");

        command.NewPassword
            .Should()
            .Be("NewPassword123!");
    }

    /// <summary>
    /// Verifies governance policy
    /// is correct.
    /// </summary>
    [Fact]
    public void GovernancePolicy_Should_Return_Expected_Value()
    {
        // Arrange

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                "NewPassword123!");

        // Assert

        command.GovernancePolicy
            .Should()
            .Be("IDENTITY.PASSWORD.CHANGE");
    }

    /// <summary>
    /// Verifies protected resource
    /// is correct.
    /// </summary>
    [Fact]
    public void Resource_Should_Return_Expected_Value()
    {
        // Arrange

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                "NewPassword123!");

        // Assert

        command.Resource
            .Should()
            .Be("User");
    }

    /// <summary>
    /// Verifies requested action
    /// is correct.
    /// </summary>
    [Fact]
    public void Action_Should_Return_Expected_Value()
    {
        // Arrange

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                "NewPassword123!");

        // Assert

        command.Action
            .Should()
            .Be("ChangePassword");
    }

    /// <summary>
    /// Verifies command implements
    /// <see cref="ICommand"/>.
    /// </summary>
    [Fact]
    public void Should_Implement_ICommand()
    {
        // Arrange

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                "NewPassword123!");

        // Assert

        command
            .Should()
            .BeAssignableTo<ICommand>();
    }

    /// <summary>
    /// Verifies command implements
    /// <see cref="IGovernanceRequest"/>.
    /// </summary>
    [Fact]
    public void Should_Implement_IGovernanceRequest()
    {
        // Arrange

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                "NewPassword123!");

        // Assert

        command
            .Should()
            .BeAssignableTo<IGovernanceRequest>();
    }

    /// <summary>
    /// Verifies equality is based
    /// on property values.
    /// </summary>
    [Fact]
    public void Equality_Should_Be_Value_Based()
    {
        // Arrange

        var userId =
            Guid.NewGuid();

        var left =
            new ChangePasswordCommand(
                userId,
                "CurrentPassword123!",
                "NewPassword123!");

        var right =
            new ChangePasswordCommand(
                userId,
                "CurrentPassword123!",
                "NewPassword123!");

        // Assert

        left
            .Should()
            .Be(right);

        left.GetHashCode()
            .Should()
            .Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies commands with different
    /// values are not equal.
    /// </summary>
    [Fact]
    public void Equality_Should_Return_False_When_Values_Are_Different()
    {
        // Arrange

        var left =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                "NewPassword123!");

        var right =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "OldPassword456!",
                "AnotherPassword789!");

        // Assert

        left
            .Should()
            .NotBe(right);
    }
}