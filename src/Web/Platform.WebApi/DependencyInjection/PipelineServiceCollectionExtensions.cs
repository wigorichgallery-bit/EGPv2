// ===========================================
// File Location :
// src/Web/Platform.WebApi/DependencyInjection/
// PipelineServiceCollectionExtensions.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Governance;
using Platform.Pipeline.Logging;
using Platform.Pipeline.Transaction;
using Platform.Pipeline.Validation;
using Platform.WebApi.Composition;
using Platform.WebApi.Logging;

namespace Platform.WebApi.DependencyInjection;

/// <summary>
/// Provides dependency injection registration
/// for Platform.Pipeline.
///
/// Responsibility:
/// - Register pipeline executor.
/// - Register execution logging.
/// - Register pipeline behaviors.
/// - Configure pipeline execution chain.
///
/// Architectural Rules:
/// - Composition Root only.
/// - No business logic.
/// - No persistence logic.
/// - No governance implementation.
/// - No transaction implementation.
///
/// Pipeline Order:
/// 100 - Validation
/// 200 - Governance
/// 300 - Transaction
/// 400 - Logging
///
/// Side Effects:
/// - Registers pipeline services into
///   the dependency injection container.
///
/// Thread Safety:
/// - Stateless.
/// </summary>
public static class PipelineServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Platform.Pipeline services.
    ///
    /// Registration Order:
    /// 1. Execution logging.
    /// 2. Pipeline executor.
    /// 3. Result pipeline behaviors.
    /// 4. Generic pipeline behaviors.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <returns>
    /// Updated service collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    public static IServiceCollection AddPipeline(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        // ===========================================
        // Core Pipeline Services
        // ===========================================

        RegisterExecutionLogging(
            services);

        RegisterPipelineExecutor(
            services);

        // ===========================================
        // Pipeline Behaviors
        // ===========================================

        RegisterResultBehaviors(
            services);

        RegisterGenericBehaviors(
            services);

        return services;
    }

    /// <summary>
    /// Registers execution logging services.
    ///
    /// Responsibility:
    /// - Register execution logger.
    /// - Provide default logging implementation.
    ///
    /// Side Effects:
    /// - Registers logging services.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    private static void RegisterExecutionLogging(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped<IExecutionLogger, NullExecutionLogger>();
    }

    /// <summary>
    /// Registers pipeline executor.
    ///
    /// Responsibility:
    /// - Register pipeline orchestration service.
    /// - Resolve and execute ordered behaviors.
    ///
    /// Side Effects:
    /// - Registers pipeline executor.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    private static void RegisterPipelineExecutor(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped<
            IPipelineExecutor,
            PipelineExecutor>();
    }

    /// <summary>
    /// Registers pipeline behaviors for
    /// requests returning <see cref="Platform.SharedKernel.Results.Result"/>.
    ///
    /// Registration Order:
    /// 100 - Validation
    /// 200 - Governance
    /// 300 - Transaction
    /// 400 - Logging
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    private static void RegisterResultBehaviors(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped(
            typeof(IPipelineBehavior<>),
            typeof(ValidationBehavior<>));

        services.AddScoped(
            typeof(IPipelineBehavior<>),
            typeof(GovernanceBehavior<>));

        services.AddScoped(
            typeof(IPipelineBehavior<>),
            typeof(TransactionBehavior<>));

        services.AddScoped(
            typeof(IPipelineBehavior<>),
            typeof(LoggingBehavior<>));
    }

    /// <summary>
    /// Registers pipeline behaviors for
    /// requests returning
    /// <c>Result&lt;TValue&gt;</c>.
    ///
    /// Registration Order:
    /// 100 - Validation
    /// 200 - Governance
    /// 300 - Transaction
    /// 400 - Logging
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    private static void RegisterGenericBehaviors(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviorT<,>));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(GovernanceBehaviorT<,>));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(TransactionBehaviorT<,>));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehaviorT<,>));
    }
}