// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/CreateUser/CreateUserUseCase.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Errors;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Creates a new user account.
///
/// Responsibility:
/// - Validate user uniqueness.
/// - Create user aggregate.
/// - Hash password.
/// - Persist aggregate.
/// - Return application result.
///
/// Architectural Rules:
/// - Contains orchestration logic only.
/// - Does not contain persistence implementation.
/// - Does not contain cryptographic implementation.
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
public sealed class CreateUserUseCase : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserAccountRepository
        _userAccountRepository;

    private readonly IPasswordHasher
        _passwordHasher;

    private readonly IClock
        _clock;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CreateUserUseCase"/> class.
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
    public CreateUserUseCase(
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
    /// Executes user creation workflow.
    /// </summary>
    /// <param name="command">
    /// User creation request.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Created user identifier when successful;
    /// otherwise a failure result.
    /// </returns>
    public async Task<Result<Guid>> ExecuteAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            command);

        try
        {
            var email =
                new EmailAddress(
                    command.Email);

            var phoneNumber =
                new PhoneNumber(
                    command.PhoneNumber);

            var usernameExists =
                await _userAccountRepository
                    .ExistsByUsernameAsync(
                        command.Username,
                        cancellationToken);

            if (usernameExists)
            {
                return Result<Guid>.Failure(
                    IdentityErrors.UsernameAlreadyExists);
            }

            var emailExists =
                await _userAccountRepository
                    .ExistsByEmailAsync(
                        email,
                        cancellationToken);

            if (emailExists)
            {
                return Result<Guid>.Failure(
                    IdentityErrors.EmailAlreadyExists);
            }

            var phoneExists =
                await _userAccountRepository
                    .ExistsByPhoneAsync(
                        phoneNumber,
                        cancellationToken);

            if (phoneExists)
            {
                return Result<Guid>.Failure(
                    IdentityErrors.PhoneAlreadyExists);
            }

            var passwordHash =
                _passwordHasher.Hash(
                    command.Password);

            var userAccount =
                new UserAccount(
                    Guid.NewGuid(),
                    command.Username,
                    email,
                    phoneNumber,
                    passwordHash,
                    _clock.UtcNow);

            await _userAccountRepository
                .AddAsync(
                    userAccount,
                    cancellationToken);

            return Result<Guid>.Success(
                userAccount.Id);
        }
        catch (DomainException exception)
        {
            return Result<Guid>.Failure(
                IdentityErrorMapper.Map(
                    exception));
        }
    }
}