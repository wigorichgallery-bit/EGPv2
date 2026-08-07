using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Identity.Application.Features.Users.Queries;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Queries;

/// <summary>
/// Unit tests for <see cref="GetUsersQueryHandler"/>.
/// </summary>
public sealed partial class GetUsersQueryHandlerTests
{
    private readonly Mock<IUserQueryRepository>
        _userQueryRepository = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private GetUsersQueryHandler CreateSut(
        IUserQueryRepository? repository = null)
    {
        return new GetUsersQueryHandler(
            repository
            ?? _userQueryRepository.Object);
    }

    /// <summary>
    /// Creates a valid query.
    /// </summary>
    private static GetUsersQuery CreateQuery()
    {
        return new GetUsersQuery();
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
            () => new GetUsersQueryHandler(
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
    public async Task ExecuteAsync_Should_Return_Success_When_Users_Exist()
    {
        // Arrange

        var query =
            CreateQuery();

        IReadOnlyList<UserDto> users =
        [
            new UserDto(
            Guid.NewGuid(),
            "john",
            "john@example.com",
            "+628123456789",
            true,
            true,
            UserStatus.Active,
            false,
            MFAMethod.None),

        new UserDto(
            Guid.NewGuid(),
            "jane",
            "jane@example.com",
            "+628987654321",
            false,
            false,
            UserStatus.Active,
            true,
            MFAMethod.Email)
        ];

        _userQueryRepository
            .Setup(x =>
                x.ListAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

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
            .BeSameAs(users);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_Empty_Collection_When_No_Users_Exist()
    {
        // Arrange

        var query =
            CreateQuery();

        IReadOnlyList<UserDto> users =
            [];

        _userQueryRepository
            .Setup(x =>
                x.ListAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

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
            .BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_Should_Call_ListAsync_Once()
    {
        // Arrange

        var query =
            CreateQuery();

        IReadOnlyList<UserDto> users =
            [];

        _userQueryRepository
            .Setup(x =>
                x.ListAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(query);

        // Assert

        _userQueryRepository.Verify(
            x => x.ListAsync(
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

        IReadOnlyList<UserDto> users =
            [];

        _userQueryRepository
            .Setup(x =>
                x.ListAsync(
                    token))
            .ReturnsAsync(users);

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(
            query,
            token);

        // Assert

        _userQueryRepository.Verify(
            x => x.ListAsync(
                token),
            Times.Once);
    }

}