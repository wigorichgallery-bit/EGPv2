// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Logging/LoggingBehavior.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Models;
using Platform.SharedKernel.Results;
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Logging;

/// <summary>
/// Logging pipeline behavior for requests
/// returning Result.
/// </summary>
/// <typeparam name="TRequest">
/// Request type.
/// </typeparam>
public sealed class LoggingBehavior<TRequest>
    : IPipelineBehavior<TRequest>, IPipelineOrdered
{
    private readonly IExecutionLogger _logger;
    public int Order => 400;
    public LoggingBehavior(
        IExecutionLogger logger)
    {
        Guard.AgainstNull(
            logger,
            nameof(logger));

        _logger = logger;
    }

    public async Task<Result> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken,
        Func<Task<Result>> next)
    {
        Guard.AgainstNull(
            request,
            nameof(request));

        Guard.AgainstNull(
            next,
            nameof(next));

        var stopwatch =
            Stopwatch.StartNew();

        try
        {
            var result =
                await next();

            stopwatch.Stop();

            await TryLogAsync(
                new ExecutionLogEntry(
                    RequestName:
                        typeof(TRequest).Name,
                    Success:
                        result.IsSuccess,
                    ErrorCode:
                        result.IsFailure
                            ? result.Error.Code
                            : null,
                    ExceptionType:
                        null,
                    DurationMs:
                        stopwatch.ElapsedMilliseconds,
                    TimestampUtc:
                        DateTime.UtcNow),
                cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            await TryLogAsync(
                new ExecutionLogEntry(
                    RequestName:
                        typeof(TRequest).Name,
                    Success:
                        false,
                    ErrorCode:
                        null,
                    ExceptionType:
                        ex.GetType().Name,
                    DurationMs:
                        stopwatch.ElapsedMilliseconds,
                    TimestampUtc:
                        DateTime.UtcNow),
                cancellationToken);

            throw;
        }
    }

    private async Task TryLogAsync(
        ExecutionLogEntry entry,
        CancellationToken cancellationToken)
    {
        try
        {
            await _logger.LogAsync(
                entry,
                cancellationToken);
        }
        catch
        {
            // Logging must never break execution.
        }
    }
}