// ===========================================
// File Location : src/Application/Platform.Identity.Application/Abstractions/Persistence/Commands/IUserAccountRepository.cs
// ===========================================
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Identity.Application.Abstractions.Persistence.Commands;

/// <summary>
/// Defines persistence operations for the UserAccount aggregate root.
///
/// Responsibility:
/// - Aggregate retrieval.
/// - Aggregate existence checks.
/// - Aggregate persistence lifecycle.
///
/// Invariants:
/// - Works only with UserAccount aggregate roots.
/// - Does not expose persistence implementation details.
/// - Does not expose IQueryable or infrastructure concerns.
///
/// Side Effects:
/// - None. Contract definition only.
///
/// Algorithm:
/// 1. Retrieve aggregate by identity criteria.
/// 2. Check aggregate existence.
/// 3. Persist aggregate state transitions.
/// 4. Defer transaction control to IUnitOfWork.
///
/// Complexity:
/// O(1) contract definition.
/// </summary>
public interface IUserAccountRepository
{
    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The matching user aggregate if found; otherwise null.
    /// </returns>
    Task<UserAccount?> GetByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    /// <param name="username">Unique username.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The matching user aggregate if found; otherwise null.
    /// </returns>
    Task<UserAccount?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by email address.
    /// </summary>
    /// <param name="email">Email address value object.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The matching user aggregate if found; otherwise null.
    /// </returns>
    Task<UserAccount?> GetByEmailAsync(
        EmailAddress email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// True when the user exists; otherwise false.
    /// </returns>
    Task<bool> ExistsByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether a username already exists.
    /// </summary>
    /// <param name="username">Username to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// True when the username exists; otherwise false.
    /// </returns>
    Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether an email address already exists.
    /// </summary>
    /// <param name="email">Email address value object.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// True when the email exists; otherwise false.
    /// </returns>
    Task<bool> ExistsByEmailAsync(
        EmailAddress email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether a phone number already exists.
    /// </summary>
    /// <param name="phoneNumber">
    /// Phone number value object.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// True when the phone number exists; otherwise false.
    /// </returns>
    Task<bool> ExistsByPhoneAsync(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all users 
    /// Primarily intended for administrative
    ///  or batch-processing scenarios.
    ///  Read-model queries should be implemented
    ///  through IUserQueryRepository.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Read-only collection of user aggregates.
    /// </returns>
    Task<IReadOnlyList<UserAccount>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new aggregate instance.
    /// </summary>
    /// <param name="userAccount">Aggregate instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(
        UserAccount userAccount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks an aggregate as modified.
    /// </summary>
    /// <param name="userAccount">Aggregate instance.</param>
    void Update(
        UserAccount userAccount);

    /// <summary>
    /// Marks an aggregate for removal.
    /// </summary>
    /// <param name="userAccount">Aggregate instance.</param>
    void Remove(
        UserAccount userAccount);
}