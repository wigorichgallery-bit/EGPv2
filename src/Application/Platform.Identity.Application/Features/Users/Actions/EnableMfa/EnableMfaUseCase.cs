// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/EnableMfa/EnableMfaUseCase.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Errors;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Enables multi-factor authentication
/// for a user account.
///
/// Responsibility:
/// - Load user aggregate.
/// - Execute MFA enablement workflow.
/// - Persist aggregate changes.
/// - Return application result.
///
/// Architectural Rules:
/// - Contains orchestration logic only.
/// - Does not contain persistence implementation.
/// - Does not contain infrastructure concerns.
/// - Does not contain cryptographic implementation.
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
/// </summary>
public sealed class EnableMfaUseCase : ICommandHandler<EnableMfaCommand>
{
    private readonly IUserAccountRepository
        _userAccountRepository;

    private readonly IClock
        _clock;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="EnableMfaUseCase"/> class.
    /// </summary>
    /// <param name="userAccountRepository">
    /// User account repository.
    /// </param>
    /// <param name="clock">
    /// UTC clock provider.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when dependency is null.
    /// </exception>
    public EnableMfaUseCase(
        IUserAccountRepository userAccountRepository,
        IClock clock)
    {
        _userAccountRepository =
            userAccountRepository
            ?? throw new ArgumentNullException(
                nameof(userAccountRepository));

        _clock =
            clock
            ?? throw new ArgumentNullException(
                nameof(clock));
    }

    /// <summary>
    /// Executes MFA enablement workflow.
    /// </summary>
    /// <param name="command">
    /// MFA enablement request.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Success when MFA is enabled successfully;
    /// otherwise a failure result.
    /// </returns>
    public async Task<Result> ExecuteAsync(
        EnableMfaCommand command,
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

            userAccount.EnableMFA(
                command.Method,
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