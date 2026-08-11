// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Queries/GetUsers/GetUsersQueryHandler.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Features.Users.Queries;

/// <summary>
/// Handles execution of
/// <see cref="GetUsersQuery"/>.
///
/// Responsibility:
/// - Retrieve all user read models.
/// - Coordinate query repository access.
/// - Return immutable user DTO collection.
///
/// Architectural Rules:
/// - Query side only.
/// - Read-only.
/// - No aggregate loading.
/// - No domain mutation.
/// - No transaction.
/// - No UnitOfWork.
/// - No infrastructure implementation.
///
/// Thread Safety:
/// - Stateless.
/// - Scoped lifetime.
///
/// Algorithm:
/// 1. Validate query instance.
/// 2. Retrieve all users.
/// 3. Return successful query result.
///
/// Complexity:
/// O(n)
///
/// Where:
/// n = number of users returned.
/// </summary>
public sealed class GetUsersQueryHandlerTests : IQueryHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly IUserQueryRepository
        _userQueryRepository;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetUsersQueryHandler"/> class.
    /// </summary>
    /// <param name="userQueryRepository">
    /// User query repository.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="userQueryRepository"/>
    /// is null.
    /// </exception>
    public GetUsersQueryHandlerTests(
        IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository =
            userQueryRepository
            ?? throw new ArgumentNullException(
                nameof(userQueryRepository));
    }

    /// <summary>
    /// Executes the query.
    /// </summary>
    /// <param name="query">
    /// Query contract.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Read-only collection of
    /// <see cref="UserDto"/> instances.
    /// </returns>
    public async Task<Result<IReadOnlyList<UserDto>>> ExecuteAsync(
        GetUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            query);

        var users =
            await _userQueryRepository
                .ListAsync(
                    cancellationToken);

        return Result<IReadOnlyList<UserDto>>
            .Success(
                users);
    }
}