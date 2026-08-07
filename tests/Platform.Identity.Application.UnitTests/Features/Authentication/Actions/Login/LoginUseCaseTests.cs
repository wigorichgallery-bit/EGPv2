using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Configuration.Authentication;
using Platform.Identity.Application.Contracts.Authentication.Enums;
using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Identity.Application.Errors;
using Platform.Identity.Application.Features.Authentication.Actions;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Application.Features.Authentication.Policies.Contracts;
using Platform.Identity.Application.Features.Authentication.Policies.Models;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Abstractions;
using Xunit;

using ContractChallengeType =
    Platform.Identity.Application.Contracts.Authentication.Enums.AuthenticationChallengeType;

using ContractChallengePurpose =
    Platform.Identity.Application.Contracts.Authentication.Enums.AuthenticationChallengePurpose;

using DomainChallengeType =
    Platform.Identity.Domain.Enums.AuthenticationChallengeType;

using DomainChallengePurpose =
    Platform.Identity.Domain.Enums.AuthenticationChallengePurpose;
using Platform.Identity.Application.Contracts.Authentication.Dtos;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Actions;

/// <summary>
/// Unit tests for <see cref="LoginUseCase"/>.
/// </summary>
public sealed partial class LoginUseCaseTests
{
    private readonly Mock<IUserAccountRepository>
        _userAccountRepository = new();

    private readonly Mock<IRoleQueryRepository>
        _roleQueryRepository = new();

    private readonly Mock<IAuthenticationChallengeRepository>
        _authenticationChallengeRepository = new();

    private readonly Mock<IAuthenticationIdentityResolver>
        _identityResolver = new();

    private readonly Mock<IPasswordHasher>
        _passwordHasher = new();

    private readonly Mock<ITokenService>
        _tokenService = new();

    private readonly Mock<IAuthenticationPolicyEvaluator>
        _authenticationPolicyEvaluator = new();

    private readonly Mock<IAuthenticationChallengeBuilder>
        _authenticationChallengeBuilder = new();

    private readonly Mock<IAuthenticationChallengeDeliveryService>
        _authenticationChallengeDeliveryService = new();

    private readonly Mock<IClock>
        _clock = new();

    private readonly Mock<IUnitOfWork>
        _unitOfWork = new();

    private readonly Mock<ILogger<LoginUseCase>>
        _logger = new();

    private readonly AuthenticationOptions
        _authenticationOptions = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private LoginUseCase CreateSut()
    {
        return new LoginUseCase(
            _userAccountRepository.Object,
            _roleQueryRepository.Object,
            _authenticationChallengeRepository.Object,
            _identityResolver.Object,
            _passwordHasher.Object,
            _tokenService.Object,
            _authenticationPolicyEvaluator.Object,
            _authenticationChallengeBuilder.Object,
            _authenticationChallengeDeliveryService.Object,
            _clock.Object,
            _unitOfWork.Object,
            _logger.Object,
            _authenticationOptions);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_UserAccountRepository_Is_Null()
    {
  Action act = () =>
        new LoginUseCase(
            null!,
            _roleQueryRepository.Object,
            _authenticationChallengeRepository.Object,
            _identityResolver.Object,
            _passwordHasher.Object,
            _tokenService.Object,
            _authenticationPolicyEvaluator.Object,
            _authenticationChallengeBuilder.Object,
            _authenticationChallengeDeliveryService.Object,
            _clock.Object,
            _unitOfWork.Object,
            _logger.Object,
            _authenticationOptions);

    act.Should()
        .Throw<ArgumentNullException>()
        .WithParameterName("userAccountRepository");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// role query repository is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_RoleQueryRepository_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                null!,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("roleQueryRepository");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// authentication challenge repository is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_AuthenticationChallengeRepository_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                null!,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("authenticationChallengeRepository");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// identity resolver is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_IdentityResolver_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                null!,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("identityResolver");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// password hasher is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_PasswordHasher_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                null!,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("passwordHasher");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// token service is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_TokenService_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                null!,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("tokenService");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// authentication policy evaluator is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_AuthenticationPolicyEvaluator_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                null!,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("authenticationPolicyEvaluator");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// authentication challenge builder is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_AuthenticationChallengeBuilder_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                null!,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("authenticationChallengeBuilder");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// authentication challenge delivery service is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_AuthenticationChallengeDeliveryService_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                null!,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("authenticationChallengeDeliveryService");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// clock is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_Clock_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                null!,
                _unitOfWork.Object,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("clock");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// unit of work is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_UnitOfWork_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                null!,
                _logger.Object,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("unitOfWork");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// logger is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_Logger_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                null!,
                _authenticationOptions);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// authentication options are null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_AuthenticationOptions_Is_Null()
    {
        // Arrange
        Action act = () =>
            new LoginUseCase(
                _userAccountRepository.Object,
                _roleQueryRepository.Object,
                _authenticationChallengeRepository.Object,
                _identityResolver.Object,
                _passwordHasher.Object,
                _tokenService.Object,
                _authenticationPolicyEvaluator.Object,
                _authenticationChallengeBuilder.Object,
                _authenticationChallengeDeliveryService.Object,
                _clock.Object,
                _unitOfWork.Object,
                _logger.Object,
                null!);

        // Act & Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("authenticationOptions");
    }
    
    /// <summary>
    /// Verifies authentication fails when
    /// the supplied identity cannot be resolved.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_InvalidCredentials_When_User_Not_Found()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        _identityResolver
            .Setup(x =>
                x.ResolveAsync(
                    command.Identity,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((Platform.Identity.Domain.Aggregates.UserAccount?)null);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsFailure
            .Should()
            .BeTrue();

        result.Error
            .Should()
            .BeSameAs(
                IdentityErrors.InvalidCredentials);

        _passwordHasher.Verify(
            x => x.Verify(
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);

        _unitOfWork.Verify(
            x => x.CommitAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies authentication fails when
    /// the supplied password is invalid.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_InvalidCredentials_When_Password_Is_Invalid()
    {
        // Arrange

        var now =
            new DateTime(
                2026,
                1,
                1,
                12,
                0,
                0,
                DateTimeKind.Utc);

        var command =
            new LoginCommand(
                "john.doe",
                "WrongPassword");

        var user =
            UserAccountFixture.Create();

        _identityResolver
            .Setup(x =>
                x.ResolveAsync(
                    command.Identity,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.Password,
                    user.PasswordHash))
            .Returns(false);

        _clock
            .SetupGet(x =>
                x.UtcNow)
            .Returns(now);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsFailure
            .Should()
            .BeTrue();

        result.Error
            .Should()
            .BeSameAs(
                IdentityErrors.InvalidCredentials);

        _userAccountRepository.Verify(
            x => x.Update(user),
            Times.Once);

        _unitOfWork.Verify(
            x => x.CommitAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);

        _roleQueryRepository.Verify(
            x => x.FindByIdsAsync(
                It.IsAny<IReadOnlyCollection<Guid>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _tokenService.Verify(
            x => x.GenerateTokenAsync(
                It.IsAny<Platform.Identity.Application.Contracts.Authentication.Requests.TokenGenerationRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _authenticationPolicyEvaluator.Verify(
            x => x.EvaluateAsync(
                It.IsAny<Platform.Identity.Application.Features.Authentication.Policies.Models.AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _authenticationChallengeBuilder.Verify(
            x => x.Build(
                It.IsAny<Platform.Identity.Domain.Aggregates.UserAccount>(),
                It.IsAny<Platform.Identity.Domain.Enums.AuthenticationChallengePurpose>()),
            Times.Never);

        _authenticationChallengeRepository.Verify(
            x => x.AddAsync(
                It.IsAny<Platform.Identity.Domain.Aggregates.AuthenticationChallenge>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _authenticationChallengeDeliveryService.Verify(
            x => x.DeliverAsync(
                It.IsAny<Platform.Identity.Application.Features.Authentication.Models.AuthenticationChallengeDeliveryRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies authentication is rejected when
    /// the user account is locked.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_UserLocked_When_User_Is_Locked()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.CreateLocked();

        _identityResolver
            .Setup(x =>
                x.ResolveAsync(
                    command.Identity,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.Password,
                    user.PasswordHash))
            .Returns(true);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsFailure
            .Should()
            .BeTrue();

        result.Error
            .Should()
            .BeSameAs(
                IdentityErrors.UserLocked);

        _authenticationPolicyEvaluator.Verify(
            x => x.EvaluateAsync(
                It.IsAny<Platform.Identity.Application.Features.Authentication.Policies.Models.AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _tokenService.Verify(
            x => x.GenerateTokenAsync(
                It.IsAny<Platform.Identity.Application.Contracts.Authentication.Requests.TokenGenerationRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWork.Verify(
            x => x.CommitAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies authentication is rejected when
    /// the user account is disabled.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_UserDisabled_When_User_Is_Disabled()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.Create();

        var now = DateTime.UtcNow;

        user.Disable(now);

        _identityResolver
            .Setup(x =>
                x.ResolveAsync(
                    command.Identity,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.Password,
                    user.PasswordHash))
            .Returns(true);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsFailure
            .Should()
            .BeTrue();

        result.Error
            .Should()
            .BeSameAs(
                IdentityErrors.UserDisabled);

        _authenticationPolicyEvaluator.Verify(
            x => x.EvaluateAsync(
                It.IsAny<Platform.Identity.Application.Features.Authentication.Policies.Models.AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _tokenService.Verify(
            x => x.GenerateTokenAsync(
                It.IsAny<Platform.Identity.Application.Contracts.Authentication.Requests.TokenGenerationRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _authenticationChallengeBuilder.Verify(
            x => x.Build(
                It.IsAny<Platform.Identity.Domain.Aggregates.UserAccount>(),
                It.IsAny<Platform.Identity.Domain.Enums.AuthenticationChallengePurpose>()),
            Times.Never);

        _authenticationChallengeRepository.Verify(
            x => x.AddAsync(
                It.IsAny<Platform.Identity.Domain.Aggregates.AuthenticationChallenge>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _authenticationChallengeDeliveryService.Verify(
            x => x.DeliverAsync(
                It.IsAny<Platform.Identity.Application.Features.Authentication.Models.AuthenticationChallengeDeliveryRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWork.Verify(
            x => x.CommitAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies authentication fails when
    /// account verification is required.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_AccountVerificationRequired_When_Policy_Requires_Verification()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.Create();

        _identityResolver
            .Setup(x => x.ResolveAsync(
                command.Identity,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x => x.Verify(
                command.Password,
                user.PasswordHash))
            .Returns(true);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _authenticationPolicyEvaluator
            .Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                PolicyEvaluationResult.Stop(
                    AuthenticationDecision.RequireVerification()));

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeSameAs(
            IdentityErrors.AccountVerificationRequired);

        _authenticationChallengeBuilder.VerifyNoOtherCalls();
        _authenticationChallengeRepository.VerifyNoOtherCalls();
        _authenticationChallengeDeliveryService.VerifyNoOtherCalls();
        _tokenService.VerifyNoOtherCalls();

        _unitOfWork.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies authentication fails when
    /// password reset is required.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_PasswordResetRequired_When_Policy_Requires_Password_Reset()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.Create();

        _identityResolver
            .Setup(x => x.ResolveAsync(
                command.Identity,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x => x.Verify(
                command.Password,
                user.PasswordHash))
            .Returns(true);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _authenticationPolicyEvaluator
            .Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                PolicyEvaluationResult.Stop(
                    AuthenticationDecision.RequirePasswordReset()));

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeSameAs(
            IdentityErrors.PasswordResetRequired);

        _authenticationChallengeBuilder.VerifyNoOtherCalls();
        _authenticationChallengeRepository.VerifyNoOtherCalls();
        _authenticationChallengeDeliveryService.VerifyNoOtherCalls();
        _tokenService.VerifyNoOtherCalls();

        _unitOfWork.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies authentication fails when
    /// policy denies authentication.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_InvalidCredentials_When_Policy_Denies_Authentication()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.Create();

        _identityResolver
            .Setup(x => x.ResolveAsync(
                command.Identity,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x => x.Verify(
                command.Password,
                user.PasswordHash))
            .Returns(true);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _authenticationPolicyEvaluator
            .Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                PolicyEvaluationResult.Stop(
                    AuthenticationDecision.Deny(
                        "Denied")));

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeSameAs(
            IdentityErrors.InvalidCredentials);

        _authenticationChallengeBuilder.VerifyNoOtherCalls();
        _authenticationChallengeRepository.VerifyNoOtherCalls();
        _authenticationChallengeDeliveryService.VerifyNoOtherCalls();
        _tokenService.VerifyNoOtherCalls();

        _unitOfWork.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies authentication fails when
    /// policy requests account lock.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_UserLocked_When_Policy_Requests_Account_Lock()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.Create();

        _identityResolver
            .Setup(x => x.ResolveAsync(
                command.Identity,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x => x.Verify(
                command.Password,
                user.PasswordHash))
            .Returns(true);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _authenticationPolicyEvaluator
            .Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                PolicyEvaluationResult.Stop(
                    AuthenticationDecision.LockAccount()));

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeSameAs(
            IdentityErrors.UserLocked);

        _authenticationChallengeBuilder.VerifyNoOtherCalls();
        _authenticationChallengeRepository.VerifyNoOtherCalls();
        _authenticationChallengeDeliveryService.VerifyNoOtherCalls();
        _tokenService.VerifyNoOtherCalls();

        _unitOfWork.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies authentication challenge is created,
    /// persisted and delivered when required by policy.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_ChallengeRequired_When_Policy_Requires_Challenge()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.CreateEmailMfaUser();

        var challengeSecret =
            new ChallengeSecret("HASHED_SECRET");

        var challenge =
            AuthenticationChallengeFixture.Create(
                challengeSecret,
                challengeType: DomainChallengeType.EmailOtp,
                purpose: DomainChallengePurpose.Login);

        var buildResult =
            new AuthenticationChallengeBuildResult(
                challenge,
                "123456");

        _identityResolver
            .Setup(x =>
                x.ResolveAsync(
                    command.Identity,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.Password,
                    user.PasswordHash))
            .Returns(true);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _authenticationPolicyEvaluator
            .Setup(x =>
                x.EvaluateAsync(
                    It.IsAny<AuthenticationContext>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                PolicyEvaluationResult.Stop(
                    AuthenticationDecision.RequireChallenge()));

        _authenticationChallengeBuilder
            .Setup(x =>
                x.Build(
                    user,
                    DomainChallengePurpose.Login))
            .Returns(buildResult);

        var sut = CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess.Should().BeTrue();

        result.Value.Status.Should()
            .Be(AuthenticationStatus.ChallengeRequired);

        result.Value.Token.Should().BeNull();

        result.Value.ChallengeId.Should()
            .Be(challenge.Id);

        result.Value.ChallengeType.Should()
            .Be(ContractChallengeType.EmailOtp);

        result.Value.ChallengePurpose.Should()
            .Be(ContractChallengePurpose.Login);

        result.Value.ChallengeExpiresAtUtc.Should()
            .Be(challenge.ExpiresAtUtc);

        _authenticationChallengeBuilder.Verify(
            x => x.Build(
                user,
                DomainChallengePurpose.Login),
            Times.Once);

        _authenticationChallengeRepository.Verify(
            x => x.AddAsync(
                challenge,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _authenticationChallengeDeliveryService.Verify(
            x => x.DeliverAsync(
                It.Is<AuthenticationChallengeDeliveryRequest>(
                    r =>
                        r.Challenge == challenge &&
                        r.User == user &&
                        r.PlainTextSecret == "123456"),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.CommitAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);

        _roleQueryRepository.Verify(
            x => x.FindByIdsAsync(
                It.IsAny<IReadOnlyCollection<Guid>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _tokenService.Verify(
            x => x.GenerateTokenAsync(
                It.IsAny<TokenGenerationRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies successful authentication returns
    /// an authentication token.
    /// </summary>
    /// <summary>
    /// Verifies successful authentication returns
    /// an authentication token.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_Authentication_Succeeds()
    {
        // Arrange

        var now =
            new DateTime(
                2026,
                1,
                1,
                12,
                0,
                0,
                DateTimeKind.Utc);

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.CreateFullyVerified();

        var role =
            new RoleDto(
                Guid.NewGuid(),
                "Administrator",
                false,
                "Global",
                true,
                new[]
                {
                "USER.READ",
                "USER.WRITE",
                "USER.READ"
                });

        var token =
            new AuthenticationTokenDto(
                "ACCESS_TOKEN",
                "REFRESH_TOKEN",
                "Bearer",
                3600,
                now.AddHours(1));

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _identityResolver
            .Setup(x =>
                x.ResolveAsync(
                    command.Identity,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.Password,
                    user.PasswordHash))
            .Returns(true);

        _authenticationPolicyEvaluator
            .Setup(x =>
                x.EvaluateAsync(
                    It.IsAny<AuthenticationContext>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                PolicyEvaluationResult.Continue());

        _roleQueryRepository
            .Setup(x =>
                x.FindByIdsAsync(
                    It.IsAny<IReadOnlyCollection<Guid>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]
            {
            role
            });

        _tokenService
            .Setup(x =>
                x.GenerateTokenAsync(
                    It.IsAny<TokenGenerationRequest>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(token);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(
                command,
                CancellationToken.None);

        // Assert

        result.IsSuccess.Should().BeTrue();

        result.Value.Status.Should()
            .Be(AuthenticationStatus.Success);

        result.Value.Token.Should()
            .BeSameAs(token);

        result.Value.ChallengeId.Should().BeNull();

        result.Value.ChallengeType.Should().BeNull();

        result.Value.ChallengePurpose.Should().BeNull();

        result.Value.ChallengeExpiresAtUtc.Should().BeNull();

        _userAccountRepository.Verify(
            x => x.Update(user),
            Times.Once);

        _roleQueryRepository.Verify(
            x => x.FindByIdsAsync(
                It.IsAny<IReadOnlyCollection<Guid>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _tokenService.Verify(
            x => x.GenerateTokenAsync(
                It.Is<TokenGenerationRequest>(
                    request =>
                        request.UserId == user.Id &&
                        request.Username == user.Username &&
                        request.Email == user.Email.Value &&
                        request.SecurityStamp == user.SecurityStamp &&
                        request.Roles.SequenceEqual(
                            new[]
                            {
                            "Administrator"
                            }) &&
                        request.Permissions.Count == 2 &&
                        request.Permissions.Contains("USER.READ") &&
                        request.Permissions.Contains("USER.WRITE")),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _authenticationChallengeBuilder.Verify(
            x => x.Build(
                It.IsAny<UserAccount>(),
                It.IsAny<Platform.Identity.Domain.Enums.AuthenticationChallengePurpose>()),
            Times.Never);

        _authenticationChallengeRepository.Verify(
            x => x.AddAsync(
                It.IsAny<AuthenticationChallenge>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _authenticationChallengeDeliveryService.Verify(
            x => x.DeliverAsync(
                It.IsAny<AuthenticationChallengeDeliveryRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWork.Verify(
            x => x.CommitAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies ExecuteAsync throws when command is null.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        // Arrange

        var sut =
            CreateSut();

        // Act

        Func<Task> action =
            () => sut.ExecuteAsync(
                null!,
                CancellationToken.None);

        // Assert

        await action
            .Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("command");
    }

    /// <summary>
    /// Verifies the supplied cancellation token
    /// is propagated to downstream services.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Forward_CancellationToken_To_Dependencies()
    {
        // Arrange

        var cancellationToken =
            new CancellationTokenSource().Token;

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.CreateFullyVerified();

        _identityResolver
            .Setup(x =>
                x.ResolveAsync(
                    command.Identity,
                    cancellationToken))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.Password,
                    user.PasswordHash))
            .Returns(true);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _authenticationPolicyEvaluator
            .Setup(x =>
                x.EvaluateAsync(
                    It.IsAny<AuthenticationContext>(),
                    cancellationToken))
            .ReturnsAsync(
                PolicyEvaluationResult.Continue());

        _roleQueryRepository
            .Setup(x =>
                x.FindByIdsAsync(
                    It.IsAny<IReadOnlyCollection<Guid>>(),
                    cancellationToken))
            .ReturnsAsync(Array.Empty<RoleDto>());

        _tokenService
            .Setup(x =>
                x.GenerateTokenAsync(
                    It.IsAny<Platform.Identity.Application.Contracts.Authentication.Requests.TokenGenerationRequest>(),
                    cancellationToken))
            .ReturnsAsync(
                new Platform.Identity.Application.Contracts.Authentication.Dtos.AuthenticationTokenDto(
                    "ACCESS",
                    "REFRESH",
                    "Bearer",
                    3600,
                    DateTime.UtcNow.AddHours(1)));

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(
            command,
            cancellationToken);

        // Assert

        _identityResolver.VerifyAll();

        _authenticationPolicyEvaluator.VerifyAll();

        _roleQueryRepository.VerifyAll();

        _tokenService.VerifyAll();

        _unitOfWork.Verify(
            x => x.CommitAsync(
                cancellationToken),
            Times.Once);
    }

    /// <summary>
    /// Verifies duplicate permissions from
    /// multiple roles are removed.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Remove_Duplicate_Permissions_From_Multiple_Roles()
    {
        // Arrange

        var command =
            new LoginCommand(
                "john.doe",
                "Password123!");

        var user =
            UserAccountFixture.CreateFullyVerified();

        var roles =
            new[]
            {
                new RoleDto(
                    Guid.NewGuid(),
                    "Administrator",
                    false,
                    "Global",
                    true,
                    new[]
                    {
                        "USER.READ",
                        "USER.WRITE"
                    }),

                new RoleDto(
                    Guid.NewGuid(),
                    "Auditor",
                    false,
                    "Global",
                    true,
                    new[]
                    {
                        "USER.READ",
                        "REPORT.READ"
                    })
            };

        _identityResolver
            .Setup(x =>
                x.ResolveAsync(
                    command.Identity,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.Password,
                    user.PasswordHash))
            .Returns(true);

        _clock
            .SetupGet(x =>
                x.UtcNow)
            .Returns(DateTime.UtcNow);

        _authenticationPolicyEvaluator
            .Setup(x =>
                x.EvaluateAsync(
                    It.IsAny<AuthenticationContext>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                PolicyEvaluationResult.Continue());

        _roleQueryRepository
            .Setup(x =>
                x.FindByIdsAsync(
                    It.IsAny<IReadOnlyCollection<Guid>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(roles);

        _tokenService
            .Setup(x =>
                x.GenerateTokenAsync(
                    It.Is<TokenGenerationRequest>(
                        r =>
                            r.Roles.Count == 2 &&
                            r.Permissions.Count == 3 &&
                            r.Permissions.Contains("USER.READ") &&
                            r.Permissions.Contains("USER.WRITE") &&
                            r.Permissions.Contains("REPORT.READ")),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new Platform.Identity.Application.Contracts.Authentication.Dtos.AuthenticationTokenDto(
                    "ACCESS",
                    "REFRESH",
                    "Bearer",
                    3600,
                    DateTime.UtcNow.AddHours(1)));

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        _tokenService.VerifyAll();
    }

    
}