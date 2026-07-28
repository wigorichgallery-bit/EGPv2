// ===========================================
// File Location :
// src/Web/Platform.WebApi/DependencyInjection/
// WebApiServiceCollectionExtensions.cs
// ===========================================
namespace Platform.WebApi.DependencyInjection;

/// <summary>
/// Provides centralized dependency injection
/// registration for Platform.WebApi.
///
/// Responsibility:
/// - Register application services.
/// - Register pipeline services.
/// - Register infrastructure services.
/// - Register ASP.NET Core services.
/// - Configure API discovery services.
///
/// Architectural Rules:
/// - Composition Root only.
/// - No business logic.
/// - No persistence logic.
/// - No infrastructure implementation.
/// - No domain logic.
///
/// Side Effects:
/// - Registers services into the dependency
///   injection container.
///
/// Thread Safety:
/// - Stateless.
/// </summary>
public static class WebApiServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Platform.WebApi services.
    ///
    /// Registration Order:
    /// 1. Platform services.
    /// 2. MVC controllers.
    /// 3. OpenAPI services.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <param name="configuration">
    /// Application configuration.
    /// </param>
    /// <returns>
    /// Updated service collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="configuration"/>
    /// is null.
    /// </exception>
    public static IServiceCollection AddWebApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        ArgumentNullException.ThrowIfNull(
            configuration);

        // ===========================================
        // Platform Services
        // ===========================================

        RegisterPlatformServices(
            services,
            configuration);

        // ===========================================
        // ASP.NET Core Services
        // ===========================================

        RegisterControllers(
            services);

        RegisterOpenApi(
            services);

        return services;
    }

    /// <summary>
    /// Registers all platform layer services.
    ///
    /// Responsibility:
    /// - Register application layer.
    /// - Register pipeline layer.
    /// - Register infrastructure layer.
    ///
    /// Architectural Rules:
    /// - Composition Root only.
    /// - No business logic.
    /// - No orchestration logic.
    ///
    /// Side Effects:
    /// - Registers platform services into
    ///   the dependency injection container.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <param name="configuration">
    /// Application configuration.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="configuration"/>
    /// is null.
    /// </exception>
    private static void RegisterPlatformServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        ArgumentNullException.ThrowIfNull(
            configuration);

        services.AddApplication();

        services.AddPipeline();

        services.AddInfrastructure(
            configuration);
    }

    /// <summary>
    /// Registers MVC controller services.
    ///
    /// Responsibility:
    /// - Register API controllers.
    /// - Enable controller discovery.
    ///
    /// Architectural Rules:
    /// - ASP.NET Core registration only.
    /// - No business logic.
    ///
    /// Side Effects:
    /// - Enables MVC controller support.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    private static void RegisterControllers(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddControllers();
    }

    /// <summary>
    /// Registers OpenAPI discovery services.
    ///
    /// Responsibility:
    /// - Register endpoint discovery.
    /// - Register Swagger/OpenAPI generation.
    ///
    /// Architectural Rules:
    /// - ASP.NET Core registration only.
    /// - No business logic.
    ///
    /// Side Effects:
    /// - Enables OpenAPI metadata generation.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    private static void RegisterOpenApi(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
    }
}