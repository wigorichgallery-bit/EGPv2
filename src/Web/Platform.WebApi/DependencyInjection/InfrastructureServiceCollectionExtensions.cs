// ===========================================
// File Location :
// src/Web/Platform.WebApi/DependencyInjection/
// InfrastructureServiceCollectionExtensions.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Persistence.Context;
using Platform.Persistence.Repositories.Commands;
using Platform.Persistence.Repositories.Queries;
using Platform.Persistence.Time;
using Platform.Persistence.UnitOfWorks;
using Platform.Security.Infrastructure.Passwords;
using Platform.Security.Infrastructure.Verification;
using Platform.SharedKernel.Abstractions;

namespace Platform.WebApi.DependencyInjection;

/// <summary>
/// Provides centralized dependency injection
/// registration for infrastructure services.
///
/// Responsibility:
/// - Register EF Core database context.
/// - Register command repositories.
/// - Register query repositories.
/// - Register Unit of Work.
/// - Register infrastructure services.
/// - Register security services.
/// - Register clock services.
///
/// Architectural Rules:
/// - Composition Root only.
/// - No business logic.
/// - No domain logic.
/// - No persistence implementation.
///
/// Side Effects:
/// - Registers infrastructure services into
///   the dependency injection container.
///
/// Thread Safety:
/// - Stateless.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registers all infrastructure services.
    ///
    /// Registration Order:
    /// 1. Database context.
    /// 2. Persistence services.
    /// 3. Security services.
    /// 4. Shared infrastructure services.
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
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        ArgumentNullException.ThrowIfNull(
            configuration);

        // ===========================================
        // Database
        // ===========================================

        RegisterDbContext(
            services,
            configuration);

        // ===========================================
        // Persistence
        // ===========================================

        RegisterRepositories(
            services);

        // ===========================================
        // Infrastructure Services
        // ===========================================

        RegisterSharedInfrastructure(
            services);

        RegisterSecurityServices(
            services);

        return services;
    }

    /// <summary>
    /// Registers the EF Core database context.
    ///
    /// Responsibility:
    /// - Configure EF Core.
    /// - Configure SQL Server provider.
    /// - Register DbContext lifetime.
    ///
    /// Architectural Rules:
    /// - Infrastructure registration only.
    /// - No business logic.
    ///
    /// Side Effects:
    /// - Registers
    ///   <see cref="GovernanceDbContext"/>.
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
    private static void RegisterDbContext(
        IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        ArgumentNullException.ThrowIfNull(
            configuration);

        var connectionString =
            configuration.GetConnectionString(
                "DefaultConnection");

        services.AddDbContext<GovernanceDbContext>(
            options =>
            {
                options.UseSqlServer(
                    connectionString);

#if DEBUG
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
#endif
            });
    }

    /// <summary>
    /// Registers persistence repositories.
    ///
    /// Responsibility:
    /// - Register command repositories.
    /// - Register query repositories.
    ///
    /// Architectural Rules:
    /// - Repository registration only.
    /// - No business logic.
    ///
    /// Side Effects:
    /// - Registers repository services.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    private static void RegisterRepositories(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        // ===========================================
        // Command Repositories
        // ===========================================

        services.AddScoped<
            IUserAccountRepository,
            UserAccountRepository>();

        services.AddScoped<
            IRoleRepository,
            RoleRepository>();

        services.AddScoped<
            IAuthenticationChallengeRepository,
            AuthenticationChallengeRepository>();

        // ===========================================
        // Query Repositories
        // ===========================================

        services.AddScoped<
            IUserQueryRepository,
            UserQueryRepository>();

        services.AddScoped<
            IRoleQueryRepository,
            RoleQueryRepository>();
    }

    /// <summary>
    /// Registers shared infrastructure services.
    ///
    /// Responsibility:
    /// - Register Unit of Work.
    /// - Register system clock.
    ///
    /// Architectural Rules:
    /// - Infrastructure registration only.
    ///
    /// Side Effects:
    /// - Registers shared infrastructure services.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    private static void RegisterSharedInfrastructure(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped<
            IUnitOfWork,
            UnitOfWork>();

        services.AddSingleton<
            IClock,
            SystemClock>();
    }

    /// <summary>
    /// Registers infrastructure security services.
    ///
    /// Responsibility:
    /// - Register password hashing.
    /// - Register verification services.
    ///
    /// Architectural Rules:
    /// - Infrastructure registration only.
    /// - No business logic.
    ///
    /// Side Effects:
    /// - Registers security services.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    private static void RegisterSecurityServices(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped<
            IPasswordHasher,
            PasswordHasher>();

        services.AddScoped<
            IVerificationCodeValidator,
            VerificationCodeValidator>();
    }
}