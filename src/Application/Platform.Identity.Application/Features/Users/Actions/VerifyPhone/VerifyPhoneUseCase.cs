// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/VerifyPhone/VerifyPhoneUseCase.cs
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
/// Verifies a user's phone number.
///
/// Responsibility:
/// - Load user aggregate.
/// - Validate verification code.
/// - Execute phone verification workflow.
/// - Persist aggregate changes.
/// - Return application result.
///
/// Architectural Rules:
/// - Contains orchestration logic only.
/// - Does not contain verification implementation.
/// - Does not contain persistence implementation.
/// - Does not contain infrastructure concerns.
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
public sealed class VerifyPhoneUseCase : ICommandHandler<VerifyPhoneCommand>
{
    private readonly IUserAccountRepository
        _userAccountRepository;

    private readonly IVerificationCodeValidator
        _verificationCodeValidator;

    private readonly IClock
        _clock;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="VerifyPhoneUseCase"/> class.
    /// </summary>
    /// <param name="userAccountRepository">
    /// User account repository.
    /// </param>
    /// <param name="verificationCodeValidator">
    /// Verification code validator.
    /// </param>
    /// <param name="clock">
    /// UTC clock provider.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when dependency is null.
    /// </exception>
    public VerifyPhoneUseCase(
        IUserAccountRepository userAccountRepository,
        IVerificationCodeValidator verificationCodeValidator,
        IClock clock)
    {
        _userAccountRepository =
            userAccountRepository
            ?? throw new ArgumentNullException(
                nameof(userAccountRepository));

        _verificationCodeValidator =
            verificationCodeValidator
            ?? throw new ArgumentNullException(
                nameof(verificationCodeValidator));

        _clock =
            clock
            ?? throw new ArgumentNullException(
                nameof(clock));
    }

    /// <summary>
    /// Executes phone verification workflow.
    /// </summary>
    /// <param name="command">
    /// Phone verification request.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Success when phone verification completes;
    /// otherwise a failure result.
    /// </returns>
    public async Task<Result> ExecuteAsync(
        VerifyPhoneCommand command,
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

            var verificationSucceeded =
                await _verificationCodeValidator
                    .ValidateAsync(
                        command.UserId,
                        command.VerificationCode,
                        cancellationToken);

            if (!verificationSucceeded)
            {
                return Result.Failure(
                    IdentityErrors.InvalidVerificationCode);
            }

            userAccount.VerifyPhone(
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