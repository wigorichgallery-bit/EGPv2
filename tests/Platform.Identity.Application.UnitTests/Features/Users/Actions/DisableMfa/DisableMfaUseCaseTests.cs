using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Errors;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.Aggregates;
using Platform.SharedKernel.Abstractions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="DisableMfaUseCase"/>.
/// </summary>
public sealed class DisableMfaUseCaseTests
{
    private readonly Mock<IUserAccountRepository>
        _userAccountRepository = new();

    private readonly Mock<IClock>
        _clock = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private DisableMfaUseCase CreateSut(
        IUserAccountRepository? userAccountRepository = null,
        IClock? clock = null)
    {
        return new DisableMfaUseCase(
            userAccountRepository
                ?? _userAccountRepository.Object,
            clock
                ?? _clock.Object);
    }

    // ============================================================
    // Constructor
    // ============================================================

    [Fact]
    public void Constructor_Should_Create_Instance()
    {
        var sut =
            CreateSut();

        sut.Should()
            .NotBeNull();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_UserAccountRepository_Is_Null()
    {
        Action act =
            () => new DisableMfaUseCase(
                null!,
                _clock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("userAccountRepository");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_Clock_Is_Null()
    {
        Action act =
            () => new DisableMfaUseCase(
                _userAccountRepository.Object,
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("clock");
    }

    [Fact]
    public async Task ExecuteAsync_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        var sut =
            CreateSut();

        Func<Task> act =
            () => sut.ExecuteAsync(
                null!,
                CancellationToken.None);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("command");
    }

    /// <summary>
    /// Creates a valid command.
    /// </summary>
    private static DisableMfaCommand CreateCommand()
    {
        return new DisableMfaCommand(
            Guid.NewGuid());
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
    /// MFA has not been enabled.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_InvalidState_When_Mfa_Is_Not_Enabled()
    {
        // Arrange

        var now =
            new DateTime(
                2026,
                1,
                1,
                10,
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

    /// <summary>
    /// Verifies repository update is not
    /// performed when a domain exception occurs.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Not_Update_User_When_DomainException_Is_Thrown()
    {
        // Arrange

        var now =
            new DateTime(
                2026,
                1,
                1,
                10,
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
    /// is forwarded to the repository.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Forward_CancellationToken()
    {
        // Arrange

        var token =
            new CancellationTokenSource().Token;

        var command =
            CreateCommand();

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    token))
            .ReturnsAsync((UserAccount?)null);

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
    }

    // ============================================================
    // ExecuteAsync
    // Success
    // ============================================================

    /// <summary>
    /// Verifies MFA is disabled successfully.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_Mfa_Is_Disabled()
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
            UserAccountFixture.CreateEmailMfaUser();

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
            .BeFalse();

        user.MFAMethod
            .Should()
            .Be(
                Platform.Identity.Domain.Enums
                    .MFAMethod.None);

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
    /// Verifies repository update is executed
    /// exactly once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Update_UserAccount_Once()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand();

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

        await sut.ExecuteAsync(command);

        // Assert

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }

    /// <summary>
    /// Verifies repository is queried
    /// exactly once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Load_User_Once()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand();

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

        await sut.ExecuteAsync(command);

        // Assert

        _userAccountRepository.Verify(
            x => x.GetByIdAsync(
                command.UserId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies MFA method is reset
    /// after successful disablement.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Reset_MfaMethod_To_None()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            CreateCommand();

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

        await sut.ExecuteAsync(command);

        // Assert

        user.MFAMethod
            .Should()
            .Be(
                Platform.Identity.Domain.Enums
                    .MFAMethod.None);
    }
}