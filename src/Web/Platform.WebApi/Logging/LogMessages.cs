// ===========================================
// File Location :
// src/Web/Platform.WebApi/Logging/
// LogMessages.cs
// ===========================================
namespace Platform.WebApi.Logging;

/// <summary>
/// Provides centralized structured logging
/// message templates used throughout
/// Platform.WebApi.
///
/// Responsibility:
/// - Centralize logging message templates.
/// - Prevent duplicated log messages.
/// - Ensure structured logging consistency.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
internal static class LogMessages
{
    /// <summary>
    /// Request completed message.
    /// </summary>
    public const string RequestCompleted =
        "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms. CorrelationId: {CorrelationId}";

    /// <summary>
    /// Unhandled exception message.
    /// </summary>
    public const string UnhandledException =
        "Unhandled exception occurred while processing {Method} {Path}. ExceptionType: {ExceptionType}";
}