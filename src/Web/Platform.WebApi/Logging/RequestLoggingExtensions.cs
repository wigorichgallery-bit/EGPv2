// ===========================================
// File Location :
// src/Web/Platform.WebApi/Logging/
// RequestLoggingExtensions.cs
// ===========================================
namespace Platform.WebApi.Logging;

/// <summary>
/// Provides high-performance structured
/// request logging extensions.
///
/// Responsibility:
/// - Centralize request logging.
/// - Eliminate logging allocations.
/// - Ensure logging consistency.
///
/// Thread Safety:
/// - Stateless.
/// </summary>
internal static partial class RequestLoggingExtensions
{
    // Must match LogEvents.RequestCompletedInformation
    private const int RequestCompletedInformationEventId =
        1001;

    // Must match LogEvents.RequestCompletedWarning
    private const int RequestCompletedWarningEventId =
        1005;

    // Must match LogEvents.RequestCompletedError
    private const int RequestCompletedErrorEventId =
        1006;

    /// <summary>
    /// Writes an informational request log.
    /// </summary>
    [LoggerMessage(
        EventId = RequestCompletedInformationEventId,
        Level = LogLevel.Information,
        Message = LogMessages.RequestCompleted)]
    internal static partial void RequestCompletedInformation(
        this ILogger logger,
        string method,
        string path,
        int statusCode,
        long elapsedMilliseconds,
        string correlationId);

    /// <summary>
    /// Writes a warning request log.
    /// </summary>
    [LoggerMessage(
        EventId = RequestCompletedWarningEventId,
        Level = LogLevel.Warning,
        Message = LogMessages.RequestCompleted)]
    internal static partial void RequestCompletedWarning(
        this ILogger logger,
        string method,
        string path,
        int statusCode,
        long elapsedMilliseconds,
        string correlationId);

    /// <summary>
    /// Writes an error request log.
    /// </summary>
    [LoggerMessage(
        EventId = RequestCompletedErrorEventId,
        Level = LogLevel.Error,
        Message = LogMessages.RequestCompleted)]
    internal static partial void RequestCompletedError(
        this ILogger logger,
        string method,
        string path,
        int statusCode,
        long elapsedMilliseconds,
        string correlationId);
}