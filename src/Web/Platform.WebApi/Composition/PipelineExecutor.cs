// ===========================================
// File Location :
// src/Web/Platform.WebApi/Composition/PipelineExecutor.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Results;

namespace Platform.WebApi.Composition;

/// <summary>
/// Executes requests through the configured
/// pipeline behavior chain.
///
/// Responsibility:
/// - Resolve pipeline behaviors from DI.
/// - Order pipeline behaviors.
/// - Build execution delegate chain.
/// - Execute the final request handler.
/// - Return operation result.
///
/// Architectural Rules:
/// - Composition Root component.
/// - No business logic.
/// - No persistence logic.
/// - No governance logic.
/// - No validation logic.
/// - No transaction logic.
///
/// Side Effects:
/// - Resolves registered behaviors from DI container.
/// - Executes pipeline chain.
///
/// Algorithm:
/// 1. Resolve behaviors from DI.
/// 2. Order behaviors using IPipelineOrdered.
/// 3. Build delegate chain in reverse order.
/// 4. Execute first behavior.
/// 5. Execute final handler.
/// 6. Return result.
///
/// Complexity:
/// O(n)
///
/// Where:
/// n = number of registered behaviors.
///
/// Thread Safety:
/// Scoped service.
/// Not intended for singleton usage.
/// </summary>
public sealed class PipelineExecutor : IPipelineExecutor
{
    /// <summary>
    /// Service provider used to resolve
    /// pipeline behaviors.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PipelineExecutor"/> class.
    ///
    /// Validation:
    /// - Service provider must not be null.
    /// </summary>
    /// <param name="serviceProvider">
    /// Service provider.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when serviceProvider is null.
    /// </exception>
    public PipelineExecutor(
        IServiceProvider serviceProvider)
    {
        _serviceProvider =
            serviceProvider
            ?? throw new ArgumentNullException(
                nameof(serviceProvider));
    }

    /// <summary>
    /// Executes a request returning
    /// <see cref="Result"/>.
    ///
    /// Algorithm:
    /// 1. Resolve behaviors.
    /// 2. Order behaviors.
    /// 3. Build delegate chain.
    /// 4. Execute pipeline.
    /// 5. Return result.
    /// </summary>
    /// <typeparam name="TRequest">
    /// Request type.
    /// </typeparam>
    /// <param name="request">
    /// Request instance.
    /// </param>
    /// <param name="handler">
    /// Final request handler.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Operation result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when handler is null.
    /// </exception>
    public async Task<Result> ExecuteAsync<TRequest>(
        TRequest request,
        Func<Task<Result>> handler,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(handler);

        var behaviors =
            _serviceProvider
                .GetServices<IPipelineBehavior<TRequest>>()
                .OrderBy(GetOrder)
                .ToList();

        Func<Task<Result>> next = handler;

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var currentNext = next;

            next = () =>
                behavior.HandleAsync(
                    request,
                    cancellationToken,
                    currentNext);
        }

        return await next()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Executes a request returning
    /// <see cref="Result{TValue}"/>.
    ///
    /// Algorithm:
    /// 1. Resolve behaviors.
    /// 2. Order behaviors.
    /// 3. Build delegate chain.
    /// 4. Execute pipeline.
    /// 5. Return result.
    /// </summary>
    /// <typeparam name="TRequest">
    /// Request type.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// Result value type.
    /// </typeparam>
    /// <param name="request">
    /// Request instance.
    /// </param>
    /// <param name="handler">
    /// Final request handler.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Operation result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when handler is null.
    /// </exception>
    public async Task<Result<TValue>> ExecuteAsync<TRequest, TValue>(
        TRequest request,
        Func<Task<Result<TValue>>> handler,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(handler);

        var behaviors =
            _serviceProvider
                .GetServices<IPipelineBehavior<TRequest, TValue>>()
                .OrderBy(GetOrder)
                .ToList();

        Func<Task<Result<TValue>>> next = handler;

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var currentNext = next;

            next = () =>
                behavior.HandleAsync(
                    request,
                    cancellationToken,
                    currentNext);
        }

        return await next()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Resolves execution order for a pipeline behavior.
    ///
    /// Rules:
    /// - Behaviors implementing
    ///   <see cref="IPipelineOrdered"/>
    ///   use their configured order.
    /// - Behaviors not implementing
    ///   <see cref="IPipelineOrdered"/>
    ///   execute last.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <typeparam name="TBehavior">
    /// Behavior type.
    /// </typeparam>
    /// <param name="behavior">
    /// Pipeline behavior instance.
    /// </param>
    /// <returns>
    /// Execution order.
    /// </returns>
    private static int GetOrder<TBehavior>(
        TBehavior behavior)
    {
        return behavior is IPipelineOrdered ordered
            ? ordered.Order
            : int.MaxValue;
    }
}