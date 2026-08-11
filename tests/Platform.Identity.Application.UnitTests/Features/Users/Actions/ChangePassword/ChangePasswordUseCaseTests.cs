using FluentAssertions;
using FluentAssertions.Specialized;
using Microsoft.Extensions.Logging;
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
/// Unit tests for <see cref="ChangePasswordUseCase"/>.
/// </summary>
public sealed class ChangePasswordUseCaseTests
{
    private readonly Mock<IUserAccountRepository>
        _userAccountRepository = new();

    private readonly Mock<IPasswordHasher>
        _passwordHasher = new();

    private readonly Mock<IClock>
        _clock = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private ChangePasswordUseCase CreateSut(
        IUserAccountRepository? userAccountRepository = null,
        IPasswordHasher? passwordHasher = null,
        IClock? clock = null)
    {
        return new ChangePasswordUseCase(
            userAccountRepository
                ?? _userAccountRepository.Object,
            passwordHasher
                ?? _passwordHasher.Object,
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
            () => new ChangePasswordUseCase(
                null!,
                _passwordHasher.Object,
                _clock.Object);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("userAccountRepository");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// passwordHasher is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_PasswordHasher_Is_Null()
    {
        // Act

        Action act =
            () => new ChangePasswordUseCase(
                _userAccountRepository.Object,
                null!,
                _clock.Object);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("passwordHasher");
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
            () => new ChangePasswordUseCase(
                _userAccountRepository.Object,
                _passwordHasher.Object,
                null!);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("clock");
    }

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
            .WithParameterName("command");
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
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "OldPassword123!",
                "NewPassword123!");

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

        _passwordHasher.Verify(
            x => x.Verify(
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies failure is returned when
    /// current password is invalid.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_InvalidPassword_When_CurrentPassword_Is_Invalid()
    {
        // Arrange

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "WrongPassword",
                "NewPassword123!");

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(false);

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
            .Be(IdentityErrors.InvalidPassword);

        _userAccountRepository.Verify(
            x => x.GetByIdAsync(
                command.UserId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _passwordHasher.Verify(
            x => x.Verify(
                command.CurrentPassword,
                user.PasswordHash),
            Times.Once);

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies password hashing is not performed
    /// when current password verification fails.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Not_Hash_NewPassword_When_CurrentPassword_Is_Invalid()
    {
        // Arrange

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "WrongPassword",
                "NewPassword123!");

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(false);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);
    }

    // ============================================================
    // ExecuteAsync
    // Success
    // ============================================================

    /// <summary>
    /// Verifies password is changed successfully.
    /// </summary>
    /// <summary>
    /// Verifies password is changed successfully.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_Password_Is_Changed()
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
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "OldPassword123!",
                "NewPassword123!");

        var user =
            UserAccountFixture.Create();

        var previousSecurityStamp =
            user.SecurityStamp;

        var previousPasswordVersion =
            user.PasswordVersion;

        var previousPasswordHash =
            user.PasswordHash;

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns("NEW_PASSWORD_HASH");

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        user.PasswordHash
            .Should()
            .Be("NEW_PASSWORD_HASH");

        user.PasswordVersion
            .Should()
            .Be(previousPasswordVersion + 1);

        user.SecurityStamp
            .Should()
            .NotBe(previousSecurityStamp);

        user.LastPasswordChangedAt
            .Should()
            .Be(now);

        _passwordHasher.Verify(
            x => x.Verify(
                command.CurrentPassword,
                previousPasswordHash),
            Times.Once);

        _passwordHasher.Verify(
            x => x.Hash(
                command.NewPassword),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.Update(
                user),
            Times.Once);
    }

    /// <summary>
    /// Verifies password hash is generated
    /// using the supplied new password.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Hash_NewPassword_Once()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "OldPassword123!",
                "NewPassword123!");

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

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns("HASH_001");

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _passwordHasher.Verify(
            x => x.Hash(
                command.NewPassword),
            Times.Once);
    }

    /// <summary>
    /// Verifies repository update is called
    /// after password changes.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Update_UserAccount_When_Password_Is_Changed()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "OldPassword123!",
                "NewPassword123!");

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

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns("HASH_001");

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

    // ============================================================
    // ExecuteAsync
    // DomainException Mapping
    // ============================================================

    /// <summary>
    /// Verifies PasswordReuse domain exception
    /// is translated into application error.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_PasswordReuse_When_NewPasswordHash_Equals_CurrentHash()
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
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword",
                "NewPassword");

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

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        // Return current hash so domain throws PasswordReuse.
        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns(user.PasswordHash);

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
            .Be(IdentityErrors.PasswordReuse);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies InvalidState domain exception
    /// is translated into application error
    /// when user is disabled.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_InvalidState_When_User_Is_Disabled()
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
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword",
                "NewPassword");

        var user =
            UserAccountFixture.Create();

        user.Disable(now);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns("NEW_PASSWORD_HASH");

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
    /// Verifies repository update is not called
    /// when domain validation fails.
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
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword",
                "NewPassword");

        var user =
            UserAccountFixture.Create();

        user.Disable(now);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns("NEW_PASSWORD_HASH");

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

    // ============================================================
    // ExecuteAsync
    // Interaction Verification
    // ============================================================

    /// <summary>
    /// Verifies password verification is executed
    /// before password hashing.
    /// </summary>
    /// <summary>
    /// Verifies password verification is executed
    /// before password hashing.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Verify_CurrentPassword_Once()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword",
                "NewPassword");

        var user =
            UserAccountFixture.Create();

        var previousPasswordHash =
            user.PasswordHash;

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns("NEW_HASH");

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _passwordHasher.Verify(
            x => x.Verify(
                command.CurrentPassword,
                previousPasswordHash),
            Times.Once);
    }

    /// <summary>
    /// Verifies repository is queried exactly once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Load_User_Once()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword",
                "NewPassword");

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

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(command.NewPassword))
            .Returns("NEW_HASH");

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
    /// Verifies password hashing is never executed
    /// when user cannot be found.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Not_Hash_When_User_NotFound()
    {
        // Arrange

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword",
                "NewPassword");

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserAccount?)null);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies repository update occurs only once
    /// during successful password change.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Update_User_Only_Once()
    {
        // Arrange

        var now =
            DateTime.UtcNow;

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword",
                "NewPassword");

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

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns("HASH_001");

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
    /// Verifies supplied cancellation token
    /// is forwarded to repository.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Forward_CancellationToken()
    {
        // Arrange

        var token =
            new CancellationTokenSource().Token;

        var command =
            new ChangePasswordCommand(
                Guid.NewGuid(),
                "CurrentPassword",
                "NewPassword");

        var user =
            UserAccountFixture.Create();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    token))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x =>
                x.Verify(
                    command.CurrentPassword,
                    user.PasswordHash))
            .Returns(true);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.NewPassword))
            .Returns("HASH");

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
}