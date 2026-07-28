using Platform.Identity.Domain.Aggregates;

namespace Platform.Identity.Application.Abstractions.Persistence.Commands;

/// <summary>
/// Defines persistence operations for
/// <see cref="AuthenticationChallenge"/> aggregates.
///
/// <para>
/// Responsibility:
/// - Persist authentication challenge aggregates.
/// - Retrieve aggregates for command operations.
/// - Maintain aggregate consistency.
/// </para>
///
/// <para>
/// Architectural Rules:
/// - Command-side repository only.
/// - Aggregate-oriented.
/// - No query/projection responsibilities.
/// - No business logic.
/// - Infrastructure independent.
/// </para>
/// </summary>
public interface IAuthenticationChallengeRepository
{
    /// <summary>
    /// Retrieves an authentication challenge aggregate
    /// by its unique identifier.
    /// </summary>
    /// <param name="challengeId">
    /// The authentication challenge identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// The authentication challenge aggregate if found;
    /// otherwise <see langword="null"/>.
    /// </returns>
    Task<AuthenticationChallenge?> GetByIdAsync(
        Guid challengeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new authentication challenge aggregate.
    /// </summary>
    /// <param name="authenticationChallenge">
    /// The aggregate to persist.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    Task AddAsync(
        AuthenticationChallenge authenticationChallenge,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing authentication challenge aggregate.
    /// </summary>
    /// <param name="authenticationChallenge">
    /// The aggregate to update.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    Task UpdateAsync(
        AuthenticationChallenge authenticationChallenge,
        CancellationToken cancellationToken = default);
}