using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Actions;
using Platform.Identity.Application.Features.Common;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Actions;

/// <summary>
/// Unit tests for <see cref="LoginValidator"/>.
/// </summary>
public sealed class LoginValidatorTests
{
    private readonly LoginValidator _sut = new();

    /// <summary>
    /// Verifies a null command throws an exception.
    /// </summary>
    [Fact]
    public void Validate_Should_Throw_When_Command_Is_Null()
    {
        // Act
        Action act = () => _sut.Validate(null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("command");
    }

    /// <summary>
    /// Verifies validation succeeds for a valid command.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Success_When_Command_Is_Valid()
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            "Password123");

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies identity is required.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_Should_Fail_When_Identity_Is_Null_Empty_Or_Whitespace(
        string? identity)
    {
        // Arrange
        var command = new LoginCommand(
            identity!,
            "Password123");

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle(x =>
                x.Code == "IDENTITY.IDENTITY_REQUIRED" &&
                x.Message == "Identity is required.");
    }

    /// <summary>
    /// Verifies identity exceeding the maximum length is rejected.
    /// </summary>
    [Fact]
    public void Validate_Should_Fail_When_Identity_Exceeds_Maximum_Length()
    {
        // Arrange
        var command = new LoginCommand(
            new string(
                'A',
                ValidationConstants.MaximumIdentityLength + 1),
            "Password123");

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle(x =>
                x.Code == "IDENTITY.IDENTITY_TOO_LONG");
    }

    /// <summary>
    /// Verifies password is required.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_Should_Fail_When_Password_Is_Null_Empty_Or_Whitespace(
        string? password)
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            password!);

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle(x =>
                x.Code == "IDENTITY.PASSWORD_REQUIRED" &&
                x.Message == "Password is required.");
    }

    /// <summary>
    /// Verifies passwords shorter than the minimum length are rejected.
    /// </summary>
    [Fact]
    public void Validate_Should_Fail_When_Password_Is_Shorter_Than_Minimum_Length()
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            new string(
                'P',
                ValidationConstants.PasswordMinLength - 1));

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle(x =>
                x.Code == "IDENTITY.PASSWORD_TOO_SHORT");
    }

    /// <summary>
    /// Verifies passwords exceeding the maximum length are rejected.
    /// </summary>
    [Fact]
    public void Validate_Should_Fail_When_Password_Exceeds_Maximum_Length()
    {
        // Arrange
        var command = new LoginCommand(
            "john.doe",
            new string(
                'P',
                ValidationConstants.MaximumPasswordLength + 1));

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle(x =>
                x.Code == "IDENTITY.PASSWORD_TOO_LONG");
    }

    /// <summary>
    /// Verifies multiple validation errors are returned together.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_All_Validation_Errors()
    {
        // Arrange
        var command = new LoginCommand(
            string.Empty,
            string.Empty);

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors.Should().HaveCount(2);

        result.Errors.Should()
            .Contain(x =>
                x.Code == "IDENTITY.IDENTITY_REQUIRED");

        result.Errors.Should()
            .Contain(x =>
                x.Code == "IDENTITY.PASSWORD_REQUIRED");
    }
}