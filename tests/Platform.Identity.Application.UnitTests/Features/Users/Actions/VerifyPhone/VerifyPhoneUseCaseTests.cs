using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Errors;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.Aggregates;
using Platform.SharedKernel.Abstractions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="VerifyPhoneUseCase"/>.
/// </summary>
public sealed class VerifyPhoneUseCaseTests
{
    private readonly Mock<IUserAccountRepository>
        _userAccountRepository = new();

    private readonly Mock<IVerificationCodeValidator>
        _verificationCodeValidator = new();

    private readonly Mock<IClock>
        _clock = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private VerifyPhoneUseCase CreateSut(
        IUserAccountRepository? userAccountRepository = null,
        IVerificationCodeValidator? verificationCodeValidator = null,
        IClock? clock = null)
    {
        return new VerifyPhoneUseCase(
            userAccountRepository
                ?? _userAccountRepository.Object,
            verificationCodeValidator
                ?? _verificationCodeValidator.Object,
            clock
                ?? _clock.Object);
    }

    /// <summary>
    /// Creates a valid command.
    /// </summary>
    private static VerifyPhoneCommand CreateCommand()
    {
        return new VerifyPhoneCommand(
            Guid.NewGuid(),
            "123456");
    }

    // ============================================================
    // Constructor
    // ============================================================

    [Fact]
    public void Constructor_Should_Create_Instance()
    {
        // Act

        var sut =
            CreateSut();

        // Assert

        sut.Should()
            .NotBeNull();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_UserAccountRepository_Is_Null()
    {
        // Act

        Action act =
            () => new VerifyPhoneUseCase(
                null!,
                _verificationCodeValidator.Object,
                _clock.Object);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "userAccountRepository");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_VerificationCodeValidator_Is_Null()
    {
        // Act

        Action act =
            () => new VerifyPhoneUseCase(
                _userAccountRepository.Object,
                null!,
                _clock.Object);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "verificationCodeValidator");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_Clock_Is_Null()
    {
        // Act

        Action act =
            () => new VerifyPhoneUseCase(
                _userAccountRepository.Object,
                _verificationCodeValidator.Object,
                null!);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "clock");
    }

    // ============================================================
    // ExecuteAsync
    // ============================================================

    [Fact]
    public async Task ExecuteAsync_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        // Arrange

        var sut =
            CreateSut();

        // Act

        Func<Task> act =
            () => sut.ExecuteAsync(
                null!,
                CancellationToken.None);

        // Assert

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(
                "command");
    }
    // ============================================================
    // ExecuteAsync
    // Failure
    // ============================================================

    /// <summary>
    /// Verifies failure is returned when
    /// user does not exist.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_UserNotFound_When_User_Does_Not_Exist()
    {
        // Arrange

        var command =
            CreateCommand();

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserAccount?)null);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeFalse();

        result.Error
            .Should()
            .Be(IdentityErrors.UserNotFound);

        _verificationCodeValidator.Verify(
            x => x.ValidateAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies failure is returned when
    /// verification code is invalid.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_InvalidVerificationCode_When_Code_Is_Invalid()
    {
        // Arrange

        var command =
            CreateCommand();

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _verificationCodeValidator
            .Setup(x =>
                x.ValidateAsync(
                    command.UserId,
                    command.VerificationCode,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeFalse();

        result.Error
            .Should()
            .Be(
                IdentityErrors.InvalidVerificationCode);

        _verificationCodeValidator.Verify(
            x => x.ValidateAsync(
                command.UserId,
                command.VerificationCode,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies repository update is not
    /// performed when verification fails.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Not_Update_User_When_Verification_Fails()
    {
        // Arrange

        var command =
            CreateCommand();

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _verificationCodeValidator
            .Setup(x =>
                x.ValidateAsync(
                    command.UserId,
                    command.VerificationCode,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies supplied cancellation token
    /// is forwarded to all collaborators.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Forward_CancellationToken()
    {
        // Arrange

        var token =
            new CancellationTokenSource().Token;

        var command =
            CreateCommand();

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    token))
            .ReturnsAsync(user);

        _verificationCodeValidator
            .Setup(x =>
                x.ValidateAsync(
                    command.UserId,
                    command.VerificationCode,
                    token))
            .ReturnsAsync(false);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(
            command,
            token);

        // Assert

        _userAccountRepository.Verify(
            x => x.GetByIdAsync(
                command.UserId,
                token),
            Times.Once);

        _verificationCodeValidator.Verify(
            x => x.ValidateAsync(
                command.UserId,
                command.VerificationCode,
                token),
            Times.Once);
    }

    // ============================================================
    // ExecuteAsync
    // Success
    // ============================================================

    /// <summary>
    /// Verifies phone is successfully verified.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_Phone_Is_Verified()
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
            CreateCommand();

        var user =
            UserAccountFixture.Create();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _verificationCodeValidator
            .Setup(x =>
                x.ValidateAsync(
                    command.UserId,
                    command.VerificationCode,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        user.PhoneVerified
            .Should()
            .BeTrue();

        user.UpdatedAt
            .Should()
            .Be(now);

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }

    /// <summary>
    /// Verifies verification code validator
    /// is invoked once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Validate_Verification_Code_Once()
    {
        // Arrange

        var command =
            CreateCommand();

        var user =
            UserAccountFixture.Create();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _verificationCodeValidator
            .Setup(x =>
                x.ValidateAsync(
                    command.UserId,
                    command.VerificationCode,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _verificationCodeValidator.Verify(
            x => x.ValidateAsync(
                command.UserId,
                command.VerificationCode,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies repository update is executed
    /// exactly once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Update_UserAccount_Once()
    {
        // Arrange

        var command =
            CreateCommand();

        var user =
            UserAccountFixture.Create();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _verificationCodeValidator
            .Setup(x =>
                x.ValidateAsync(
                    command.UserId,
                    command.VerificationCode,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }

    /// <summary>
    /// Verifies repository loads user
    /// exactly once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Load_User_Once()
    {
        // Arrange

        var command =
            CreateCommand();

        var user =
            UserAccountFixture.Create();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _verificationCodeValidator
            .Setup(x =>
                x.ValidateAsync(
                    command.UserId,
                    command.VerificationCode,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _userAccountRepository.Verify(
            x => x.GetByIdAsync(
                command.UserId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies already verified phone
    /// still returns success.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_Phone_Is_Already_Verified()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand();

        var user =
            UserAccountFixture.CreatePhoneVerified();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _verificationCodeValidator
            .Setup(x =>
                x.ValidateAsync(
                    command.UserId,
                    command.VerificationCode,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        user.PhoneVerified
            .Should()
            .BeTrue();

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }
}