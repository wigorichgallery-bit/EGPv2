using FluentAssertions;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Pipeline.Abstractions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="CreateUserValidator"/>.
/// </summary>
public sealed class CreateUserValidatorTests
{
    private readonly CreateUserValidator
        _validator = new();

    /// <summary>
    /// Creates a valid command.
    /// </summary>
    private static CreateUserCommand CreateValidCommand()
    {
        return new CreateUserCommand(
            "john.doe",
            "john.doe@example.com",
            "+6281234567890",
            "Password123!");
    }

    // ============================================================
    // Null Guard
    // ============================================================

    /// <summary>
    /// Verifies Validate throws when
    /// command is null.
    /// </summary>
    [Fact]
    public void Validate_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        // Act

        Action act =
            () => _validator.Validate(
                null!);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("command");
    }

    // ============================================================
    // Success
    // ============================================================

    /// <summary>
    /// Verifies validation succeeds
    /// for a valid command.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Success_When_Command_Is_Valid()
    {
        // Arrange

        var command =
            CreateValidCommand();

        // Act

        var result =
            _validator.Validate(
                command);

        // Assert

        result.IsValid
            .Should()
            .BeTrue();

        result.Errors
            .Should()
            .BeEmpty();
    }

    // ============================================================
    // Username
    // ============================================================

    /// <summary>
    /// Verifies username is required.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Username_Is_Empty()
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                Username = string.Empty
            };

        // Act

        var result =
            _validator.Validate(
                command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .ContainSingle(
                e => e.Code ==
                     "IDENTITY.USERNAME_REQUIRED");
    }

    /// <summary>
    /// Verifies username length
    /// minimum is enforced.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Username_Is_Too_Short()
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                Username = "ab"
            };

        // Act

        var result =
            _validator.Validate(
                command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.USERNAME_TOO_SHORT");
    }

    /// <summary>
    /// Verifies username maximum
    /// length is enforced.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Username_Is_Too_Long()
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                Username =
                    new string('A', 300)
            };

        // Act

        var result =
            _validator.Validate(
                command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.USERNAME_TOO_LONG");
    }

    // ============================================================
    // Email
    // ============================================================

    /// <summary>
    /// Verifies email is required.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Email_Is_Empty()
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                Email = string.Empty
            };

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
                e => e.Code ==
                     "IDENTITY.EMAIL_REQUIRED");
    }

    /// <summary>
    /// Verifies email format is validated.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Email_Is_Invalid()
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                Email = "invalid-email"
            };

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
                e => e.Code ==
                     "IDENTITY.INVALID_EMAIL");
    }

    // ============================================================
    // Phone Number
    // ============================================================

    /// <summary>
    /// Verifies phone number is required.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_PhoneNumber_Is_Empty()
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                PhoneNumber = string.Empty
            };

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
                e => e.Code ==
                     "IDENTITY.PHONE_REQUIRED");
    }

    /// <summary>
    /// Verifies E.164 phone format is enforced.
    /// </summary>
    [Theory]
    [InlineData("08123456789")]
    [InlineData("628123456789")]
    [InlineData("+62-8123456789")]
    [InlineData("ABCDEF")]
    public void Validate_Should_Return_Error_When_PhoneNumber_Is_Invalid(
        string phoneNumber)
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                PhoneNumber = phoneNumber
            };

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
                e => e.Code ==
                     "IDENTITY.INVALID_PHONE_NUMBER");
    }

    // ============================================================
    // Password
    // ============================================================

    /// <summary>
    /// Verifies password is required.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Password_Is_Empty()
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                Password = string.Empty
            };

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
                e => e.Code ==
                     "IDENTITY.PASSWORD_REQUIRED");
    }

    /// <summary>
    /// Verifies password minimum length
    /// is enforced.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_Error_When_Password_Is_Too_Short()
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                Password = "123"
            };

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
                e => e.Code ==
                     "IDENTITY.PASSWORD_TOO_SHORT");
    }

    // ============================================================
    // Multiple Validation Errors
    // ============================================================

    /// <summary>
    /// Verifies all validation errors
    /// are returned together.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_All_ValidationErrors()
    {
        // Arrange

        var command =
            new CreateUserCommand(
                Username: string.Empty,
                Email: "invalid-email",
                PhoneNumber: "08123456789",
                Password: "123");

        // Act

        var result =
            _validator.Validate(
                command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .HaveCount(4);

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.USERNAME_REQUIRED");

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.INVALID_EMAIL");

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.INVALID_PHONE_NUMBER");

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.PASSWORD_TOO_SHORT");
    }

    /// <summary>
    /// Verifies whitespace values
    /// are treated as empty.
    /// </summary>
    [Fact]
    public void Validate_Should_Return_RequiredErrors_When_Values_Are_Whitespace()
    {
        // Arrange

        var command =
            new CreateUserCommand(
                Username: "   ",
                Email: "   ",
                PhoneNumber: "   ",
                Password: "   ");

        // Act

        var result =
            _validator.Validate(
                command);

        // Assert

        result.IsValid
            .Should()
            .BeFalse();

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.USERNAME_REQUIRED");

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.EMAIL_REQUIRED");

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.PHONE_REQUIRED");

        result.Errors
            .Should()
            .Contain(
                e => e.Code ==
                     "IDENTITY.PASSWORD_REQUIRED");
    }

    /// <summary>
    /// Verifies a valid E.164 phone number
    /// passes validation.
    /// </summary>
    [Theory]
    [InlineData("+6281234567890")]
    [InlineData("+12025550123")]
    [InlineData("+447911123456")]
    public void Validate_Should_Accept_Valid_E164_PhoneNumber(
        string phoneNumber)
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                PhoneNumber = phoneNumber
            };

        // Act

        var result =
            _validator.Validate(
                command);

        // Assert

        result.IsValid
            .Should()
            .BeTrue();

        result.Errors
            .Should()
            .BeEmpty();
    }

    /// <summary>
    /// Verifies a valid email address
    /// passes validation.
    /// </summary>
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("john.doe@example.com")]
    [InlineData("john+admin@example.co.id")]
    public void Validate_Should_Accept_Valid_Email(
        string email)
    {
        // Arrange

        var command =
            CreateValidCommand() with
            {
                Email = email
            };

        // Act

        var result =
            _validator.Validate(
                command);

        // Assert

        result.IsValid
            .Should()
            .BeTrue();

        result.Errors
            .Should()
            .BeEmpty();
    }
}