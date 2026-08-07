using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Errors;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.SharedKernel.Abstractions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="EnableMfaUseCase"/>.
/// </summary>
public sealed class EnableMfaUseCaseTests
{
    private readonly Mock<IUserAccountRepository>
        _userAccountRepository = new();

    private readonly Mock<IClock>
        _clock = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private EnableMfaUseCase CreateSut(
        IUserAccountRepository? userAccountRepository = null,
        IClock? clock = null)
    {
        return new EnableMfaUseCase(
            userAccountRepository
                ?? _userAccountRepository.Object,
            clock
                ?? _clock.Object);
    }

    // ============================================================
    // Constructor
    // ============================================================

    /// <summary>
    /// Verifies constructor creates
    /// the use case successfully.
    /// </summary>
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

    /// <summary>
    /// Verifies constructor throws when
    /// userAccountRepository is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_UserAccountRepository_Is_Null()
    {
        // Act

        Action act =
            () => new EnableMfaUseCase(
                null!,
                _clock.Object);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "userAccountRepository");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// clock is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_Clock_Is_Null()
    {
        // Act

        Action act =
            () => new EnableMfaUseCase(
                _userAccountRepository.Object,
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

    /// <summary>
    /// Verifies ExecuteAsync throws when
    /// command is null.
    /// </summary>
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

    /// <summary>
    /// Creates a valid command.
    /// </summary>
    private static EnableMfaCommand CreateCommand(
        Platform.Identity.Domain.Enums.MFAMethod method =
            Platform.Identity.Domain.Enums.MFAMethod.Email)
    {
        return new EnableMfaCommand(
            Guid.NewGuid(),
            method);
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

        _userAccountRepository.Verify(
            x => x.GetByIdAsync(
                command.UserId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies failure is returned when
    /// email MFA is requested but email
    /// has not been verified.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_EmailNotVerified_When_Email_Is_Not_Verified()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand(
                MFAMethod.Email);

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
            .Be(IdentityErrors.EmailNotVerified);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies failure is returned when
    /// SMS MFA is requested but phone
    /// has not been verified.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_PhoneNotVerified_When_Phone_Is_Not_Verified()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand(
                MFAMethod.SMS);

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
            .Be(IdentityErrors.PhoneNotVerified);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies failure is returned when
    /// TOTP MFA is requested but no
    /// TOTP secret has been configured.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_TotpRequired_When_TotpSecret_Is_Missing()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand(
                MFAMethod.TOTP);

        var user =
            UserAccountFixture.Create();

        user.VerifyEmail(now);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

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
            .Be(IdentityErrors.TotpRequired);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies failure is returned when
    /// MFA has already been enabled.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_InvalidState_When_Mfa_Is_Already_Enabled()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand(
                MFAMethod.Email);

        var user =
            UserAccountFixture.CreateEmailMfaUser();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

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
            .Be(IdentityErrors.InvalidState);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    // ============================================================
    // ExecuteAsync
    // Success
    // ============================================================

    /// <summary>
    /// Verifies email MFA is enabled successfully.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Enable_Email_Mfa_Successfully()
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
            CreateCommand(
                MFAMethod.Email);

        var user =
            UserAccountFixture.CreateEmailVerified();

        var previousSecurityStamp =
            user.SecurityStamp;

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        user.MFAEnabled
            .Should()
            .BeTrue();

        user.MFAMethod
            .Should()
            .Be(MFAMethod.Email);

        user.SecurityStamp
            .Should()
            .NotBe(previousSecurityStamp);

        user.UpdatedAt
            .Should()
            .Be(now);

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }

    /// <summary>
    /// Verifies SMS MFA is enabled successfully.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Enable_Sms_Mfa_Successfully()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand(
                MFAMethod.SMS);

        var user =
            UserAccountFixture.CreatePhoneVerified();

        var previousSecurityStamp =
            user.SecurityStamp;

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        user.MFAEnabled
            .Should()
            .BeTrue();

        user.MFAMethod
            .Should()
            .Be(MFAMethod.SMS);

        user.SecurityStamp
            .Should()
            .NotBe(previousSecurityStamp);

        user.UpdatedAt
            .Should()
            .Be(now);

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }

    /// <summary>
    /// Verifies WhatsApp MFA is enabled successfully.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Enable_WhatsApp_Mfa_Successfully()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand(
                MFAMethod.WhatsApp);

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

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        user.MFAEnabled
            .Should()
            .BeTrue();

        user.MFAMethod
            .Should()
            .Be(MFAMethod.WhatsApp);

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }

    /// <summary>
    /// Verifies repository is queried once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Load_User_Once()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand(
                MFAMethod.Email);

        var user =
            UserAccountFixture.CreateEmailVerified();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

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
    /// Verifies cancellation token is forwarded.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Forward_CancellationToken()
    {
        // Arrange

        var token =
            new CancellationTokenSource().Token;

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand(
                MFAMethod.Email);

        var user =
            UserAccountFixture.CreateEmailVerified();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    token))
            .ReturnsAsync(user);

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

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }
}