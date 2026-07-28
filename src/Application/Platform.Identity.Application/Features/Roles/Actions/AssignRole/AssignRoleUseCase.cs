// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Roles/Actions/AssignRole/AssignRoleUseCase.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Errors;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Features.Roles.Actions;

/// <summary>
/// Assigns a role to a user account.
///
/// Responsibility:
/// - Validate user existence.
/// - Validate role existence.
/// - Validate role status.
/// - Execute role assignment workflow.
/// - Persist aggregate changes.
/// - Return application result.
///
/// Architectural Rules:
/// - Contains orchestration logic only.
/// - Does not contain persistence implementation.
/// - Does not contain infrastructure concerns.
/// - Does not perform direct data access.
///
/// Transaction Policy:
/// - Transaction lifecycle is owned by
///   TransactionBehavior.
/// - This use case never commits or rolls back
///   transactions.
///
/// Failure Handling:
/// - DomainException is translated through
///   IdentityErrorMapper.
/// - Business failures return Result failures.
/// - Infrastructure exceptions are allowed
///   to propagate.
///
/// Complexity:
/// O(n) where n is assigned role count.
/// </summary>
public sealed class AssignRoleUseCase : ICommandHandler<AssignRoleCommand>
{
    private readonly IUserAccountRepository
        _userAccountRepository;

    private readonly IRoleRepository
        _roleRepository;

    private readonly IClock
        _clock;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AssignRoleUseCase"/> class.
    /// </summary>
    /// <param name="userAccountRepository">
    /// User account repository.
    /// </param>
    /// <param name="roleRepository">
    /// Role repository.
    /// </param>
    /// <param name="clock">
    /// UTC clock provider.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when dependency is null.
    /// </exception>
    public AssignRoleUseCase(
        IUserAccountRepository userAccountRepository,
        IRoleRepository roleRepository,
        IClock clock)
    {
        _userAccountRepository =
            userAccountRepository
            ?? throw new ArgumentNullException(
                nameof(userAccountRepository));

        _roleRepository =
            roleRepository
            ?? throw new ArgumentNullException(
                nameof(roleRepository));

        _clock =
            clock
            ?? throw new ArgumentNullException(
                nameof(clock));
    }

    /// <summary>
    /// Executes role assignment workflow.
    /// </summary>
    /// <param name="command">
    /// Role assignment request.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Success when role assignment completes;
    /// otherwise a failure result.
    /// </returns>
    public async Task<Result> ExecuteAsync(
        AssignRoleCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            command);

        try
        {
            var userAccount =
                await _userAccountRepository
                    .GetByIdAsync(
                        command.UserId,
                        cancellationToken);

            if (userAccount is null)
            {
                return Result.Failure(
                    IdentityErrors.UserNotFound);
            }

            var role =
                await _roleRepository
                    .GetByIdAsync(
                        command.RoleId,
                        cancellationToken);

            if (role is null)
            {
                return Result.Failure(
                    IdentityErrors.RoleNotFound);
            }

            if (!role.IsActive)
            {
                return Result.Failure(
                    IdentityErrors.RoleInactive);
            }

            userAccount.AssignRole(
                command.RoleId,
                _clock.UtcNow);

            _userAccountRepository.Update(
                userAccount);

            return Result.Success();
        }
        catch (DomainException exception)
        {
            return Result.Failure(
                IdentityErrorMapper.Map(
                    exception));
        }
    }
}