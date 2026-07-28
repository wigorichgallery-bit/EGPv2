// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Features/Users/Actions/ChangePassword/ChangePasswordUseCase.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Errors;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Changes a user's password.
///
/// Responsibility:
/// - Load user aggregate.
/// - Validate current password.
/// - Generate new password hash.
/// - Execute password rotation.
/// - Persist aggregate changes.
/// - Return application result.
///
/// Architectural Rules:
/// - Contains orchestration logic only.
/// - Does not contain cryptographic implementation.
/// - Does not contain persistence implementation.
/// - Does not access infrastructure directly.
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
public sealed class ChangePasswordUseCase : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserAccountRepository
        _userAccountRepository;

    private readonly IPasswordHasher
        _passwordHasher;

    private readonly IClock
        _clock;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ChangePasswordUseCase"/> class.
    /// </summary>
    /// <param name="userAccountRepository">
    /// User account repository.
    /// </param>
    /// <param name="passwordHasher">
    /// Password hashing service.
    /// </param>
    /// <param name="clock">
    /// UTC clock provider.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when dependency is null.
    /// </exception>
    public ChangePasswordUseCase(
        IUserAccountRepository userAccountRepository,
        IPasswordHasher passwordHasher,
        IClock clock)
    {
        _userAccountRepository =
            userAccountRepository
            ?? throw new ArgumentNullException(
                nameof(userAccountRepository));

        _passwordHasher =
            passwordHasher
            ?? throw new ArgumentNullException(
                nameof(passwordHasher));

        _clock =
            clock
            ?? throw new ArgumentNullException(
                nameof(clock));
    }

    /// <summary>
    /// Executes password change workflow.
    /// </summary>
    /// <param name="command">
    /// Password change request.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Success when password change completes;
    /// otherwise a failure result.
    /// </returns>
    public async Task<Result> ExecuteAsync(
        ChangePasswordCommand command,
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

            var passwordValid =
                _passwordHasher.Verify(
                    command.CurrentPassword,
                    userAccount.PasswordHash);

            if (!passwordValid)
            {
                return Result.Failure(
                    IdentityErrors.InvalidPassword);
            }

            var newPasswordHash =
                _passwordHasher.Hash(
                    command.NewPassword);

            userAccount.ChangePassword(
                newPasswordHash,
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