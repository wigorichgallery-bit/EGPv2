// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/Authentication/Actions/Login/
// LoginUseCase.cs
// ===========================================

using System.Linq;
using Microsoft.Extensions.Logging;

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Configuration.Authentication;
using Platform.Identity.Application.Contracts.Authentication.Enums;
using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.Identity.Application.Contracts.Authentication.Responses;
using Platform.Identity.Application.Errors;
using Platform.Identity.Application.Features.Authentication.Mapping;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Application.Features.Authentication.Policies.Contracts;
using Platform.Identity.Application.Features.Authentication.Policies.Models;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Results;

namespace Platform.Identity.Application.Features.Authentication.Actions;

/// <summary>
/// Coordinates the complete user authentication workflow.
///
/// Responsibilities:
/// <list type="bullet">
/// <item><description>Resolve authentication identity.</description></item>
/// <item><description>Verify supplied credentials.</description></item>
/// <item><description>Evaluate authentication policies.</description></item>
/// <item><description>Coordinate authentication challenges.</description></item>
/// <item><description>Generate authentication tokens.</description></item>
/// <item><description>Persist authentication state changes.</description></item>
/// </list>
///
/// This class acts only as the application orchestration layer.
/// Domain rules remain inside the domain model and dedicated
/// application services.
/// </summary>
public sealed class LoginUseCase
    : ICommandHandler<LoginCommand, LoginResponse>
{
    // =========================================================
    // Repositories
    // =========================================================

    private readonly IUserAccountRepository
        _userAccountRepository;

    private readonly IRoleQueryRepository
        _roleQueryRepository;

    private readonly IAuthenticationChallengeRepository
        _authenticationChallengeRepository;

    // =========================================================
    // Authentication Services
    // =========================================================

    private readonly IAuthenticationIdentityResolver
        _identityResolver;

    private readonly IPasswordHasher
        _passwordHasher;

    private readonly ITokenService
        _tokenService;

    private readonly IAuthenticationPolicyEvaluator
        _authenticationPolicyEvaluator;

    private readonly IAuthenticationChallengeBuilder
        _authenticationChallengeBuilder;

    private readonly IAuthenticationChallengeDeliveryService
        _authenticationChallengeDeliveryService;

    // =========================================================
    // Infrastructure
    // =========================================================

    private readonly IClock
        _clock;

    private readonly IUnitOfWork
        _unitOfWork;

    private readonly ILogger<LoginUseCase>
        _logger;

    private readonly AuthenticationOptions
        _authenticationOptions;

    // =========================================================
    // Constructor
    // =========================================================

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="LoginUseCase"/> class.
    /// </summary>
    public LoginUseCase(
        IUserAccountRepository userAccountRepository,
        IRoleQueryRepository roleQueryRepository,
        IAuthenticationChallengeRepository authenticationChallengeRepository,
        IAuthenticationIdentityResolver identityResolver,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IAuthenticationPolicyEvaluator authenticationPolicyEvaluator,
        IAuthenticationChallengeBuilder authenticationChallengeBuilder,
        IAuthenticationChallengeDeliveryService authenticationChallengeDeliveryService,
        IClock clock,
        IUnitOfWork unitOfWork,
        ILogger<LoginUseCase> logger,
        AuthenticationOptions authenticationOptions)
    {
        ArgumentNullException.ThrowIfNull(userAccountRepository);
        ArgumentNullException.ThrowIfNull(roleQueryRepository);
        ArgumentNullException.ThrowIfNull(authenticationChallengeRepository);
        ArgumentNullException.ThrowIfNull(identityResolver);
        ArgumentNullException.ThrowIfNull(passwordHasher);
        ArgumentNullException.ThrowIfNull(tokenService);
        ArgumentNullException.ThrowIfNull(authenticationPolicyEvaluator);
        ArgumentNullException.ThrowIfNull(authenticationChallengeBuilder);
        ArgumentNullException.ThrowIfNull(authenticationChallengeDeliveryService);
        ArgumentNullException.ThrowIfNull(clock);
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(authenticationOptions);

        _userAccountRepository =
            userAccountRepository;

        _roleQueryRepository =
            roleQueryRepository;

        _authenticationChallengeRepository =
            authenticationChallengeRepository;

        _identityResolver =
            identityResolver;

        _passwordHasher =
            passwordHasher;

        _tokenService =
            tokenService;

        _authenticationPolicyEvaluator =
            authenticationPolicyEvaluator;

        _authenticationChallengeBuilder =
            authenticationChallengeBuilder;

        _authenticationChallengeDeliveryService =
            authenticationChallengeDeliveryService;

        _clock =
            clock;

        _unitOfWork =
            unitOfWork;

        _logger =
            logger;

        _authenticationOptions =
            authenticationOptions;
    }

    /// <summary>
    /// Executes the authentication workflow.
    /// </summary>
    public async Task<Result<LoginResponse>> ExecuteAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // =====================================================
        // STEP 1
        // Resolve authentication identity.
        // =====================================================

        UserAccount? user =
            await _identityResolver.ResolveAsync(
                command.Identity,
                cancellationToken);

        if (user is null)
        {
            _logger.LogWarning(
                "Authentication failed. Identity '{Identity}' was not found.",
                command.Identity);

            return Result<LoginResponse>.Failure(
                IdentityErrors.InvalidCredentials);
        }

        // =====================================================
        // STEP 2
        // Verify supplied password.
        // =====================================================

        bool passwordVerified =
            _passwordHasher.Verify(
                command.Password,
                user.PasswordHash);

        if (!passwordVerified)
        {
            // =================================================
            // STEP 3
            // Register failed login attempt.
            // =================================================

            var currentUtc =
                _clock.UtcNow;

            user.RegisterFailedLoginAttempt(
                _authenticationOptions.LockoutThreshold,
                _authenticationOptions.LockoutDuration,
                currentUtc);

            _userAccountRepository.Update(
                user);

            await _unitOfWork.CommitAsync(
                cancellationToken);

            _logger.LogWarning(
                "Authentication failed. Invalid password for identity '{Identity}'.",
                command.Identity);

            return Result<LoginResponse>.Failure(
                IdentityErrors.InvalidCredentials);
        }

        // =====================================================
        // STEP 4
        // Evaluate account status.
        // =====================================================

        switch (user.Status)
        {
            case UserStatus.Locked:

                _logger.LogWarning(
                    "Authentication rejected. User account '{UserId}' is locked.",
                    user.Id);

                return Result<LoginResponse>.Failure(
                    IdentityErrors.UserLocked);

            case UserStatus.Disabled:

                _logger.LogWarning(
                    "Authentication rejected. User account '{UserId}' is disabled.",
                    user.Id);

                return Result<LoginResponse>.Failure(
                    IdentityErrors.UserDisabled);
        }

        // =====================================================
        // STEP 5
        // Evaluate authentication policies.
        // =====================================================

        var loginRequest =
            new LoginRequest(
                command.Identity,
                command.Password);

        var authenticationContext =
            new AuthenticationContext(
                user,
                loginRequest,
                _clock.UtcNow);

        var policyResult =
            await _authenticationPolicyEvaluator
                .EvaluateAsync(
                    authenticationContext,
                    cancellationToken);

        if (!policyResult.ShouldContinue)
        {
            switch (policyResult.Decision.Decision)
            {
                case AuthenticationDecisionType.RequireVerification:

                    _logger.LogInformation(
                        "Authentication requires account verification for user '{UserId}'.",
                        user.Id);

                    return Result<LoginResponse>.Failure(
                        IdentityErrors.AccountVerificationRequired);

                case AuthenticationDecisionType.RequirePasswordReset:

                    _logger.LogInformation(
                        "Authentication requires password reset for user '{UserId}'.",
                        user.Id);

                    return Result<LoginResponse>.Failure(
                        IdentityErrors.PasswordResetRequired);

                case AuthenticationDecisionType.LockAccount:

                    _logger.LogWarning(
                        "Authentication policy requested account lock for user '{UserId}'.",
                        user.Id);

                    return Result<LoginResponse>.Failure(
                        IdentityErrors.UserLocked);

                case AuthenticationDecisionType.Deny:

                    _logger.LogWarning(
                        "Authentication denied for user '{UserId}'. Reason: {Reason}",
                        user.Id,
                        policyResult.Decision.Reason);

                    return Result<LoginResponse>.Failure(
                        IdentityErrors.InvalidCredentials);

                case AuthenticationDecisionType.RequireChallenge:

                    // Continue to STEP 6.
                    break;

                case AuthenticationDecisionType.Allow:
                default:
                    break;
            }
        }

        // =====================================================
        // STEP 6
        // Build and deliver authentication challenge.
        // =====================================================

        if (policyResult.Decision.Decision ==
            AuthenticationDecisionType.RequireChallenge)
        {
            var challengeResult =
                _authenticationChallengeBuilder.Build(
                    user,
                    Platform.Identity.Domain.Enums
                        .AuthenticationChallengePurpose.Login);

            await _authenticationChallengeRepository.AddAsync(
                challengeResult.Challenge,
                cancellationToken);

            await _authenticationChallengeDeliveryService.DeliverAsync(
                new AuthenticationChallengeDeliveryRequest(
                    challengeResult.Challenge,
                    user,
                    challengeResult.PlainTextSecret),
                cancellationToken);

            await _unitOfWork.CommitAsync(
                cancellationToken);

            _logger.LogInformation(
                "Authentication challenge '{ChallengeId}' created for user '{UserId}'.",
                challengeResult.Challenge.Id,
                user.Id);

            return Result<LoginResponse>.Success(
                new LoginResponse(
                    AuthenticationStatus.ChallengeRequired,
                    Token: null,
                    ChallengeId:
                        challengeResult.Challenge.Id,
                    ChallengeType:
                        AuthenticationChallengeTypeMapper
                            .ToContract(
                                challengeResult.Challenge.ChallengeType),
                    ChallengePurpose:
                        AuthenticationChallengePurposeMapper
                            .ToContract(
                                challengeResult.Challenge.Purpose),
                    ChallengeExpiresAtUtc:
                        challengeResult.Challenge.ExpiresAtUtc));
        }

        // =====================================================
        // STEP 7
        // Record successful authentication.
        // =====================================================

        user.RecordSuccessfulLogin(
            _clock.UtcNow);

        _userAccountRepository.Update(
            user);

        // =====================================================
        // STEP 8
        // Load authorization data.
        // =====================================================

        var roleIds =
            user.RoleAssignments
                .Select(
                    static assignment => assignment.RoleId)
                .ToArray();

        var roles =
            await _roleQueryRepository.FindByIdsAsync(
                roleIds,
                cancellationToken);

        // -----------------------------------------------------
        // NOTE
        // Replace these projections only if the verified RoleDto
        // contract uses different property names.
        // -----------------------------------------------------

        IReadOnlyCollection<string> roleNames =
            roles
                .Select(
                    static role => role.Name)
                .ToArray();

        IReadOnlyCollection<string> permissions =
            roles
                .SelectMany(
                    static role => role.PermissionIds)
                .Distinct(
                    StringComparer.OrdinalIgnoreCase)
                .ToArray();

        // =====================================================
        // STEP 9
        // Generate authentication token.
        // =====================================================

        var tokenRequest =
            new TokenGenerationRequest(
                user.Id,
                user.Username,
                user.Email.Value,
                user.SecurityStamp,
                roleNames,
                permissions);

        var token =
            await _tokenService.GenerateTokenAsync(
                tokenRequest,
                cancellationToken);

        // =====================================================
        // STEP 10
        // Persist successful authentication.
        // =====================================================

        await _unitOfWork.CommitAsync(
            cancellationToken);

        _logger.LogInformation(
            "User '{UserId}' authenticated successfully.",
            user.Id);

        // =====================================================
        // STEP 11
        // Return successful authentication response.
        // =====================================================

        return Result<LoginResponse>.Success(
            new LoginResponse(
                AuthenticationStatus.Success,
                Token: token,
                ChallengeId: null,
                ChallengeType: null,
                ChallengePurpose: null,
                ChallengeExpiresAtUtc: null));
    }
}