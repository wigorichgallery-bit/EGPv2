using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="ChangePasswordValidator"/>.
/// </summary>
public sealed class ChangePasswordValidatorTests
{
    private readonly ChangePasswordValidator
        _validator = new();

    /// <summary>
    /// Verifies Validate throws when
    /// command is null.
    /// </summary>
    [Fact]
    public void Validate_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        FluentActions
            .Invoking(() =>
                _validator.Validate(
                    null!))
            .Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("command");
    }

    /// <summary>
    /// Verifies validation succeeds
    /// for a valid command.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Success_When_Command_Is_Valid()
    {
        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                "NewPassword123!");

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeTrue();

        result.Errors
            .Should()
            .BeEmpty();
    }

    /// <summary>
    /// Verifies validation fails when
    /// user identifier is empty.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_UserId_Is_Empty()
    {
        var command =
            new ChangePasswordCommand(
                Guid.Empty,
                "CurrentPassword123!",
                "NewPassword123!");

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(x =>
                x.Code == "IDENTITY.USER_ID_REQUIRED");
    }

    /// <summary>
    /// Verifies validation fails when
    /// current password is empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_Should_Return_Error_When_CurrentPassword_Is_Invalid(
        string? currentPassword)
    {
        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                currentPassword!,
                "NewPassword123!");

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(x =>
                x.Code ==
                "IDENTITY.CURRENT_PASSWORD_REQUIRED");
    }

    /// <summary>
    /// Verifies validation fails when
    /// new password is empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_Should_Return_Error_When_NewPassword_Is_Invalid(
        string? newPassword)
    {
        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                newPassword!);

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(x =>
                x.Code ==
                "IDENTITY.NEW_PASSWORD_REQUIRED");
    }

    /// <summary>
    /// Verifies validation fails when
    /// new password is shorter than the minimum length.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_NewPassword_Is_Too_Short()
    {
        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword123!",
                "1234567");

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(x =>
                x.Code ==
                "IDENTITY.NEW_PASSWORD_TOO_SHORT");
    }

    /// <summary>
    /// Verifies validation fails when
    /// new password equals current password.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_NewPassword_Equals_CurrentPassword()
    {
        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "Password123!",
                "Password123!");

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(x =>
                x.Code ==
                "IDENTITY.PASSWORD_MUST_CHANGE");
    }

    /// <summary>
    /// Verifies validation returns all
    /// applicable validation errors.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Multiple_Errors_When_Command_Is_Invalid()
    {
        var command =
            new ChangePasswordCommand(
                Guid.Empty,
                "",
                "");

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .HaveCount(3);

        result.Errors
            .Select(x => x.Code)
            .Should()
            .Contain(new[]
            {
                "IDENTITY.USER_ID_REQUIRED",
                "IDENTITY.CURRENT_PASSWORD_REQUIRED",
                "IDENTITY.NEW_PASSWORD_REQUIRED"
            });
    }
}