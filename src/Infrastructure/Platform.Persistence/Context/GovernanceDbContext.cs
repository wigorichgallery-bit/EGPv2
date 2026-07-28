// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Context/GovernanceDbContext.cs
// ===========================================
using Platform.Identity.Domain.Aggregates;

namespace Platform.Persistence.Context;

/// <summary>
/// Represents the Entity Framework Core database
/// context for the Enterprise Governance Platform.
///
/// Responsibility:
/// - Define aggregate persistence boundary.
/// - Expose aggregate DbSets.
/// - Apply entity configurations.
/// - Coordinate EF Core persistence lifecycle.
/// - Provide future extension points for
///   persistence infrastructure.
///
/// Architectural Rules:
/// - Infrastructure layer only.
/// - No business logic.
/// - No application orchestration.
/// - No domain decision making.
/// - No direct domain event dispatching.
///
/// Aggregate Coverage:
/// - UserAccount
/// - Role
///
/// Value Object Strategy:
/// - Owned types configured through
///   IEntityTypeConfiguration.
/// - Value converters configured through
///   IEntityTypeConfiguration.
///
/// Domain Event Strategy:
/// - Domain events remain inside aggregates.
/// - Event collection is handled by UnitOfWork.
/// - Event dispatching occurs after successful
///   transaction commit.
///
/// Thread Safety:
/// - Not thread safe.
/// - Scoped lifetime only.
///
/// Side Effects:
/// - Persists aggregate state using EF Core.
/// </summary>
public sealed class GovernanceDbContext
    : DbContext
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GovernanceDbContext"/> class.
    /// </summary>
    /// <param name="options">
    /// DbContext configuration options.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="options"/>
    /// is null.
    /// </exception>
    public GovernanceDbContext(
        DbContextOptions<GovernanceDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the user account aggregate set.
    /// </summary>
    public DbSet<UserAccount> UserAccounts
        => Set<UserAccount>();

    /// <summary>
    /// Gets the role aggregate set.
    /// </summary>
    public DbSet<Role> Roles
        => Set<Role>();

    /// <summary>
    /// Configures the EF Core model.
    ///
    /// Algorithm:
    /// 1. Invoke base configuration.
    /// 2. Discover entity configurations.
    /// 3. Apply configurations.
    ///
    /// Complexity:
    /// O(n)
    ///
    /// Where:
    /// n = number of entity configurations.
    /// </summary>
    /// <param name="modelBuilder">
    /// EF Core model builder.
    /// </param>
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(
            modelBuilder);

        base.OnModelCreating(
            modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(GovernanceDbContext).Assembly);
    }

    /// <summary>
    /// Saves all pending changes.
    ///
    /// Algorithm:
    /// 1. Execute pre-save hook.
    /// 2. Persist changes.
    /// 3. Execute post-save hook.
    ///
    /// Complexity:
    /// O(n)
    ///
    /// Where:
    /// n = tracked entities.
    /// </summary>
    /// <returns>
    /// Number of affected rows.
    /// </returns>
    public override int SaveChanges()
    {
        BeforeSaveChanges();

        var affectedRows =
            base.SaveChanges();

        AfterSaveChanges();

        return affectedRows;
    }

    /// <summary>
    /// Saves all pending changes asynchronously.
    ///
    /// Algorithm:
    /// 1. Execute pre-save hook.
    /// 2. Persist changes.
    /// 3. Execute post-save hook.
    ///
    /// Complexity:
    /// O(n)
    ///
    /// Where:
    /// n = tracked entities.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Number of affected rows.
    /// </returns>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        BeforeSaveChanges();

        var affectedRows =
            await base.SaveChangesAsync(
                cancellationToken);

        AfterSaveChanges();

        return affectedRows;
    }

    /// <summary>
    /// Executes logic before EF Core persists
    /// tracked entities.
    ///
    /// Responsibility:
    /// - Reserved for future persistence hooks.
    /// - Reserved for UTC normalization.
    /// - Reserved for auditing.
    /// - Reserved for aggregate inspection.
    ///
    /// Current Behavior:
    /// - No operation.
    /// </summary>
    private void BeforeSaveChanges()
    {
        // Future extension point.
    }

    /// <summary>
    /// Executes logic after EF Core successfully
    /// persists tracked entities.
    ///
    /// Responsibility:
    /// - Reserved for future persistence hooks.
    /// - Reserved for domain event collection.
    /// - Reserved for integration with
    ///   UnitOfWork.
    ///
    /// Current Behavior:
    /// - No operation.
    /// </summary>
    private void AfterSaveChanges()
    {
        // Future extension point.
    }
}