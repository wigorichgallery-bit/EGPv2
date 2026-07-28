// ===========================================
// File Location :
// src/Web/Platform.WebApi/Middleware/
// RequestTimingMiddleware.cs
// ===========================================
//
// REFACTOR BLOCK
//
// Reason:
// Introduce enterprise-grade HTTP response
// timing middleware.
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

namespace Platform.WebApi.Middleware;

/// <summary>
/// Provides HTTP response timing information.
///
/// Responsibility:
/// - Read request execution duration.
/// - Write response timing header.
///
/// Architectural Rules:
/// - No business logic.
/// - No logging.
/// - No persistence.
/// - No stopwatch ownership.
///
/// Side Effects:
/// - Adds response timing header.
///
/// Thread Safety:
/// - Stateless.
/// - Safe for concurrent requests.
/// </summary>
public sealed class RequestTimingMiddleware
{
    /// <summary>
    /// Pipeline continuation delegate.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of
    /// <see cref="RequestTimingMiddleware"/>.
    /// </summary>
    /// <param name="next">
    /// Next middleware.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="next"/>
    /// is null.
    /// </exception>
    public RequestTimingMiddleware(
        RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(
            next);

        _next = next;
    }

    /// <summary>
    /// Executes the middleware.
    ///
    /// Algorithm:
    /// 1. Execute remaining pipeline.
    /// 2. Read elapsed milliseconds.
    /// 3. Write response timing header.
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

        await _next(context)
            .ConfigureAwait(false);

        if (TryGetElapsedMilliseconds(
                context,
                out var elapsedMilliseconds))
        {
            WriteResponseHeader(
                context,
                elapsedMilliseconds);
        }
    }

    /// <summary>
    /// Attempts to read the request execution
    /// duration from the current request context.
    ///
    /// Responsibility:
    /// - Read the elapsed execution time from
    ///   <see cref="HttpContext.Items"/>.
    /// - Never calculate execution time.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <param name="elapsedMilliseconds">
    /// Request execution duration.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the elapsed
    /// time is available; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    private static bool TryGetElapsedMilliseconds(
        HttpContext context,
        out long elapsedMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        elapsedMilliseconds = 0L;

        if (!context.Items.TryGetValue(
                HttpItemKeys.ElapsedMilliseconds,
                out var value))
        {
            return false;
        }

        switch (value)
        {
            case long longValue:
                elapsedMilliseconds = longValue;
                return true;

            case int intValue:
                elapsedMilliseconds = intValue;
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Writes the response timing header.
    ///
    /// Responsibility:
    /// - Expose request execution duration.
    /// - Never overwrite an already-started
    ///   response.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <param name="elapsedMilliseconds">
    /// Request execution duration.
    /// </param>
    private static void WriteResponseHeader(
        HttpContext context,
        long elapsedMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Headers[
            HttpHeaderNames.ResponseTime] =
            $"{elapsedMilliseconds} ms";
    }
}