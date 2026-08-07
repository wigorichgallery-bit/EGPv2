using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Errors;
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Exceptions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Actions;

/// <summary>
/// Unit tests for <see cref="CreateUserUseCase"/>.
/// </summary>
public sealed class CreateUserUseCaseTests
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
    private CreateUserUseCase CreateSut(
        IUserAccountRepository? userAccountRepository = null,
        IPasswordHasher? passwordHasher = null,
        IClock? clock = null)
    {
        return new CreateUserUseCase(
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
            () => new CreateUserUseCase(
                null!,
                _passwordHasher.Object,
                _clock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("userAccountRepository");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_PasswordHasher_Is_Null()
    {
        Action act =
            () => new CreateUserUseCase(
                _userAccountRepository.Object,
                null!,
                _clock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("passwordHasher");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_Clock_Is_Null()
    {
        Action act =
            () => new CreateUserUseCase(
                _userAccountRepository.Object,
                _passwordHasher.Object,
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
    private static CreateUserCommand CreateCommand()
    {
        return new CreateUserCommand(
            "john.doe",
            "john.doe@example.com",
            "+6281234567890",
            "Password123!");
    }

    // ============================================================
    // ExecuteAsync
    // Failure
    // ============================================================

    /// <summary>
    /// Verifies failure is returned when
    /// username already exists.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_UsernameAlreadyExists_When_Username_Exists()
    {
        // Arrange

        var command =
            CreateCommand();

        _userAccountRepository
            .Setup(x =>
                x.ExistsByUsernameAsync(
                    command.Username,
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
            .BeFalse();

        result.Error
            .Should()
            .Be(IdentityErrors.UsernameAlreadyExists);

        _userAccountRepository.Verify(
            x => x.ExistsByUsernameAsync(
                command.Username,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.ExistsByEmailAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.ExistsByPhoneAsync(
                It.IsAny<PhoneNumber>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserAccount>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies failure is returned when
    /// email already exists.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_EmailAlreadyExists_When_Email_Exists()
    {
        // Arrange

        var command =
            CreateCommand();

        _userAccountRepository
            .Setup(x =>
                x.ExistsByUsernameAsync(
                    command.Username,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByEmailAsync(
                    It.IsAny<EmailAddress>(),
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
            .BeFalse();

        result.Error
            .Should()
            .Be(IdentityErrors.EmailAlreadyExists);

        _userAccountRepository.Verify(
            x => x.ExistsByUsernameAsync(
                command.Username,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.ExistsByEmailAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.ExistsByPhoneAsync(
                It.IsAny<PhoneNumber>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserAccount>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies failure is returned when
    /// phone number already exists.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_PhoneAlreadyExists_When_Phone_Exists()
    {
        // Arrange

        var command =
            CreateCommand();

        _userAccountRepository
            .Setup(x =>
                x.ExistsByUsernameAsync(
                    command.Username,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByEmailAsync(
                    It.IsAny<EmailAddress>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByPhoneAsync(
                    It.IsAny<PhoneNumber>(),
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
            .BeFalse();

        result.Error
            .Should()
            .Be(IdentityErrors.PhoneAlreadyExists);

        _userAccountRepository.Verify(
            x => x.ExistsByUsernameAsync(
                command.Username,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.ExistsByEmailAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.ExistsByPhoneAsync(
                It.IsAny<PhoneNumber>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserAccount>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // ============================================================
    // ExecuteAsync
    // Success
    // ============================================================

    /// <summary>
    /// Verifies successful user creation returns
    /// the created user identifier.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_User_Is_Created()
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

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByUsernameAsync(
                    command.Username,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByEmailAsync(
                    It.IsAny<EmailAddress>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByPhoneAsync(
                    It.IsAny<PhoneNumber>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.Password))
            .Returns("HASHED_PASSWORD");

        UserAccount? persistedUser =
            null;

        _userAccountRepository
            .Setup(x =>
                x.AddAsync(
                    It.IsAny<UserAccount>(),
                    It.IsAny<CancellationToken>()))
            .Callback<UserAccount, CancellationToken>(
                (user, _) => persistedUser = user)
            .Returns(Task.CompletedTask);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        persistedUser
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Be(persistedUser!.Id);

        persistedUser.Username
            .Should()
            .Be(command.Username);

        persistedUser.Email.Value
            .Should()
            .Be(command.Email);

        persistedUser.PhoneNumber.Value
            .Should()
            .Be(command.PhoneNumber);

        persistedUser.PasswordHash
            .Should()
            .Be("HASHED_PASSWORD");

        persistedUser.CreatedAt
            .Should()
            .Be(now);
    }

    /// <summary>
    /// Verifies password is hashed once.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Hash_Password_Once()
    {
        // Arrange

        var command =
            CreateCommand();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByUsernameAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByEmailAsync(
                    It.IsAny<EmailAddress>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByPhoneAsync(
                    It.IsAny<PhoneNumber>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasher
            .Setup(x =>
                x.Hash(command.Password))
            .Returns("HASH");

        _userAccountRepository
            .Setup(x =>
                x.AddAsync(
                    It.IsAny<UserAccount>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _passwordHasher.Verify(
            x => x.Hash(
                command.Password),
            Times.Once);
    }

    /// <summary>
    /// Verifies created user is persisted.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Add_UserAccount_Once()
    {
        // Arrange

        var command =
            CreateCommand();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByUsernameAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByEmailAsync(
                    It.IsAny<EmailAddress>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByPhoneAsync(
                    It.IsAny<PhoneNumber>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasher
            .Setup(x =>
                x.Hash(
                    command.Password))
            .Returns("HASH");

        _userAccountRepository
            .Setup(x =>
                x.AddAsync(
                    It.IsAny<UserAccount>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(command);

        // Assert

        _userAccountRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserAccount>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================================================
    // ExecuteAsync
    // DomainException Mapping
    // ============================================================

    /// <summary>
    /// Verifies domain exception raised while creating
    /// EmailAddress is translated into an application error.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Failure_When_EmailAddress_Is_Invalid()
    {
        // Arrange

        var command =
            new CreateUserCommand(
                "john.doe",
                "invalid-email",
                "+6281234567890",
                "Password123!");

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
                IdentityErrorMapper.Map(
                    new DomainException(
                        "IDENTITY.INVALID_EMAIL",
                        string.Empty)));

        _userAccountRepository.Verify(
            x => x.ExistsByUsernameAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies domain exception raised while creating
    /// PhoneNumber is translated into an application error.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Failure_When_PhoneNumber_Is_Invalid()
    {
        // Arrange

        var command =
            new CreateUserCommand(
                "john.doe",
                "john.doe@example.com",
                "INVALID_PHONE",
                "Password123!");

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
                IdentityErrorMapper.Map(
                    new DomainException(
                        "IDENTITY.INVALID_PHONE_NUMBER",
                        string.Empty)));

        _userAccountRepository.Verify(
            x => x.ExistsByUsernameAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _passwordHasher.Verify(
            x => x.Hash(
                It.IsAny<string>()),
            Times.Never);
    }

    // ============================================================
    // Interaction Verification
    // ============================================================

    /// <summary>
    /// Verifies uniqueness checks are executed
    /// in the expected order.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Check_All_Uniqueness_Before_Creating_User()
    {
        // Arrange

        var command =
            CreateCommand();

        var sequence =
            new MockSequence();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _userAccountRepository
            .InSequence(sequence)
            .Setup(x =>
                x.ExistsByUsernameAsync(
                    command.Username,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .InSequence(sequence)
            .Setup(x =>
                x.ExistsByEmailAsync(
                    It.IsAny<EmailAddress>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userAccountRepository
            .InSequence(sequence)
            .Setup(x =>
                x.ExistsByPhoneAsync(
                    It.IsAny<PhoneNumber>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasher
            .Setup(x =>
                x.Hash(command.Password))
            .Returns("HASH");

        _userAccountRepository
            .Setup(x =>
                x.AddAsync(
                    It.IsAny<UserAccount>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies supplied cancellation token
    /// is forwarded to repository methods.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Forward_CancellationToken()
    {
        // Arrange

        var token =
            new CancellationTokenSource().Token;

        var command =
            CreateCommand();

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByUsernameAsync(
                    command.Username,
                    token))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByEmailAsync(
                    It.IsAny<EmailAddress>(),
                    token))
            .ReturnsAsync(false);

        _userAccountRepository
            .Setup(x =>
                x.ExistsByPhoneAsync(
                    It.IsAny<PhoneNumber>(),
                    token))
            .ReturnsAsync(false);

        _passwordHasher
            .Setup(x =>
                x.Hash(command.Password))
            .Returns("HASH");

        _userAccountRepository
            .Setup(x =>
                x.AddAsync(
                    It.IsAny<UserAccount>(),
                    token))
            .Returns(Task.CompletedTask);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(
            command,
            token);

        // Assert

        _userAccountRepository.Verify(
            x => x.ExistsByUsernameAsync(
                command.Username,
                token),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.ExistsByEmailAsync(
                It.IsAny<EmailAddress>(),
                token),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.ExistsByPhoneAsync(
                It.IsAny<PhoneNumber>(),
                token),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserAccount>(),
                token),
            Times.Once);
    }

    
}