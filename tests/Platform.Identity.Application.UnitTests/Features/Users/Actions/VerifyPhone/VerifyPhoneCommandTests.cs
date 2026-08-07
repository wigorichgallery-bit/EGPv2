using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="VerifyPhoneCommand"/>.
/// </summary>
public sealed class VerifyPhoneCommandTests
{
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        var userId =
            Guid.NewGuid();

        var command =
            new VerifyPhoneCommand(
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
            new VerifyPhoneCommand(
                Guid.NewGuid(),
                "123456");

        command.GovernancePolicy
            .Should()
            .Be("IDENTITY.PHONE.VERIFY");
    }

    [Fact]
    public void Resource_Should_Return_Expected_Value()
    {
        var command =
            new VerifyPhoneCommand(
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
            new VerifyPhoneCommand(
                Guid.NewGuid(),
                "123456");

        command.Action
            .Should()
            .Be("VerifyPhone");
    }

    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        var id =
            Guid.NewGuid();

        var left =
            new VerifyPhoneCommand(
                id,
                "ABC123");

        var right =
            new VerifyPhoneCommand(
                id,
                "ABC123");

        left.Should()
            .Be(right);
    }

    [Fact]
    public void Record_Should_Not_Be_Equal_When_Value_Is_Different()
    {
        var left =
            new VerifyPhoneCommand(
                Guid.NewGuid(),
                "111111");

        var right =
            new VerifyPhoneCommand(
                Guid.NewGuid(),
                "222222");

        left.Should()
            .NotBe(right);
    }
}