// ===========================================
// File Location :
// src/Web/Platform.WebApi/Middleware/
// ExceptionHandlingMiddleware.cs
// ===========================================
// REFACTOR BLOCK
//
// Reason:
// Introduce enterprise-grade global exception handling middleware.
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
using Platform.SharedKernel.Exceptions;
using Platform.WebApi.Constants;
using Platform.WebApi.Logging;

namespace Platform.WebApi.Middleware;

/// <summary>
/// Provides centralized global exception handling
/// for the Enterprise Governance Platform.
///
/// Responsibility:
/// - Capture all unhandled exceptions.
/// - Convert exceptions into RFC7807 ProblemDetails.
/// - Prevent internal exception leakage.
/// - Produce deterministic HTTP responses.
/// - Preserve correlation information.
///
/// Architectural Rules:
/// - No business logic.
/// - No persistence logic.
/// - No repository access.
/// - No application orchestration.
/// - No transaction handling.
///
/// Side Effects:
/// - Writes HTTP response.
/// - Logs unhandled exceptions.
/// - Adds correlation metadata.
///
/// Thread Safety:
/// - Stateless.
/// - Safe for concurrent requests.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{

    /// <summary>
    /// Pipeline continuation delegate.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger instance.
    /// </summary>
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// JSON serializer options.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions =
        new()
        {
            PropertyNamingPolicy =
                JsonNamingPolicy.CamelCase
        };

    /// <summary>
    /// Initializes a new instance of
    /// <see cref="ExceptionHandlingMiddleware"/>.
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
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(logger);

        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Executes middleware.
    ///
    /// Algorithm:
    /// 1. Execute next middleware.
    /// 2. Catch unhandled exception.
    /// 3. Log exception.
    /// 4. Map exception.
    /// 5. Create ProblemDetails.
    /// 6. Write response.
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

        try
        {
            await _next(context)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.UnhandledException(
                exception,
                context.Request.Method,
                context.Request.Path,
                exception.GetType().Name);

            var problemDetails =
                CreateProblemDetails(
                    context,
                    exception);

            await WriteProblemDetailsAsync(
                    context,
                    problemDetails)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Creates a RFC7807 ProblemDetails
    /// instance from an exception.
    ///
    /// Responsibility:
    /// - Determine HTTP status code.
    /// - Determine response title.
    /// - Attach correlation metadata.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <param name="exception">
    /// Captured exception.
    /// </param>
    /// <returns>
    /// ProblemDetails instance.
    /// </returns>
    private static ProblemDetails CreateProblemDetails(
        HttpContext context,
        Exception exception)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(exception);

        var (statusCode, title) =
            MapException(exception);

        var correlationId =
            ResolveCorrelationId(context);

        var traceId =
            Activity.Current?.Id
            ?? context.TraceIdentifier;

        var problem =
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
    //             Type = $"https://httpstatuses.com/{statusCode}",
    //             Type = $"https://datatracker.ietf.org/doc/html/rfc9110#name-{title.ToLowerInvariant().Replace(" ", "-")}",
                Type = "about:blank",
                Detail = GetSafeDetail(exception),
                Instance = context.Request.Path
            };

        problem.Extensions["traceId"] =
            traceId;

        problem.Extensions["correlationId"] =
            correlationId;

        return problem;
    }

    /// <summary>
    /// Maps an exception into an HTTP status code
    /// and response title.
    ///
    /// Responsibility:
    /// - Centralize exception mapping.
    /// - Produce deterministic HTTP responses.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="exception">
    /// Exception instance.
    /// </param>
    /// <returns>
    /// HTTP status code and response title.
    /// </returns>
    private static (
        int StatusCode,
        string Title)
        MapException(
            Exception exception)
    {
        ArgumentNullException.ThrowIfNull(
            exception);

        return exception switch
        {
            ArgumentException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Bad Request"
                ),

            UnauthorizedAccessException =>
                (
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized"
                ),

            SecurityException =>
                (
                    StatusCodes.Status403Forbidden,
                    "Forbidden"
                ),

            KeyNotFoundException =>
                (
                    StatusCodes.Status404NotFound,
                    "Resource Not Found"
                ),

            DomainException =>
                (
                    StatusCodes.Status409Conflict,
                    "Domain Rule Violation"
                ),

            InvalidOperationException =>
                (
                    StatusCodes.Status409Conflict,
                    "Invalid Operation"
                ),

            NotImplementedException =>
                (
                    StatusCodes.Status501NotImplemented,
                    "Not Implemented"
                ),

            _ =>
                (
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error"
                )
        };
    }

    /// <summary>
    /// Resolves correlation identifier from the
    /// current request context.
    ///
    /// Algorithm:
    /// 1. Try HttpContext.Items.
    /// 2. Try Request Header.
    /// 3. Generate fallback identifier.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <returns>
    /// Correlation identifier.
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

        if (context.Request.Headers.TryGetValue(
                HttpHeaderNames.CorrelationId,
                out var headerValue))
        {
            var requestCorrelationId =
                headerValue.ToString();

            if (!string.IsNullOrWhiteSpace(
                    requestCorrelationId))
            {
                return requestCorrelationId;
            }
        }

        return Guid.NewGuid()
            .ToString("N");
    }

    /// <summary>
    /// Returns a safe error detail suitable
    /// for client consumption.
    ///
    /// Responsibility:
    /// - Prevent internal information leakage.
    /// - Preserve meaningful validation messages.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="exception">
    /// Exception instance.
    /// </param>
    /// <returns>
    /// Safe error detail.
    /// </returns>
    private static string GetSafeDetail(
        Exception exception)
    {
        ArgumentNullException.ThrowIfNull(
            exception);

        return exception switch
        {
            ArgumentException =>
                exception.Message,

            DomainException =>
                exception.Message,

            UnauthorizedAccessException =>
                "Authentication is required.",

            SecurityException =>
                "Access to the requested resource is denied.",

            KeyNotFoundException =>
                "The requested resource was not found.",

            InvalidOperationException =>
                exception.Message,

            NotImplementedException =>
                "The requested functionality is not available.",

            _ =>
                "An unexpected server error occurred."
        };
    }

    /// <summary>
    /// Writes ProblemDetails to the HTTP response.
    ///
    /// Algorithm:
    /// 1. Set response status code.
    /// 2. Set content type.
    /// 3. Serialize ProblemDetails.
    /// 4. Write response body.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <param name="problemDetails">
    /// Problem details payload.
    /// </param>
    /// <returns>
    /// Asynchronous operation.
    /// </returns>
    private static async Task WriteProblemDetailsAsync(
        HttpContext context,
        ProblemDetails problemDetails)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        ArgumentNullException.ThrowIfNull(
            problemDetails);

        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();

        context.Response.StatusCode =
            problemDetails.Status
            ?? StatusCodes.Status500InternalServerError;

        context.Response.Headers.CacheControl =
            "no-cache, no-store, must-revalidate";

        context.Response.Headers.Pragma =
            "no-cache";

        context.Response.Headers.Expires =
            "0";

        context.Response.ContentType =
            HttpMediaTypes.ProblemJson;

        await JsonSerializer.SerializeAsync(
                context.Response.Body,
                problemDetails,
                JsonOptions,
                context.RequestAborted)
            .ConfigureAwait(false);
    }

}