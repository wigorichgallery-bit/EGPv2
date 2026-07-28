// ===========================================
// File Location :
// src/Web/Platform.WebApi/
// Composition/
// IdentityRoleSeeder.cs
// ===========================================
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Constants;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Context;

namespace Platform.WebApi.Composition;

/// <summary>
/// Seeds mandatory system roles during
/// application startup.
///
/// Responsibility:
/// - Ensure built-in system roles exist.
/// - Seed immutable system roles.
/// - Prevent duplicate seed execution.
/// - Execute only during startup.
///
/// Architectural Rules:
/// - Composition Root only.
/// - No business logic.
/// - No application orchestration.
/// - No controller dependency.
/// - No repository dependency.
///
/// Seed Strategy:
/// - Execute once.
/// - Idempotent.
/// - Stable identifiers.
/// - Stable scopes.
///
/// Thread Safety:
/// - Startup execution only.
/// </summary>
public static class IdentityRoleSeeder
{
    /// <summary>
    /// Seeds built-in system roles.
    ///
    /// Algorithm:
    /// 1. Create service scope.
    /// 2. Resolve DbContext.
    /// 3. Skip when roles already exist.
    /// 4. Create default roles.
    /// 5. Persist roles.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="serviceProvider">
    /// Root service provider.
    /// </param>
    /// <returns>
    /// Asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="serviceProvider"/>
    /// is null.
    /// </exception>
    public static async Task SeedAsync(
        IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(
            serviceProvider);

        using IServiceScope scope =
            serviceProvider.CreateScope();

        GovernanceDbContext dbContext =
            scope.ServiceProvider
                .GetRequiredService<
                    GovernanceDbContext>();

        bool hasRoles =
            await dbContext
                .Roles
                .AnyAsync()
                .ConfigureAwait(false);

        if (hasRoles)
        {
            return;
        }

        DateTime utcNow =
            DateTime.UtcNow;

        Role[] roles =
        [
            new(
                SystemRoleIds.SystemAdministrator,
                "SystemAdministrator",
                true,
                RoleScope.Global,
                utcNow),

            new(
                SystemRoleIds.GovernanceAdministrator,
                "GovernanceAdministrator",
                true,
                RoleScope.Global,
                utcNow),

            new(
                SystemRoleIds.SecurityAdministrator,
                "SecurityAdministrator",
                true,
                RoleScope.Global,
                utcNow),

            new(
                SystemRoleIds.Auditor,
                "Auditor",
                true,
                RoleScope.Global,
                utcNow),

            new(
                SystemRoleIds.Operator,
                "Operator",
                true,
                RoleScope.Global,
                utcNow)
        ];

        await dbContext
            .Roles
            .AddRangeAsync(
                roles,
                CancellationToken.None)
            .ConfigureAwait(false);

        await dbContext
            .SaveChangesAsync(
                CancellationToken.None)
            .ConfigureAwait(false);
    }
}