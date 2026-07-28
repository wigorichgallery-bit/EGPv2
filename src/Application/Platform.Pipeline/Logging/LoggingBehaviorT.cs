// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Logging/LoggingBehaviorT.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Models;
using Platform.SharedKernel.Results;
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Logging;

/// <summary>
/// Logging pipeline behavior for requests
/// returning Result&lt;TValue&gt;.
/// </summary>
/// <typeparam name="TRequest">
/// Request type.
/// </typeparam>
/// <typeparam name="TValue">
/// Result value type.
/// </typeparam>
public sealed class LoggingBehaviorT<TRequest, TValue>
    : IPipelineBehavior<TRequest, TValue>, IPipelineOrdered
{
    private readonly IExecutionLogger _logger;
    public int Order => 400;
    public LoggingBehaviorT(
        IExecutionLogger logger)
    {
        Guard.AgainstNull(
            logger,
            nameof(logger));

        _logger = logger;
    }

    public async Task<Result<TValue>> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken,
        Func<Task<Result<TValue>>> next)
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
                    typeof(TRequest).Name,
                    result.IsSuccess,
                    result.IsFailure
                        ? result.Error.Code
                        : null,
                    null,
                    stopwatch.ElapsedMilliseconds,
                    DateTime.UtcNow),
                cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            await TryLogAsync(
                new ExecutionLogEntry(
                    typeof(TRequest).Name,
                    false,
                    null,
                    ex.GetType().Name,
                    stopwatch.ElapsedMilliseconds,
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