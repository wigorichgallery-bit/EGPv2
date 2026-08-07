using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="VerifyEmailCommand"/>.
/// </summary>
public sealed class VerifyEmailCommandTests
{
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        var userId =
            Guid.NewGuid();

        var command =
            new VerifyEmailCommand(
                userId,
                "123456");

        command.UserId
            .Should()
            .Be(userId);

        command.VerificationCode
            .Should()
            .Be("123456");
    }

    [Fact]
    public void GovernancePolicy_Should_Return_Expected_Value()
    {
        var command =
            new VerifyEmailCommand(
                Guid.NewGuid(),
                "123456");

        command.GovernancePolicy
            .Should()
            .Be("IDENTITY.EMAIL.VERIFY");
    }

    [Fact]
    public void Resource_Should_Return_Expected_Value()
    {
        var command =
            new VerifyEmailCommand(
                Guid.NewGuid(),
                "123456");

        command.Resource
            .Should()
            .Be("User");
    }

    [Fact]
    public void Action_Should_Return_Expected_Value()
    {
        var command =
            new VerifyEmailCommand(
                Guid.NewGuid(),
                "123456");

        command.Action
            .Should()
            .Be("VerifyEmail");
    }

    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        var id =
            Guid.NewGuid();

        var left =
            new VerifyEmailCommand(
                id,
                "ABC123");

        var right =
            new VerifyEmailCommand(
                id,
                "ABC123");

        left.Should()
            .Be(right);
    }

    [Fact]
    public void Record_Should_Not_Be_Equal_When_Value_Is_Different()
    {
        var left =
            new VerifyEmailCommand(
                Guid.NewGuid(),
                "111111");

        var right =
            new VerifyEmailCommand(
                Guid.NewGuid(),
                "222222");

        left.Should()
            .NotBe(right);
    }
}