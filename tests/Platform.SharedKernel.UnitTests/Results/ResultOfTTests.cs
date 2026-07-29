using FluentAssertions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Results;

/// <summary>
/// Contains unit tests for the generic <see cref="Result{T}"/> class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies the behavior of the generic result type, including successful
/// and failed result creation, value preservation, and argument validation.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify successful result creation with valid values.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify failed result creation with valid errors.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify null argument validation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify generic value preservation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify default value behavior for value types.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for <see cref="Result{T}"/> only.
/// </para>
/// </remarks>
/// </summary>
public sealed class ResultOfTTests
{
    #region Result<T>.Success()

    /// <summary>
    /// Verifies that <see cref="Result{T}.Success(T)"/> creates
    /// a successful result containing the supplied value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The returned result represents success.</description></item>
    /// <item><description>The supplied value is preserved.</description></item>
    /// <item><description><see cref="Result{T}.Error"/> equals <see cref="Error.None"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Success_WithValidValue_ShouldReturnSuccessfulResult()
    {
        // Arrange
        const string value = "Hello";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeSameAs(Error.None);
        result.Value.Should().Be(value);
    }

    /// <summary>
    /// Verifies that <see cref="Result{T}.Success(T)"/> throws
    /// an <see cref="ArgumentNullException"/> when the supplied
    /// value is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentNullException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>value</c> parameter.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Success_WithNullValue_ShouldThrowArgumentNullException()
    {
        // Arrange

        // Act
        var action = () => Result<string>.Success(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("value");
    }

    #endregion

    #region Result<T>.Failure()

    /// <summary>
    /// Verifies that <see cref="Result{T}.Failure(Error)"/> creates
    /// a failed result containing the supplied error.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The returned result represents failure.</description></item>
    /// <item><description>The supplied error instance is preserved.</description></item>
    /// <item><description>The generic value is <see langword="null"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Failure_WithValidError_ShouldReturnFailureResult()
    {
        // Arrange
        var error = new Error(
            "VALIDATION",
            "Validation failed.",
            ErrorType.Validation);

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeSameAs(error);
        result.Value.Should().BeNull();
    }

    /// <summary>
    /// Verifies that <see cref="Result{T}.Failure(Error)"/> throws
    /// an <see cref="ArgumentNullException"/> when the supplied
    /// error is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentNullException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>error</c> parameter.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Failure_WithNullError_ShouldThrowArgumentNullException()
    {
        // Arrange

        // Act
        var action = () => Result<string>.Failure(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("error");
    }

    /// <summary>
    /// Verifies that <see cref="Result{T}.Failure(Error)"/>
    /// returns the default value of the generic type when
    /// <typeparamref name="T"/> is a value type.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The generic value equals <c>default(T)</c>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Failure_ForValueType_ShouldReturnDefaultValue()
    {
        // Arrange
        var error = new Error(
            "ERROR",
            "Operation failed.");

        // Act
        var result = Result<int>.Failure(error);

        // Assert
        result.Value.Should().Be(default(int));
    }

    #endregion

    #region Value Preservation

    /// <summary>
    /// Verifies that a successful generic result preserves the supplied
    /// value without modification.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The stored value equals the supplied value.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Success_ShouldPreserveValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result<int>.Success(value);

        // Assert
        result.Value.Should().Be(42);
    }

    #endregion
}