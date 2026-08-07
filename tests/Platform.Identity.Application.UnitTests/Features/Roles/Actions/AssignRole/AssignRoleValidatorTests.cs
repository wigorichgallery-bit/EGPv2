using FluentAssertions;
using Platform.Identity.Application.Features.Roles.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Roles.Actions;

/// <summary>
/// Unit tests for <see cref="AssignRoleValidator"/>.
/// </summary>
public sealed class AssignRoleValidatorTests
{
    private readonly AssignRoleValidator
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
            new AssignRoleCommand(
                Guid.NewGuid(),
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
    public void Validate_Should_Return_Failure_When_UserId_Is_Empty()
    {
        // Arrange

        var command =
            new AssignRoleCommand(
                Guid.Empty,
                Guid.NewGuid());

        // Act

        var result =
            _validator.Validate(command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(x =>
                x.Code == "IDENTITY.USER_ID_REQUIRED"
                && x.Message == "User identifier is required.");
    }

    /// <summary>
    /// Verifies validation fails when
    /// role identifier is empty.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Failure_When_RoleId_Is_Empty()
    {
        // Arrange

        var command =
            new AssignRoleCommand(
                Guid.NewGuid(),
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
            .ContainSingle(x =>
                x.Code == "IDENTITY.ROLE_ID_REQUIRED"
                && x.Message == "Role identifier is required.");
    }

    /// <summary>
    /// Verifies validation returns both
    /// errors when identifiers are empty.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Two_Errors_When_Both_Identifiers_Are_Empty()
    {
        // Arrange

        var command =
            new AssignRoleCommand(
                Guid.Empty,
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

    /// <summary>
    /// Verifies validation throws when
    /// command is null.
    /// </summary>
    [Fact]
    public void Validate_Should_Throw_When_Command_Is_Null()
    {
        // Arrange

        AssignRoleCommand command = null!;

        // Act

        FluentActions
            .Invoking(() =>
                _validator.Validate(command))
            .Should()
            .Throw<ArgumentNullException>();
    }
}