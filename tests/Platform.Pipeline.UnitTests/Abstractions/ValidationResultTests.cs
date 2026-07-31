// ===========================================
// File Location:
// tests/Platform.Pipeline.UnitTests/
// Abstractions/ValidationResultTests.cs
// ===========================================

using FluentAssertions;
using Platform.Pipeline.Abstractions;
using Xunit;

namespace Platform.Pipeline.UnitTests.Abstractions;

/// <summary>
/// Contains unit tests for <see cref="ValidationResult"/>.
/// </summary>
public sealed class ValidationResultTests
{
    /// <summary>
    /// Verifies that <see cref="ValidationResult.Success"/>
    /// returns a valid result with no validation errors.
    /// </summary>
    [Fact]
    public void Success_Should_ReturnValidResult()
    {
        // Arrange

        // Act
        var result = ValidationResult.Success();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that <see cref="ValidationResult.Failure"/>
    /// returns an invalid result containing the supplied
    /// validation errors.
    /// </summary>
    [Fact]
    public void Failure_Should_ReturnInvalidResult_WhenSingleErrorIsProvided()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError(
                "VALIDATION.REQUIRED",
                "Validation failed.")
        };

        // Act
        var result = ValidationResult.Failure(errors);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().ContainSingle();
        result.Errors.Single().Code.Should().Be("VALIDATION.REQUIRED");
        result.Errors.Single().Message.Should().Be("Validation failed.");
    }

    /// <summary>
    /// Verifies that <see cref="ValidationResult.Failure"/>
    /// preserves all supplied validation errors.
    /// </summary>
    [Fact]
    public void Failure_Should_ReturnInvalidResult_WhenMultipleErrorsAreProvided()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError(
                "CODE1",
                "Message 1"),
            new ValidationError(
                "CODE2",
                "Message 2")
        };

        // Act
        var result = ValidationResult.Failure(errors);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().BeEquivalentTo(errors);
    }

    /// <summary>
    /// Verifies that <see cref="ValidationResult.Failure"/>
    /// throws an exception when the error collection is null.
    /// </summary>
    [Fact]
    public void Failure_Should_Throw_WhenErrorsAreNull()
    {
        // Arrange
        IReadOnlyCollection<ValidationError>? errors = null;

        // Act
        Action act = () => ValidationResult.Failure(errors!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that <see cref="ValidationResult.Failure"/>
    /// throws an exception when the error collection is empty.
    /// </summary>
    [Fact]
    public void Failure_Should_Throw_WhenErrorsAreEmpty()
    {
        // Arrange
        var errors = Array.Empty<ValidationError>();

        // Act
        Action act = () => ValidationResult.Failure(errors);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errors");
    }

    /// <summary>
    /// Verifies that the returned error collection is the
    /// same instance supplied to the failure factory.
    /// </summary>
    [Fact]
    public void Failure_Should_PreserveOriginalErrorCollection()
    {
        // Arrange
        IReadOnlyCollection<ValidationError> errors =
        [
            new ValidationError(
                "CODE",
                "Message")
        ];

        // Act
        var result = ValidationResult.Failure(errors);

        // Assert
        result.Errors.Should().BeSameAs(errors);
    }
}