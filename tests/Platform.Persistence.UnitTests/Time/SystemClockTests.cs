// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Time/SystemClockTests.cs
// ===========================================

using Platform.Persistence.Time;
using Platform.SharedKernel.Abstractions;

namespace Platform.Persistence.UnitTests.Time;

public sealed class SystemClockTests
{
    // ============================================================
    // IClock IMPLEMENTATION
    // ============================================================

    [Fact]
    public void SystemClock_ShouldImplementIClock()
    {
        // Arrange
        var clock =
            new SystemClock();

        // Act
        var result =
            clock is IClock;

        // Assert
        result
            .Should()
            .BeTrue();
    }

    // ============================================================
    // UTC NOW
    // ============================================================

    [Fact]
    public void UtcNow_ShouldReturnUtcDateTime()
    {
        // Arrange
        var clock =
            new SystemClock();

        // Act
        var result =
            clock.UtcNow;

        // Assert
        result.Kind
            .Should()
            .Be(DateTimeKind.Utc);
    }

    [Fact]
    public void UtcNow_ShouldBeCloseToCurrentUtcTime()
    {
        // Arrange
        var clock =
            new SystemClock();

        var before =
            DateTime.UtcNow;

        // Act
        var result =
            clock.UtcNow;

        var after =
            DateTime.UtcNow;

        // Assert
        result
            .Should()
            .BeOnOrAfter(before);

        result
            .Should()
            .BeOnOrBefore(after);
    }

    [Fact]
    public void UtcNow_ShouldNotReturnLocalTime()
    {
        // Arrange
        var clock =
            new SystemClock();

        // Act
        var result =
            clock.UtcNow;

        // Assert
        result.Kind
            .Should()
            .NotBe(DateTimeKind.Local);
    }

    // ============================================================
    // CONSECUTIVE READS
    // ============================================================

    [Fact]
    public void UtcNow_ShouldReturnCurrentTimeOnEachAccess()
    {
        // Arrange
        var clock =
            new SystemClock();

        // Act
        var first =
            clock.UtcNow;

        var second =
            clock.UtcNow;

        // Assert
        second
            .Should()
            .BeOnOrAfter(first);

        first.Kind
            .Should()
            .Be(DateTimeKind.Utc);

        second.Kind
            .Should()
            .Be(DateTimeKind.Utc);
    }

    // ============================================================
    // STATELESS BEHAVIOR
    // ============================================================

    [Fact]
    public void MultipleSystemClockInstances_ShouldReturnUtcTime()
    {
        // Arrange
        var firstClock =
            new SystemClock();

        var secondClock =
            new SystemClock();

        // Act
        var first =
            firstClock.UtcNow;

        var second =
            secondClock.UtcNow;

        // Assert
        first.Kind
            .Should()
            .Be(DateTimeKind.Utc);

        second.Kind
            .Should()
            .Be(DateTimeKind.Utc);

        second
            .Should()
            .BeOnOrAfter(first);
    }
}