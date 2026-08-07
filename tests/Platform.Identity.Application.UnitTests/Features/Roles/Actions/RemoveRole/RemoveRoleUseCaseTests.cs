using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Errors;
using Platform.Identity.Application.Features.Roles.Actions;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.Aggregates;
using Platform.SharedKernel.Abstractions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Roles.Actions;

/// <summary>
/// Unit tests for <see cref="RemoveRoleUseCase"/>.
/// </summary>
public sealed partial class RemoveRoleUseCaseTests
{
    private readonly Mock<IUserAccountRepository>
        _userAccountRepository = new();

    private readonly Mock<IRoleRepository>
        _roleRepository = new();

    private readonly Mock<IClock>
        _clock = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private RemoveRoleUseCase CreateSut(
        IUserAccountRepository? userAccountRepository = null,
        IRoleRepository? roleRepository = null,
        IClock? clock = null)
    {
        return new RemoveRoleUseCase(
            userAccountRepository
                ?? _userAccountRepository.Object,
            roleRepository
                ?? _roleRepository.Object,
            clock
                ?? _clock.Object);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_UserAccountRepository_Is_Null()
    {
        FluentActions
            .Invoking(() =>
                new RemoveRoleUseCase(
                    null!,
                    _roleRepository.Object,
                    _clock.Object))
            .Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("userAccountRepository");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_RoleRepository_Is_Null()
    {
        FluentActions
            .Invoking(() =>
                new RemoveRoleUseCase(
                    _userAccountRepository.Object,
                    null!,
                    _clock.Object))
            .Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("roleRepository");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_Clock_Is_Null()
    {
        FluentActions
            .Invoking(() =>
                new RemoveRoleUseCase(
                    _userAccountRepository.Object,
                    _roleRepository.Object,
                    null!))
            .Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("clock");
    }

    [Fact]
    public async Task ExecuteAsync_Should_ThrowArgumentNullException_When_Command_Is_Null()
    {
        var sut =
            CreateSut();

        Func<Task> act =
            () => sut.ExecuteAsync(null!);

        await act
            .Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("command");
    }

    /// <summary>
    /// Verifies ExecuteAsync returns UserNotFound
    /// when the specified user does not exist.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_UserNotFound_When_User_Does_Not_Exist()
    {
        // Arrange

        var command =
            new RemoveRoleCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

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
            .BeSameAs(
                IdentityErrors.UserNotFound);

        _userAccountRepository.Verify(
            x => x.GetByIdAsync(
                command.UserId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _roleRepository.Verify(
            x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies ExecuteAsync returns RoleNotFound
    /// when the specified role does not exist.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_RoleNotFound_When_Role_Does_Not_Exist()
    {
        // Arrange

        var command =
            new RemoveRoleCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _roleRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.RoleId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role?)null);

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
            .BeSameAs(
                IdentityErrors.RoleNotFound);

        _userAccountRepository.Verify(
            x => x.GetByIdAsync(
                command.UserId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _roleRepository.Verify(
            x => x.GetByIdAsync(
                command.RoleId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.Update(
                It.IsAny<UserAccount>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies role assignment is removed successfully.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_Role_Is_Removed()
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

        var roleId =
            Guid.NewGuid();

        var command =
            new RemoveRoleCommand(
                Guid.NewGuid(),
                roleId);

        var user =
            UserAccountFixture.Create();

        user.AssignRole(
            roleId,
            now);

        var role =
            RoleFixture.Create(
                roleId: roleId);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        _userAccountRepository
            .Setup(x =>
                x.GetByIdAsync(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _roleRepository
            .Setup(x =>
                x.GetByIdAsync(
                    roleId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(command);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        user.RoleAssignments
            .Should()
            .NotContain(
                assignment =>
                    assignment.RoleId == roleId);

        _userAccountRepository.Verify(
            x => x.Update(user),
            Times.Once);
    }
}