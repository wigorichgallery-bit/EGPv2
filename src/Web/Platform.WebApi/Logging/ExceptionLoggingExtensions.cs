// ===========================================
// File Location :
// src/Web/Platform.WebApi/Logging/
// ExceptionLoggingExtensions.cs
// ===========================================
//
// Reason:
// High-performance exception logging using
// LoggerMessage source generator.
//
// Affected Module:
// Platform.WebApi
//
// Breaking Change:
// NO
//
// Version:
// 1.0.0
// ===========================================
namespace Platform.WebApi.Logging;

/// <summary>
/// Provides high-performance structured
/// exception logging extensions.
///
/// Responsibility:
/// - Centralize exception logging.
/// - Eliminate logging allocations.
/// - Ensure logging consistency.
///
/// Architectural Rules:
/// - No business logic.
/// - No persistence logic.
/// - No infrastructure dependency.
///
/// Thread Safety:
/// - Stateless.
/// </summary>
internal static partial class ExceptionLoggingExtensions
{
    // Must match LogEvents.UnhandledException
    private const int UnhandledExceptionEventId =
        1002;

    /// <summary>
    /// Writes an unhandled exception log entry.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="logger">
    /// Logger instance.
    /// </param>
    /// <param name="exception">
    /// Exception instance.
    /// </param>
    /// <param name="method">
    /// HTTP method.
    /// </param>
    /// <param name="path">
    /// Request path.
    /// </param>
    /// <param name="exceptionType">
    /// Exception type.
    /// </param>
    [LoggerMessage(
        EventId = UnhandledExceptionEventId,
        Level = LogLevel.Error,
        Message =
            LogMessages.UnhandledException)]
    internal static partial void UnhandledException(
        this ILogger logger,
        Exception exception,
        string method,
        string path,
        string exceptionType);
}