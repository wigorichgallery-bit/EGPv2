// ===========================================
// File Location :
// src/Web/Platform.WebApi/DependencyInjection/
// ApplicationServiceCollectionExtensions.cs
// ===========================================
// REFACTOR BLOCK
// Reason:
// Register command validators into DI container.
//
// Affected Module:
// Platform.WebApi
//
// Breaking Change:
// NO
//
// Version:
// 1.1.0
// ===========================================
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Identity.Application.Features.Roles.Actions;
using Platform.Identity.Application.Features.Roles.Queries;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Identity.Application.Features.Users.Queries;
using Platform.Pipeline.Abstractions;

namespace Platform.WebApi.DependencyInjection;

/// <summary>
/// Provides dependency injection registration
/// for Platform.Identity.Application.
///
/// Responsibility:
/// - Register application use cases.
/// - Register application orchestration services.
/// - Configure application layer dependencies.
///
/// Architectural Rules:
/// - Composition Root only.
/// - No business logic.
/// - No persistence logic.
/// - No infrastructure logic.
///
/// Side Effects:
/// - Registers application services into
///   the dependency injection container.
///
/// Thread Safety:
/// - Stateless registration class.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Registers application layer services.
    ///
    /// Algorithm:
    /// 1. Register user use cases.
    /// 2. Register role use cases.
    /// 3. Return IServiceCollection.
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
    /// Thrown when services is null.
    /// </exception>
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        RegisterUserUseCases(
            services);

        RegisterRoleUseCases(
            services);

        RegisterUserQueryHandlers(
            services);

        RegisterRoleQueryHandlers(
            services);
            
        RegisterCommandValidators(
            services);

        RegisterQueryValidators(
            services);

        return services;
    }

    /// <summary>
    /// Registers all user command handlers.
    ///
    /// Responsibility:
    /// - Register user command handlers.
    /// - Map handler contracts to implementations.
    /// - Keep command registrations centralized.
    ///
    /// Architectural Rules:
    /// - Composition Root only.
    /// - No business logic.
    /// - No infrastructure logic.
    ///
    /// Side Effects:
    /// - Registers scoped command handlers.
    ///
    /// Thread Safety:
    /// - Stateless.
    ///
    /// Complexity:
    /// - O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    private static void RegisterUserUseCases(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped<
            ICommandHandler<
                CreateUserCommand,
                Guid>,
            CreateUserUseCase>();

        services.AddScoped<
            ICommandHandler<
                ChangePasswordCommand>,
            ChangePasswordUseCase>();

        services.AddScoped<
            ICommandHandler<
                EnableMfaCommand>,
            EnableMfaUseCase>();

        services.AddScoped<
            ICommandHandler<
                DisableMfaCommand>,
            DisableMfaUseCase>();

        services.AddScoped<
            ICommandHandler<
                VerifyEmailCommand>,
            VerifyEmailUseCase>();

        services.AddScoped<
            ICommandHandler<
                VerifyPhoneCommand>,
            VerifyPhoneUseCase>();
    }

    /// <summary>
    /// Registers all role command handlers.
    ///
    /// Responsibility:
    /// - Register role command handlers.
    /// - Map handler contracts to implementations.
    ///
    /// Architectural Rules:
    /// - Composition Root only.
    /// - No business logic.
    /// - No infrastructure logic.
    ///
    /// Side Effects:
    /// - Registers scoped command handlers.
    ///
    /// Thread Safety:
    /// - Stateless.
    ///
    /// Complexity:
    /// - O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    private static void RegisterRoleUseCases(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped<
            ICommandHandler<
                AssignRoleCommand>,
            AssignRoleUseCase>();

        services.AddScoped<
            ICommandHandler<
                RemoveRoleCommand>,
            RemoveRoleUseCase>();
    }

    /// <summary>
    /// Registers all user query handlers.
    ///
    /// Responsibility:
    /// - Register user query handlers.
    /// - Map query contracts to implementations.
    ///
    /// Architectural Rules:
    /// - Composition Root only.
    /// - No business logic.
    /// - No infrastructure logic.
    ///
    /// Side Effects:
    /// - Registers scoped query handlers.
    ///
    /// Thread Safety:
    /// - Stateless.
    ///
    /// Complexity:
    /// - O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    private static void RegisterUserQueryHandlers(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped<
            IQueryHandler<
                GetUserByIdQuery,
                UserDto>,
            GetUserByIdQueryHandler>();

        services.AddScoped<
            IQueryHandler<
                GetUserByUsernameQuery,
                UserDto>,
            GetUserByUsernameQueryHandler>();

        services.AddScoped<
            IQueryHandler<
                GetUsersQuery,
                IReadOnlyList<UserDto>>,
            GetUsersQueryHandler>();
    }

    /// <summary>
    /// Registers all role query handlers.
    ///
    /// Responsibility:
    /// - Register role query handlers.
    /// - Map query contracts to implementations.
    ///
    /// Architectural Rules:
    /// - Composition Root only.
    /// - No business logic.
    /// - No infrastructure logic.
    ///
    /// Side Effects:
    /// - Registers scoped query handlers.
    ///
    /// Thread Safety:
    /// - Stateless.
    ///
    /// Complexity:
    /// - O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    private static void RegisterRoleQueryHandlers(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        services.AddScoped<
            IQueryHandler<
                GetRolesQuery,
                IReadOnlyList<RoleDto>>,
            GetRolesQueryHandler>();
    }

    /// <summary>
    /// Registers all application command validators.
    ///
    /// Responsibility:
    /// - Register command validators.
    /// - Enable validation pipeline execution.
    /// - Keep validator registration centralized.
    ///
    /// Architectural Rules:
    /// - Composition Root only.
    /// - No business logic.
    /// - No infrastructure dependency.
    ///
    /// Side Effects:
    /// - Registers validators into dependency injection.
    ///
    /// Thread Safety:
    /// - Stateless.
    ///
    /// Complexity:
    /// - O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when services is null.
    /// </exception>
    private static void RegisterCommandValidators(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        // ===========================================
        // User Validators
        // ===========================================

        services.AddScoped<
            ICommandValidator<CreateUserCommand>,
            CreateUserValidator>();

        services.AddScoped<
            ICommandValidator<ChangePasswordCommand>,
            ChangePasswordValidator>();

        services.AddScoped<
            ICommandValidator<EnableMfaCommand>,
            EnableMfaValidator>();

        services.AddScoped<
            ICommandValidator<DisableMfaCommand>,
            DisableMfaValidator>();

        services.AddScoped<
            ICommandValidator<VerifyEmailCommand>,
            VerifyEmailValidator>();

        services.AddScoped<
            ICommandValidator<VerifyPhoneCommand>,
            VerifyPhoneValidator>();

        // ===========================================
        // Role Validators
        // ===========================================

        services.AddScoped<
            ICommandValidator<AssignRoleCommand>,
            AssignRoleValidator>();

        services.AddScoped<
            ICommandValidator<RemoveRoleCommand>,
            RemoveRoleValidator>();
    }

    /// <summary>
    /// Registers all application query validators.
    ///
    /// Responsibility:
    /// - Centralize query validator registration.
    /// - Prepare validation pipeline for query validation.
    ///
    /// Architectural Rules:
    /// - Composition Root only.
    /// - No business logic.
    /// - No infrastructure dependency.
    ///
    /// Side Effects:
    /// - Registers query validators into
    ///   dependency injection container.
    ///
    /// Thread Safety:
    /// - Stateless.
    ///
    /// Complexity:
    /// - O(1)
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="services"/>
    /// is null.
    /// </exception>
    private static void RegisterQueryValidators(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        // Reserved for future query validators.
    }
}