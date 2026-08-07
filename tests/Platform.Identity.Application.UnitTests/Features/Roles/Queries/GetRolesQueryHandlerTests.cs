using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Identity.Application.Features.Roles.Queries;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Roles.Queries;

/// <summary>
/// Unit tests for <see cref="GetRolesQueryHandler"/>.
/// </summary>
public sealed class GetRolesQueryHandlerTests
{
    private readonly Mock<IRoleQueryRepository>
        _roleQueryRepository = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private GetRolesQueryHandler CreateSut(
        IRoleQueryRepository? roleQueryRepository = null)
    {
        return new GetRolesQueryHandler(
            roleQueryRepository
                ?? _roleQueryRepository.Object);
    }

    /// <summary>
    /// Verifies constructor throws when
    /// role query repository is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_RoleQueryRepository_Is_Null()
    {
        FluentActions
            .Invoking(() =>
                new GetRolesQueryHandler(
                    null!))
            .Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "roleQueryRepository");
    }

    /// <summary>
    /// Verifies ExecuteAsync throws when
    /// query is null.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_ThrowArgumentNullException_When_Query_Is_Null()
    {
        // Arrange

        var sut =
            CreateSut();

        // Act

        Func<Task> act =
            () => sut.ExecuteAsync(
                null!);

        // Assert

        await act
            .Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(
                "query");
    }

    /// <summary>
    /// Verifies ExecuteAsync returns
    /// roles from repository.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Return_Roles()
    {
        // Arrange

        var query =
            new GetRolesQuery();

        IReadOnlyList<RoleDto> roles =
        [
            new RoleDto(
                Guid.NewGuid(),
                "Administrator",
                true,
                "GLOBAL",
                true,
                new[]
                {
                    "USER.READ",
                    "USER.WRITE"
                }),

            new RoleDto(
                Guid.NewGuid(),
                "Operator",
                false,
                "TENANT",
                true,
                new[]
                {
                    "USER.READ"
                })
        ];

        _roleQueryRepository
            .Setup(x =>
                x.ListAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                roles);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ExecuteAsync(
                query);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        result.Value
            .Should()
            .BeSameAs(
                roles);

        _roleQueryRepository.Verify(
            x => x.ListAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies cancellation token is
    /// forwarded to repository.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_Should_Pass_CancellationToken()
    {
        // Arrange

        var query =
            new GetRolesQuery();

        var cancellationToken =
            new CancellationTokenSource()
                .Token;

        _roleQueryRepository
            .Setup(x =>
                x.ListAsync(
                    cancellationToken))
            .ReturnsAsync(
                Array.Empty<RoleDto>());

        var sut =
            CreateSut();

        // Act

        await sut.ExecuteAsync(
            query,
            cancellationToken);

        // Assert

        _roleQueryRepository.Verify(
            x => x.ListAsync(
                cancellationToken),
            Times.Once);
    }
}