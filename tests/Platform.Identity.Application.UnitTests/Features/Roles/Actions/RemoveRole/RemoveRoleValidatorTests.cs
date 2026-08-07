using FluentAssertions;
using Platform.Identity.Application.Features.Roles.Actions;
using Platform.Pipeline.Abstractions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Roles.Actions;

/// <summary>
/// Unit tests for <see cref="RemoveRoleValidator"/>.
/// </summary>
public sealed class RemoveRoleValidatorTests
{
    /// <summary>
    /// Verifies Validate throws when
    /// command is null.
    /// </summary>
    [Fact]
    public void Validate_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        // Arrange

        var validator =
            new RemoveRoleValidator();

        // Act

        Action act =
            () => validator.Validate(null!);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("command");
    }

    /// <summary>
    /// Verifies validation succeeds when
    /// command is valid.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Success_When_Command_Is_Valid()
    {
        // Arrange

        var validator =
            new RemoveRoleValidator();

        var command =
            new RemoveRoleCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

        // Act

        ValidationResult result =
            validator.Validate(command);

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
    public void Validate_Should_Return_Failure_When_UserId_Is_Empty()
    {
        // Arrange

        var validator =
            new RemoveRoleValidator();

        var command =
            new RemoveRoleCommand(
                Guid.Empty,
                Guid.NewGuid());

        // Act

        ValidationResult result =
            validator.Validate(command);

        // Assert

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
    /// role identifier is empty.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Failure_When_RoleId_Is_Empty()
    {
        // Arrange

        var validator =
            new RemoveRoleValidator();

        var command =
            new RemoveRoleCommand(
                Guid.NewGuid(),
                Guid.Empty);

        // Act

        ValidationResult result =
            validator.Validate(command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(x =>
                x.Code == "IDENTITY.ROLE_ID_REQUIRED");
    }

    /// <summary>
    /// Verifies validation returns both
    /// errors when identifiers are empty.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Two_Errors_When_Both_Identifiers_Are_Empty()
    {
        // Arrange

        var validator =
            new RemoveRoleValidator();

        var command =
            new RemoveRoleCommand(
                Guid.Empty,
                Guid.Empty);

        // Act

        ValidationResult result =
            validator.Validate(command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .HaveCount(2);

        result.Errors
            .Should()
            .Contain(x =>
                x.Code == "IDENTITY.USER_ID_REQUIRED");

        result.Errors
            .Should()
            .Contain(x =>
                x.Code == "IDENTITY.ROLE_ID_REQUIRED");
    }
}