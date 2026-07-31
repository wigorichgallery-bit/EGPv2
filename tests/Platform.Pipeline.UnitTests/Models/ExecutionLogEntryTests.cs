// ===========================================
// File Location:
// tests/Platform.Pipeline.UnitTests/
// Models/ExecutionLogEntryTests.cs
// ===========================================

using FluentAssertions;
using Platform.Pipeline.Models;
using Xunit;

namespace Platform.Pipeline.UnitTests.Models;

/// <summary>
/// Contains unit tests for <see cref="ExecutionLogEntry"/>.
/// </summary>
public sealed class ExecutionLogEntryTests
{
    /// <summary>
    /// Verifies that all constructor arguments are correctly
    /// assigned to their corresponding properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetAllProperties()
    {
        // Arrange
        const string requestName = "CreateUserCommand";
        const bool success = true;
        const string? errorCode = null;
        const string? exceptionType = null;
        const long durationMs = 125;
        var timestampUtc = DateTime.UtcNow;

        // Act
        var entry = new ExecutionLogEntry(
            requestName,
            success,
            errorCode,
            exceptionType,
            durationMs,
            timestampUtc);

        // Assert
        entry.RequestName.Should().Be(requestName);
        entry.Success.Should().Be(success);
        entry.ErrorCode.Should().Be(errorCode);
        entry.ExceptionType.Should().Be(exceptionType);
        entry.DurationMs.Should().Be(durationMs);
        entry.TimestampUtc.Should().Be(timestampUtc);
    }

    /// <summary>
    /// Verifies that two records with identical values
    /// are considered equal.
    /// </summary>
    [Fact]
    public void Equality_Should_ReturnTrue_WhenValuesAreIdentical()
    {
        // Arrange
        var timestampUtc = DateTime.UtcNow;

        var first = new ExecutionLogEntry(
            "CreateUserCommand",
            true,
            null,
            null,
            100,
            timestampUtc);

        var second = new ExecutionLogEntry(
            "CreateUserCommand",
            true,
            null,
            null,
            100,
            timestampUtc);

        // Act & Assert
        first.Should().Be(second);
        first.GetHashCode().Should().Be(second.GetHashCode());
    }

    /// <summary>
    /// Verifies that two records with different values
    /// are not considered equal.
    /// </summary>
    [Fact]
    public void Equality_Should_ReturnFalse_WhenValuesDiffer()
    {
        // Arrange
        var timestampUtc = DateTime.UtcNow;

        var first = new ExecutionLogEntry(
            "CreateUserCommand",
            true,
            null,
            null,
            100,
            timestampUtc);

        var second = new ExecutionLogEntry(
            "DeleteUserCommand",
            false,
            "VALIDATION.FAILED",
            "ArgumentException",
            250,
            timestampUtc.AddMilliseconds(1));

        // Act & Assert
        first.Should().NotBe(second);
    }

    /// <summary>
    /// Verifies that nullable properties can contain values.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetNullableProperties_WhenValuesAreProvided()
    {
        // Arrange
        const string errorCode = "VALIDATION.FAILED";
        const string exceptionType = "ArgumentException";

        // Act
        var entry = new ExecutionLogEntry(
            "CreateUserCommand",
            false,
            errorCode,
            exceptionType,
            350,
            DateTime.UtcNow);

        // Assert
        entry.ErrorCode.Should().Be(errorCode);
        entry.ExceptionType.Should().Be(exceptionType);
    }

    /// <summary>
    /// Verifies that nullable properties can be null.
    /// </summary>
    [Fact]
    public void Constructor_Should_AllowNullNullableProperties()
    {
        // Arrange

        // Act
        var entry = new ExecutionLogEntry(
            "CreateUserCommand",
            true,
            null,
            null,
            50,
            DateTime.UtcNow);

        // Assert
        entry.ErrorCode.Should().BeNull();
        entry.ExceptionType.Should().BeNull();
    }
}