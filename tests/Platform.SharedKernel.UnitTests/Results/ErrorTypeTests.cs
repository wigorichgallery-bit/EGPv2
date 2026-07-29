using FluentAssertions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Results;

/// <summary>
/// Contains unit tests for the <see cref="ErrorType"/> enumeration.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies that every <see cref="ErrorType"/> member retains its expected
/// numeric value.
/// </para>
///
/// <para>
/// These tests protect the enum contract against accidental value changes
/// that could introduce breaking changes in serialization, persistence,
/// logging, or external integrations.
/// </para>
///
/// <para>
/// Scope:
/// Unit tests for the <see cref="ErrorType"/> enumeration only.
/// </para>
/// </remarks>
/// </summary>
public sealed class ErrorTypeTests
{
    #region ErrorType Values

    /// <summary>
    /// Verifies that <see cref="ErrorType.None"/> has the expected numeric value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <see cref="ErrorType.None"/> equals <c>0</c>.
    /// </remarks>
    [Fact]
    public void None_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act

        // Assert
        ((int)ErrorType.None).Should().Be(0);
    }

    /// <summary>
    /// Verifies that <see cref="ErrorType.Validation"/> has the expected numeric value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <see cref="ErrorType.Validation"/> equals <c>1</c>.
    /// </remarks>
    [Fact]
    public void Validation_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act

        // Assert
        ((int)ErrorType.Validation).Should().Be(1);
    }

    /// <summary>
    /// Verifies that <see cref="ErrorType.Unauthorized"/> has the expected numeric value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <see cref="ErrorType.Unauthorized"/> equals <c>2</c>.
    /// </remarks>
    [Fact]
    public void Unauthorized_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act

        // Assert
        ((int)ErrorType.Unauthorized).Should().Be(2);
    }

    /// <summary>
    /// Verifies that <see cref="ErrorType.Forbidden"/> has the expected numeric value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <see cref="ErrorType.Forbidden"/> equals <c>3</c>.
    /// </remarks>
    [Fact]
    public void Forbidden_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act

        // Assert
        ((int)ErrorType.Forbidden).Should().Be(3);
    }

    /// <summary>
    /// Verifies that <see cref="ErrorType.NotFound"/> has the expected numeric value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <see cref="ErrorType.NotFound"/> equals <c>4</c>.
    /// </remarks>
    [Fact]
    public void NotFound_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act

        // Assert
        ((int)ErrorType.NotFound).Should().Be(4);
    }

    /// <summary>
    /// Verifies that <see cref="ErrorType.Conflict"/> has the expected numeric value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <see cref="ErrorType.Conflict"/> equals <c>5</c>.
    /// </remarks>
    [Fact]
    public void Conflict_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act

        // Assert
        ((int)ErrorType.Conflict).Should().Be(5);
    }

    /// <summary>
    /// Verifies that <see cref="ErrorType.Internal"/> has the expected numeric value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <see cref="ErrorType.Internal"/> equals <c>6</c>.
    /// </remarks>
    [Fact]
    public void Internal_ShouldHaveExpectedValue()
    {
        // Arrange

        // Act

        // Assert
        ((int)ErrorType.Internal).Should().Be(6);
    }

    #endregion
}