using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="EnableMfaCommand"/>.
/// </summary>
public sealed class EnableMfaCommandTests
{
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange

        var userId =
            Guid.NewGuid();

        // Act

        var command =
            new EnableMfaCommand(
                userId,
                MFAMethod.Email);

        // Assert

        command.UserId
            .Should()
            .Be(userId);

        command.Method
            .Should()
            .Be(MFAMethod.Email);
    }

    [Fact]
    public void GovernancePolicy_Should_Return_Expected_Value()
    {
        var command =
            new EnableMfaCommand(
                Guid.NewGuid(),
                MFAMethod.Email);

        command.GovernancePolicy
            .Should()
            .Be("IDENTITY.MFA.ENABLE");
    }

    [Fact]
    public void Resource_Should_Return_Expected_Value()
    {
        var command =
            new EnableMfaCommand(
                Guid.NewGuid(),
                MFAMethod.Email);

        command.Resource
            .Should()
            .Be("User");
    }

    [Fact]
    public void Action_Should_Return_Expected_Value()
    {
        var command =
            new EnableMfaCommand(
                Guid.NewGuid(),
                MFAMethod.Email);

        command.Action
            .Should()
            .Be("EnableMfa");
    }

    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        var id =
            Guid.NewGuid();

        var left =
            new EnableMfaCommand(
                id,
                MFAMethod.Email);

        var right =
            new EnableMfaCommand(
                id,
                MFAMethod.Email);

        left.Should()
            .Be(right);
    }

    [Fact]
    public void Record_Should_Not_Be_Equal_When_Value_Is_Different()
    {
        var left =
            new EnableMfaCommand(
                Guid.NewGuid(),
                MFAMethod.Email);

        var right =
            new EnableMfaCommand(
                Guid.NewGuid(),
                MFAMethod.SMS);

        left.Should()
            .NotBe(right);
    }
}