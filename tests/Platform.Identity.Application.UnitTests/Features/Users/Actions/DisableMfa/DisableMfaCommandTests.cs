using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="DisableMfaCommand"/>.
/// </summary>
public sealed class DisableMfaCommandTests
{
    /// <summary>
    /// Verifies constructor assigns
    /// the supplied user identifier.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_UserId()
    {
        // Arrange

        var userId =
            Guid.NewGuid();

        // Act

        var command =
            new DisableMfaCommand(
                userId);

        // Assert

        command.UserId
            .Should()
            .Be(userId);
    }

    /// <summary>
    /// Verifies governance policy.
    /// </summary>
    [Fact]
    public void GovernancePolicy_Should_Return_Expected_Value()
    {
        var command =
            new DisableMfaCommand(
                Guid.NewGuid());

        command.GovernancePolicy
            .Should()
            .Be("IDENTITY.MFA.DISABLE");
    }

    /// <summary>
    /// Verifies protected resource.
    /// </summary>
    [Fact]
    public void Resource_Should_Return_Expected_Value()
    {
        var command =
            new DisableMfaCommand(
                Guid.NewGuid());

        command.Resource
            .Should()
            .Be("User");
    }

    /// <summary>
    /// Verifies requested action.
    /// </summary>
    [Fact]
    public void Action_Should_Return_Expected_Value()
    {
        var command =
            new DisableMfaCommand(
                Guid.NewGuid());

        command.Action
            .Should()
            .Be("DisableMfa");
    }

    /// <summary>
    /// Verifies record equality.
    /// </summary>
    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        // Arrange

        var userId =
            Guid.NewGuid();

        var left =
            new DisableMfaCommand(
                userId);

        var right =
            new DisableMfaCommand(
                userId);

        // Assert

        left.Should()
            .Be(right);

        left.GetHashCode()
            .Should()
            .Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies records having different
    /// values are not equal.
    /// </summary>
    [Fact]
    public void Record_Should_Not_Be_Equal_When_UserId_Is_Different()
    {
        var left =
            new DisableMfaCommand(
                Guid.NewGuid());

        var right =
            new DisableMfaCommand(
                Guid.NewGuid());

        left.Should()
            .NotBe(right);
    }
}