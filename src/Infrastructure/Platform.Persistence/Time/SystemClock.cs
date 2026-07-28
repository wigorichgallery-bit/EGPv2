// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/Time/SystemClock.cs
// ===========================================
using Platform.SharedKernel.Abstractions;

namespace Platform.Persistence.Time;

/// <summary>
/// Provides the current UTC system time.
///
/// Responsibility:
/// - Supply current UTC time.
/// - Centralize time access.
/// - Support deterministic testing through
///   IClock abstraction.
///
/// Architectural Rules:
/// - Infrastructure implementation.
/// - Implements IClock contract.
/// - Must never return local time.
/// - Must always return UTC time.
///
/// Side Effects:
/// - Reads system clock.
///
/// Thread Safety:
/// - Stateless.
/// - Safe for concurrent usage.
///
/// Complexity:
/// O(1)
/// </summary>
public sealed class SystemClock : IClock
{
    /// <summary>
    /// Gets the current UTC time.
    ///
    /// Business Rules:
    /// - Must always return UTC time.
    /// - Must return DateTimeKind.Utc.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    public DateTime UtcNow
        => DateTime.UtcNow;
}