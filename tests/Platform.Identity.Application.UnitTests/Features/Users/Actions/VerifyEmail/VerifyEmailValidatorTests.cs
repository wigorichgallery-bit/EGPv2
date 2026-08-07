using FluentAssertions;
using Platform.Identity.Application.Features.Common;
using Platform.Identity.Application.Features.Users.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="VerifyEmailValidator"/>.
/// </summary>
public sealed class VerifyEmailValidatorTests
{
    private readonly VerifyEmailValidator
        _validator = new();

    /// <summary>
    /// Verifies validation succeeds when
    /// command is valid.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Success_When_Command_Is_Valid()
    {
        // Arrange

        var command =
            new VerifyEmailCommand(
                Guid.NewGuid(),
                "123456");

        // Act

        var result =
            _validator.Validate(command);

        // Assert

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
            new VerifyEmailCommand(
                Guid.Empty,
                "123456");

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors.Should()
            .ContainSingle(
                e =>
                    e.Code ==
                    "IDENTITY.USER_ID_REQUIRED");
    }

    /// <summary>
    /// Verifies validation fails when
    /// verification code is empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_Should_Return_Error_When_VerificationCode_Is_Empty(
        string? code)
    {
        var command =
            new VerifyEmailCommand(
                Guid.NewGuid(),
                code!);

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors.Should()
            .ContainSingle(
                e =>
                    e.Code ==
                    "IDENTITY.VERIFICATION_CODE_REQUIRED");
    }

    /// <summary>
    /// Verifies validation fails when
    /// verification code exceeds maximum length.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_VerificationCode_Is_Too_Long()
    {
        var command =
            new VerifyEmailCommand(
                Guid.NewGuid(),
                new string(
                    '1',
                    ValidationConstants.VerificationCodeMaxLength + 1));

        var result =
            _validator.Validate(command);

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors.Should()
            .ContainSingle(
                e =>
                    e.Code ==
                    "IDENTITY.VERIFICATION_CODE_TOO_LONG");
    }

    /// <summary>
    /// Verifies validation throws when
    /// command is null.
    /// </summary>
    [Fact]
    public void Validate_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        Action act =
            () => _validator.Validate(
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("command");
    }
}