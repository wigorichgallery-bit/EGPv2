// ===========================================
// File Location:
// tests/Application/Platform.Pipeline.UnitTests/
// Validation/ValidationFailureFactoryTests.cs
// ===========================================

using FluentAssertions;
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Validation;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Pipeline.UnitTests.Validation;

/// <summary>
/// Contains unit tests for <see cref="ValidationFailureFactory"/>.
/// </summary>
public sealed class ValidationFailureFactoryTests
{
    /// <summary>
    /// Verifies that <see cref="ValidationFailureFactory.CreateFailure(IReadOnlyCollection{ValidationError})"/>
    /// returns a failed result when a single validation error is supplied.
    /// </summary>
    [Fact]
    public void CreateFailure_Should_ReturnFailureResult_WhenSingleErrorIsProvided()
    {
        // Arrange
        IReadOnlyCollection<ValidationError> errors =
        [
            new(
                "VALIDATION.REQUIRED",
                "Name is required.")
        ];

        // Act
        Result result = ValidationFailureFactory.CreateFailure(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("VALIDATION.FAILED");
        result.Error.Message.Should().Be("Name is required.");
    }

    /// <summary>
    /// Verifies that validation messages are concatenated
    /// using "; " while preserving their original order.
    /// </summary>
    [Fact]
    public void CreateFailure_Should_CombineMessages_WhenMultipleErrorsAreProvided()
    {
        // Arrange
        IReadOnlyCollection<ValidationError> errors =
        [
            new(
                "CODE1",
                "Name is required."),
            new(
                "CODE2",
                "Email is invalid."),
            new(
                "CODE3",
                "Password is too short.")
        ];

        // Act
        Result result = ValidationFailureFactory.CreateFailure(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("VALIDATION.FAILED");
        result.Error.Message.Should().Be(
            "Name is required.; Email is invalid.; Password is too short.");
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentNullException"/>
    /// is thrown when the validation error collection is null.
    /// </summary>
    [Fact]
    public void CreateFailure_Should_Throw_WhenErrorsAreNull()
    {
        // Arrange
        IReadOnlyCollection<ValidationError>? errors = null;

        // Act
        Action act = () =>
            ValidationFailureFactory.CreateFailure(errors!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/>
    /// is thrown when the validation error collection is empty.
    /// </summary>
    [Fact]
    public void CreateFailure_Should_Throw_WhenErrorsAreEmpty()
    {
        // Arrange
        IReadOnlyCollection<ValidationError> errors =
            Array.Empty<ValidationError>();

        // Act
        Action act = () =>
            ValidationFailureFactory.CreateFailure(errors);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errors");
    }

    /// <summary>
    /// Verifies that the generic overload returns
    /// a failed result containing the aggregated validation error.
    /// </summary>
    [Fact]
    public void CreateFailure_Generic_Should_ReturnFailureResult()
    {
        // Arrange
        IReadOnlyCollection<ValidationError> errors =
        [
            new(
                "VALIDATION.REQUIRED",
                "Email is required.")
        ];

        // Act
        Result<int> result =
            ValidationFailureFactory.CreateFailure<int>(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("VALIDATION.FAILED");
        result.Error.Message.Should().Be("Email is required.");
    }

    /// <summary>
    /// Verifies that the generic overload throws an
    /// <see cref="ArgumentNullException"/> when the
    /// validation error collection is null.
    /// </summary>
    [Fact]
    public void CreateFailure_Generic_Should_Throw_WhenErrorsAreNull()
    {
        // Arrange
        IReadOnlyCollection<ValidationError>? errors = null;

        // Act
        Action act = () =>
            ValidationFailureFactory.CreateFailure<int>(errors!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that the generic overload throws an
    /// <see cref="ArgumentException"/> when the
    /// validation error collection is empty.
    /// </summary>
    [Fact]
    public void CreateFailure_Generic_Should_Throw_WhenErrorsAreEmpty()
    {
        // Arrange
        IReadOnlyCollection<ValidationError> errors =
            Array.Empty<ValidationError>();

        // Act
        Action act = () =>
            ValidationFailureFactory.CreateFailure<int>(errors);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errors");
    }
}