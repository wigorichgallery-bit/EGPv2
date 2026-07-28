// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Roles/Queries/GetRoles/GetRolesQueryHandler.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Features.Roles.Queries;

/// <summary>
/// Handles execution of
/// <see cref="GetRolesQuery"/>.
///
/// Responsibility:
/// - Retrieve all role read models.
/// - Coordinate query repository access.
/// - Return immutable role DTO collection.
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
/// 2. Retrieve roles from query repository.
/// 3. Return successful query result.
///
/// Complexity:
/// O(n)
///
/// Where:
/// n = number of roles returned.
/// </summary>
public sealed class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IReadOnlyList<RoleDto>>
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetRolesQueryHandler"/> class.
    /// </summary>
    private readonly IRoleQueryRepository
        _roleQueryRepository;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetRolesQueryHandler"/> class.
    /// </summary>
    /// <param name="roleQueryRepository">
    /// Role query repository.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="roleQueryRepository"/>
    /// is null.
    /// </exception>
    public GetRolesQueryHandler(
        IRoleQueryRepository roleQueryRepository)
    {
        _roleQueryRepository =
            roleQueryRepository
            ?? throw new ArgumentNullException(
                nameof(roleQueryRepository));
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
    /// <see cref="RoleDto"/> instances.
    /// </returns>
    public async Task<Result<IReadOnlyList<RoleDto>>> ExecuteAsync(
        GetRolesQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            query);

        var roles =
            await _roleQueryRepository
                .ListAsync(
                    cancellationToken);

        return Result<IReadOnlyList<RoleDto>>
            .Success(
                roles);
    }
}