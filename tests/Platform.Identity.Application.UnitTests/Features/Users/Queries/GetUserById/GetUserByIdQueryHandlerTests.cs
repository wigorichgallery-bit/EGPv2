using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Identity.Application.Errors;
using Platform.Identity.Application.Features.Users.Queries;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Queries;

/// <summary>
/// Unit tests for <see cref="GetUserByIdQueryHandler"/>.
/// </summary>
public sealed partial class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserQueryRepository>
        _userQueryRepository = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private GetUserByIdQueryHandler CreateSut(
        IUserQueryRepository? repository = null)
    {
        return new GetUserByIdQueryHandler(
            repository
            ?? _userQueryRepository.Object);
    }

    /// <summary>
    /// Creates a valid query.
    /// </summary>
    private static GetUserByIdQuery CreateQuery()
    {
        return new GetUserByIdQuery(
            Guid.NewGuid());
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
    public void Constructor_Should_ThrowArgumentNullException_When_UserQueryRepository_Is_Null()
    {
        // Act

        Action act =
            () => new GetUserByIdQueryHandler(
                null!);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "userQueryRepository");
    }

    // ============================================================
    // ExecuteAsync
    // ============================================================

    [Fact]
    public async Task ExecuteAsync_Should_ThrowArgumentNullException_When_Query_Is_Null()
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
                "query");
    }

    // ============================================================
    // ExecuteAsync
    // ============================================================

    [Fact]
    public async Task ExecuteAsync_Should_Return_UserNotFound_When_User_Does_Not_Exist()
    {
        // Arrange

        var query =
            CreateQuery();

        _userQueryRepository
            .Setup(x =>
                x.FindByIdAsync(
                    query.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDto?)null);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(query);

        // Assert

        result.IsSuccess
            .Should()
            .BeFalse();

        result.Error
            .Should()
            .Be(IdentityErrors.UserNotFound);

        _userQueryRepository.Verify(
            x => x.FindByIdAsync(
                query.UserId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_User_Exists()
    {
        // Arrange

        var query =
            CreateQuery();

        var user =
            new UserDto(
                query.UserId,
                "john",
                "john@example.com",
                "+628123456789",
                true,
                true,
                UserStatus.Active,
                false,
                MFAMethod.None);

        _userQueryRepository
            .Setup(x =>
                x.FindByIdAsync(
                    query.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(query);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        result.Value
            .Should()
            .BeSameAs(user);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Call_FindByIdAsync_Once()
    {
        // Arrange

        var query =
            CreateQuery();

        var user =
            new UserDto(
                query.UserId,
                "john",
                "john@example.com",
                "+628123456789",
                true,
                true,
                UserStatus.Active,
                false,
                MFAMethod.None);

        _userQueryRepository
            .Setup(x =>
                x.FindByIdAsync(
                    query.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(query);

        // Assert

        _userQueryRepository.Verify(
            x => x.FindByIdAsync(
                query.UserId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Forward_CancellationToken()
    {
        // Arrange

        var token =
            new CancellationTokenSource().Token;

        var query =
            CreateQuery();

        var user =
            new UserDto(
                query.UserId,
                "john",
                "john@example.com",
                "+628123456789",
                true,
                true,
                UserStatus.Active,
                false,
                MFAMethod.None);

        _userQueryRepository
            .Setup(x =>
                x.FindByIdAsync(
                    query.UserId,
                    token))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(
            query,
            token);

        // Assert

        _userQueryRepository.Verify(
            x => x.FindByIdAsync(
                query.UserId,
                token),
            Times.Once);
    }
}

