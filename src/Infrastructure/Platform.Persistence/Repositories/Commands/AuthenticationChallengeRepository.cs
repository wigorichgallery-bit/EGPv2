
// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Repositories/Commands/
// AuthenticationChallengeRepository.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Domain.Aggregates;
using Platform.Persistence.Context;

namespace Platform.Persistence.Repositories.Commands;

/// <summary>
/// Provides Entity Framework Core implementation
/// of <see cref="IAuthenticationChallengeRepository"/>.
///
/// <para>
/// Responsibility:
/// - Retrieve tracked authentication challenge aggregates.
/// - Persist authentication challenge aggregates.
/// - Preserve aggregate lifecycle state.
/// </para>
///
/// <para>
/// Architectural Rules:
/// - Command side only.
/// - Tracked entities only.
/// - No business logic.
/// - No application orchestration.
/// - No transaction management.
/// - No DTO projection.
/// </para>
///
/// <para>
/// Persistence Strategy:
/// - EF Core.
/// - Tracked aggregates.
/// - Aggregate persistence.
/// - UnitOfWork commit.
/// </para>
///
/// <para>
/// Thread Safety:
/// - Scoped lifetime.
/// - Not thread-safe.
/// </para>
/// </summary>
public sealed class AuthenticationChallengeRepository
    : IAuthenticationChallengeRepository
{
    private readonly GovernanceDbContext
        _dbContext;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContext">
    /// Database context.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="dbContext"/>
    /// is null.
    /// </exception>
    public AuthenticationChallengeRepository(
        GovernanceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(
            dbContext);

        _dbContext =
            dbContext;
    }

    /// <inheritdoc />
    public async Task<AuthenticationChallenge?> GetByIdAsync(
        Guid challengeId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .AuthenticationChallenges
            .FirstOrDefaultAsync(
                challenge =>
                    challenge.Id == challengeId,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddAsync(
        AuthenticationChallenge authenticationChallenge,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            authenticationChallenge);

        await _dbContext
            .AuthenticationChallenges
            .AddAsync(
                authenticationChallenge,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        AuthenticationChallenge authenticationChallenge,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            authenticationChallenge);

        _dbContext
            .AuthenticationChallenges
            .Update(
                authenticationChallenge);

        return Task.CompletedTask;
    }
}