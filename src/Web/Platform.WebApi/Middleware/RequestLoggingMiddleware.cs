// ===========================================
// File Location :
// src/Web/Platform.WebApi/Middleware/
// RequestLoggingMiddleware.cs
// ===========================================
//
// REFACTOR BLOCK
//
// Reason:
// Introduce enterprise-grade HTTP request
// logging middleware.
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
using Platform.WebApi.Constants;
using Platform.WebApi.Logging;

namespace Platform.WebApi.Middleware;

/// <summary>
/// Provides centralized HTTP request logging
/// for the Enterprise Governance Platform.
///
/// Responsibility:
/// - Measure request execution time.
/// - Resolve correlation identifier.
/// - Log HTTP request completion.
/// - Produce structured logs.
///
/// Architectural Rules:
/// - No business logic.
/// - No persistence logic.
/// - No authentication.
/// - No authorization.
/// - No repository access.
///
/// Side Effects:
/// - Writes structured log entries.
///
/// Thread Safety:
/// - Stateless.
/// - Safe for concurrent requests.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    /// <summary>
    /// Pipeline continuation delegate.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger instance.
    /// </summary>
    private readonly ILogger<RequestLoggingMiddleware>
        _logger;

    /// <summary>
    /// Initializes a new instance of
    /// <see cref="RequestLoggingMiddleware"/>.
    ///
    /// Responsibility:
    /// - Store immutable dependencies.
    ///
    /// Failure:
    /// - Throws when dependencies are null.
    /// </summary>
    /// <param name="next">
    /// Next middleware.
    /// </param>
    /// <param name="logger">
    /// Logger instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is null.
    /// </exception>
    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(
            next);

        ArgumentNullException.ThrowIfNull(
            logger);

        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Executes the request logging middleware.
    ///
    /// Algorithm:
    /// 1. Start request timer.
    /// 2. Resolve correlation identifier.
    /// 3. Execute remaining pipeline.
    /// 4. Stop timer.
    /// 5. Determine log level.
    /// 6. Write structured log.
    ///
    /// Logging Policy:
    /// - Logging failures must never affect
    ///   request execution.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <returns>
    /// Asynchronous operation.
    /// </returns>
    public async Task InvokeAsync(
        HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        var stopwatch =
            Stopwatch.StartNew();

        var correlationId =
            ResolveCorrelationId(
                context);

        try
        {
            await _next(context)
                .ConfigureAwait(false);
        }
        finally
        {
            stopwatch.Stop();

            context.Items[
                HttpItemKeys.ElapsedMilliseconds] =
                stopwatch.ElapsedMilliseconds;
                
            try
            {
                LogRequest(
                    context,
                    correlationId,
                    stopwatch.ElapsedMilliseconds);
            }
            catch
            {
                // Logging failures must never
                // interrupt HTTP request execution.
            }
        }
    }

    /// <summary>
    /// Resolves the correlation identifier
    /// from the current request context.
    ///
    /// Responsibility:
    /// - Read the correlation identifier from
    ///   <see cref="HttpContext.Items"/>.
    /// - Never generate a new identifier.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <returns>
    /// Correlation identifier when available;
    /// otherwise, <c>N/A</c>.
    /// </returns>
    private static string ResolveCorrelationId(
        HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        if (context.Items.TryGetValue(
                HttpItemKeys.CorrelationId,
                out var value) &&
            value is string correlationId &&
            !string.IsNullOrWhiteSpace(
                correlationId))
        {
            return correlationId;
        }

        return "N/A";
    }

    /// <summary>
    /// Determines the appropriate logging level
    /// for the completed HTTP request.
    ///
    /// Rules:
    /// - 2xx : Information
    /// - 3xx : Information
    /// - 4xx : Warning
    /// - 5xx : Error
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="statusCode">
    /// HTTP status code.
    /// </param>
    /// <returns>
    /// Logging level.
    /// </returns>
    private static LogLevel GetLogLevel(
        int statusCode)
    {
        return statusCode switch
        {
            >= StatusCodes.Status500InternalServerError =>
                LogLevel.Error,

            >= StatusCodes.Status400BadRequest =>
                LogLevel.Warning,

            _ =>
                LogLevel.Information
        };
    }

    /// <summary>
    /// Writes a structured request log.
    ///
    /// Responsibility:
    /// - Determine logging severity.
    /// - Delegate logging to the high-performance
    ///   LoggerMessage source generator.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <param name="correlationId">
    /// Correlation identifier.
    /// </param>
    /// <param name="elapsedMilliseconds">
    /// Request execution duration.
    /// </param>
    private void LogRequest(
        HttpContext context,
        string correlationId,
        long elapsedMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            correlationId);

        var method =
            context.Request.Method;

        var path =
            context.Request.Path.Value
            ?? "/";

        var statusCode =
            context.Response.StatusCode;

        var logLevel =
            GetLogLevel(
                statusCode);

        switch (logLevel)
        {
            case LogLevel.Error:

                _logger.RequestCompletedError(
                    method,
                    path,
                    statusCode,
                    elapsedMilliseconds,
                    correlationId);

                break;

            case LogLevel.Warning:

                _logger.RequestCompletedWarning(
                    method,
                    path,
                    statusCode,
                    elapsedMilliseconds,
                    correlationId);

                break;

            default:

                _logger.RequestCompletedInformation(
                    method,
                    path,
                    statusCode,
                    elapsedMilliseconds,
                    correlationId);

                break;
        }
    }

}   