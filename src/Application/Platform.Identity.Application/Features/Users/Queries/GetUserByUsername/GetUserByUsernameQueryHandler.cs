// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Queries/GetUserByUsername/GetUserByUsernameQueryHandler.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Identity.Application.Errors;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Features.Users.Queries;

/// <summary>
/// Handles execution of
/// <see cref="GetUserByUsernameQuery"/>.
///
/// Responsibility:
/// - Retrieve a user read model by username.
/// - Coordinate query repository access.
/// - Translate repository results into
///   application results.
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
/// 2. Retrieve user by username.
/// 3. Return failure when user is not found.
/// 4. Return successful query result.
///
/// Complexity:
/// O(1)
/// </summary>
public sealed class GetUserByUsernameQueryHandler : IQueryHandler<GetUserByUsernameQuery, UserDto>
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetUserByUsernameQueryHandler"/> class.
    /// </summary>
    private readonly IUserQueryRepository
        _userQueryRepository;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetUserByUsernameQueryHandler"/> class.
    /// </summary>
    /// <param name="userQueryRepository">
    /// User query repository.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="userQueryRepository"/>
    /// is null.
    /// </exception>
    public GetUserByUsernameQueryHandler(
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
    /// User DTO when found;
    /// otherwise a failure result.
    /// </returns>
    public async Task<Result<UserDto>> ExecuteAsync(
        GetUserByUsernameQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            query);

        var user =
            await _userQueryRepository
                .FindByUsernameAsync(
                    query.Username,
                    cancellationToken);

        if (user is null)
        {
            return Result<UserDto>
                .Failure(
                    IdentityErrors.UserNotFound);
        }

        return Result<UserDto>
            .Success(
                user);
    }

 
}