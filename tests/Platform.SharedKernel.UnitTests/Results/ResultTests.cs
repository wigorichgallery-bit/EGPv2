using FluentAssertions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Results;

/// <summary>
/// Contains unit tests for the <see cref="Result"/> class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies the behavior of the <see cref="Result"/> factory methods and
/// validates all invariants enforced by the protected constructor.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify successful result creation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify failure result creation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify constructor invariant enforcement.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify exception types and validation messages.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for <see cref="Result"/> only.
/// </para>
/// </remarks>
/// </summary>
public sealed class ResultTests
{
    #region Result Factory Methods

    /// <summary>
    /// Verifies that <see cref="Result.Success()"/> creates
    /// a successful result.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description><see cref="Result.IsSuccess"/> is <see langword="true"/>.</description></item>
    /// <item><description><see cref="Result.IsFailure"/> is <see langword="false"/>.</description></item>
    /// <item><description><see cref="Result.Error"/> equals <see cref="Error.None"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Success_ShouldReturnSuccessfulResult()
    {
        // Arrange

        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeSameAs(Error.None);
    }

    /// <summary>
    /// Verifies that <see cref="Result.Failure(Error)"/> creates
    /// a failed result containing the supplied error.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The returned result represents a failure.</description></item>
    /// <item><description>The supplied <see cref="Error"/> instance is preserved.</description></item>
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
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeSameAs(error);
    }

    /// <summary>
    /// Verifies that <see cref="Result.Failure(Error)"/> throws
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
        var action = () => Result.Failure(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("error");
    }

    #endregion

    #region Test Infrastructure

    /// <summary>
    /// Exposes the protected constructor of <see cref="Result"/>
    /// for unit testing.
    ///
    /// <remarks>
    /// This helper exists solely to verify constructor invariants and
    /// must never be used outside the unit test project.
    /// </remarks>
    /// </summary>
    private sealed class TestResult : Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestResult"/> class.
        /// </summary>
        /// <param name="isSuccess">
        /// Indicates whether the result represents a successful operation.
        /// </param>
        /// <param name="error">
        /// The error associated with the result.
        /// </param>
        public TestResult(bool isSuccess, Error error)
            : base(isSuccess, error)
        {
        }
    }

    #endregion

    #region Protected Constructor Invariants
    /// <summary>
    /// Verifies that the protected <see cref="Result"/> constructor
    /// successfully creates a valid result when the supplied arguments
    /// satisfy all constructor invariants.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The result represents a successful operation.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The associated error is <see cref="Error.None"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithSuccessAndErrorNone_ShouldCreateResult()
    {
        // Arrange

        // Act
        var result = new TestResult(true, Error.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeSameAs(Error.None);
    }

    /// <summary>
    /// Verifies that the protected <see cref="Result"/> constructor
    /// rejects a successful result containing an actual
    /// <see cref="Error"/> instance.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="InvalidOperationException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception message indicates that successful results must use
    /// <see cref="Error.None"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithSuccessAndActualError_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var error = new Error("CODE", "Message");

        // Act
        var action = () => new TestResult(true, error);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Success result must contain Error.None.*");
    }

    /// <summary>
    /// Verifies that the protected <see cref="Result"/> constructor
    /// rejects a failed result whose error is
    /// <see cref="Error.None"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="InvalidOperationException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception message indicates that failed results must contain
    /// an actual error.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithFailureAndErrorNone_ShouldThrowInvalidOperationException()
    {
        // Arrange

        // Act
        var action = () => new TestResult(false, Error.None);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Failure result must contain actual error.*");
    }

    /// <summary>
    /// Verifies that the protected <see cref="Result"/> constructor
    /// throws an <see cref="ArgumentNullException"/> when the supplied
    /// error instance is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentNullException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception identifies the <c>error</c> parameter.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithNullError_ShouldThrowArgumentNullException()
    {
        // Arrange

        // Act
        var action = () => new TestResult(true, null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("error");
    }

    #endregion
}