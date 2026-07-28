// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// UnitOfWorks/UnitOfWork.cs
// ===========================================
using Platform.Persistence.Context;
using Platform.SharedKernel.Abstractions;

namespace Platform.Persistence.UnitOfWorks;

/// <summary>
/// Provides the Entity Framework Core
/// implementation of <see cref="IUnitOfWork"/>.
///
/// Responsibility:
/// - Define transaction boundary.
/// - Persist aggregate changes.
/// - Commit database transaction.
/// - Roll back failed transactions.
/// - Coordinate EF Core persistence.
///
/// Architectural Rules:
/// - Infrastructure layer only.
/// - No business logic.
/// - No application orchestration.
/// - No domain decision making.
///
/// Transaction Strategy:
/// 1. Begin transaction.
/// 2. Persist changes.
/// 3. Commit transaction.
/// 4. Roll back on failure.
/// 5. Dispose transaction.
///
/// Thread Safety:
/// - Scoped lifetime.
/// - Not thread-safe.
///
/// EF Core Compatibility:
/// - EF Core 10.
/// </summary>
public sealed class UnitOfWork
    : IUnitOfWork
{
    private readonly GovernanceDbContext
        _dbContext;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="dbContext">
    /// Database context.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="dbContext"/>
    /// is null.
    /// </exception>
    public UnitOfWork(
        GovernanceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(
            dbContext);

        _dbContext =
            dbContext;
    }

    /// <summary>
    /// Commits all tracked aggregate changes
    /// within a single database transaction.
    ///
    /// Responsibility:
    /// - Begin transaction.
    /// - Persist aggregate changes.
    /// - Commit transaction.
    /// - Roll back on failure.
    ///
    /// Algorithm:
    /// 1. Begin transaction.
    /// 2. Save EF Core changes.
    /// 3. Commit transaction.
    /// 4. Return affected rows.
    ///
    /// Failure Handling:
    /// - Roll back transaction.
    /// - Clear EF Core tracking.
    /// - Rethrow exception.
    ///
    /// Complexity:
    /// O(n)
    ///
    /// Where:
    /// n = number of tracked entities.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Number of affected rows.
    /// </returns>
    public async Task<int> CommitAsync(
        CancellationToken cancellationToken = default)
    {
        await using IDbContextTransaction transaction =
            await _dbContext
                .Database
                .BeginTransactionAsync(
                    cancellationToken)
                .ConfigureAwait(false);

        try
        {
            int affectedRows =
                await _dbContext
                    .SaveChangesAsync(
                        cancellationToken)
                    .ConfigureAwait(false);

            await transaction
                .CommitAsync(
                    cancellationToken)
                .ConfigureAwait(false);

            return affectedRows;
        }
        catch
        {
            await transaction
                .RollbackAsync(
                    cancellationToken)
                .ConfigureAwait(false);

            _dbContext
                .ChangeTracker
                .Clear();

            throw;
        }
    }

    /// <summary>
    /// Rolls back all tracked aggregate
    /// changes.
    ///
    /// Responsibility:
    /// - Discard pending changes.
    /// - Clear EF Core tracking.
    ///
    /// Algorithm:
    /// 1. Clear change tracker.
    /// 2. Return.
    ///
    /// Complexity:
    /// O(n)
    ///
    /// Where:
    /// n = number of tracked entities.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Asynchronous operation.
    /// </returns>
    public Task RollbackAsync(
        CancellationToken cancellationToken = default)
    {
        _dbContext
            .ChangeTracker
            .Clear();

        return Task.CompletedTask;
    }
}