using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="DisableMfaValidator"/>.
/// </summary>
public sealed class DisableMfaValidatorTests
{
    private readonly DisableMfaValidator
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
            new DisableMfaCommand(
                Guid.NewGuid());

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
            new DisableMfaCommand(
                Guid.Empty);

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
                x =>
                    x.Code ==
                    "IDENTITY.USER_ID_REQUIRED");

        result.Errors.Single()
            .Message
            .Should()
            .Be("User identifier is required.");
    }

    /// <summary>
    /// Verifies validation throws when
    /// command is null.
    /// </summary>
    [Fact]
    public void Validate_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        // Act

        Action action =
            () => _validator.Validate(
                null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("command");
    }
}