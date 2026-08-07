using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="EnableMfaValidator"/>.
/// </summary>
public sealed class EnableMfaValidatorTests
{
    private readonly EnableMfaValidator
        _validator = new();

    /// <summary>
    /// Verifies validation succeeds when
    /// command is valid.
    /// </summary>
    [Theory]
    [InlineData(MFAMethod.Email)]
    [InlineData(MFAMethod.SMS)]
    [InlineData(MFAMethod.WhatsApp)]
    [InlineData(MFAMethod.TOTP)]
    public void Validate_Should_Return_Success_When_Command_Is_Valid(
        MFAMethod method)
    {
        // Arrange

        var command =
            new EnableMfaCommand(
                Guid.NewGuid(),
                method);

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
        // Arrange

        var command =
            new EnableMfaCommand(
                Guid.Empty,
                MFAMethod.Email);

        // Act

        var result =
            _validator.Validate(command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(
                e =>
                    e.Code ==
                    "IDENTITY.USER_ID_REQUIRED");
    }

    /// <summary>
    /// Verifies validation fails when
    /// MFA method is undefined.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Method_Is_Invalid()
    {
        // Arrange

        var command =
            new EnableMfaCommand(
                Guid.NewGuid(),
                (MFAMethod)999);

        // Act

        var result =
            _validator.Validate(command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(
                e =>
                    e.Code ==
                    "IDENTITY.INVALID_MFA_METHOD");
    }

    /// <summary>
    /// Verifies validation fails when
    /// MFA method is None.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Method_Is_None()
    {
        // Arrange

        var command =
            new EnableMfaCommand(
                Guid.NewGuid(),
                MFAMethod.None);

        // Act

        var result =
            _validator.Validate(command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(
                e =>
                    e.Code ==
                    "IDENTITY.INVALID_MFA_METHOD");
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