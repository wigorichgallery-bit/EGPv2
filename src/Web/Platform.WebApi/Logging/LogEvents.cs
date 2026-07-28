// ===========================================
// File Location :
// src/Web/Platform.WebApi/Logging/
// LogEvents.cs
// ===========================================
//
// Reason:
// Centralize logging event identifiers.
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
/// Provides standardized logging event
/// identifiers used throughout
/// Platform.WebApi.
///
/// Responsibility:
/// - Centralize EventId values.
/// - Prevent duplicated identifiers.
/// - Ensure logging consistency.
///
/// Architectural Rules:
/// - Immutable.
/// - No business logic.
/// - No infrastructure dependency.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
internal static class LogEvents
{
    // ===========================================
    // HTTP Pipeline
    // 1000 - 1099
    // ===========================================

    /// <summary>
    /// Request started.
    /// </summary>
    public const int RequestStarted = 1000;

    /// <summary>
    /// Request completed successfully.
    /// </summary>
    public const int RequestCompletedInformation = 1001;

    /// <summary>
    /// Unhandled exception.
    /// </summary>
    public const int UnhandledException = 1002;

    /// <summary>
    /// Request timeout.
    /// </summary>
    public const int RequestTimeout = 1003;

    /// <summary>
    /// Request cancelled.
    /// </summary>
    public const int RequestCancelled = 1004;

    /// <summary>
    /// Request completed with client error.
    /// </summary>
    public const int RequestCompletedWarning = 1005;

    /// <summary>
    /// Request completed with server error.
    /// </summary>
    public const int RequestCompletedError = 1006;

    // ===========================================
    // Authentication
    // 1100 - 1199
    // ===========================================

    /// <summary>
    /// Authentication failed.
    /// </summary>
    public const int AuthenticationFailed = 1100;

    /// <summary>
    /// Authentication succeeded.
    /// </summary>
    public const int AuthenticationSucceeded = 1101;

    // ===========================================
    // Authorization
    // 1200 - 1299
    // ===========================================

    /// <summary>
    /// Authorization failed.
    /// </summary>
    public const int AuthorizationFailed = 1200;

    /// <summary>
    /// Authorization succeeded.
    /// </summary>
    public const int AuthorizationSucceeded = 1201;
}