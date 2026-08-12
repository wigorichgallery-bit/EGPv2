// ===========================================
// File Location:
// tests/Platform.Identity.Application.UnitTests/
// Features/Authentication/Services/
// AuthenticationIdentityResolverTests.cs
// ===========================================

using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Application.Features.Authentication.Services;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.Identity.Application.UnitTests.Fixtures;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Services;

/// <summary>
/// Unit tests for
/// <see cref="AuthenticationIdentityResolver"/>.
/// </summary>
public sealed class AuthenticationIdentityResolverTests
{
    private readonly Mock<IUserAccountRepository>
        _userAccountRepository = new();

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private AuthenticationIdentityResolver CreateSut(
        IUserAccountRepository? userAccountRepository = null)
    {
        return new AuthenticationIdentityResolver(
            userAccountRepository
                ?? _userAccountRepository.Object);
    }

    // ============================================================
    // Constructor
    // ============================================================

    /// <summary>
    /// Verifies the resolver can be created.
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
            () => new AuthenticationIdentityResolver(
                null!);

        // Assert

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "userAccountRepository");
    }

    // ============================================================
    // ResolveAsync
    // Username
    // ============================================================

    /// <summary>
    /// Verifies username identities are resolved
    /// through the username repository operation.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_Resolve_By_Username()
    {
        // Arrange

        const string username =
            "john.doe";

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByUsernameAsync(
                    username,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ResolveAsync(
                username);

        // Assert

        result
            .Should()
            .BeSameAs(user);

        _userAccountRepository.Verify(
            x => x.GetByUsernameAsync(
                username,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.GetByEmailAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies username identities are trimmed
    /// before repository resolution.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_Trim_Username_Before_Resolution()
    {
        // Arrange

        const string suppliedIdentity =
            "  john.doe  ";

        const string normalizedUsername =
            "john.doe";

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByUsernameAsync(
                    normalizedUsername,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ResolveAsync(
                suppliedIdentity);

        // Assert

        result
            .Should()
            .BeSameAs(user);

        _userAccountRepository.Verify(
            x => x.GetByUsernameAsync(
                normalizedUsername,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies null username lookup returns null
    /// when the repository does not find a user.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_Return_Null_When_Username_Is_Not_Found()
    {
        // Arrange

        const string username =
            "unknown.user";

        _userAccountRepository
            .Setup(x =>
                x.GetByUsernameAsync(
                    username,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserAccount?)null);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ResolveAsync(
                username);

        // Assert

        result
            .Should()
            .BeNull();

        _userAccountRepository.Verify(
            x => x.GetByUsernameAsync(
                username,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================================================
    // ResolveAsync
    // Email
    // ============================================================

    /// <summary>
    /// Verifies email identities are resolved
    /// through the email repository operation.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_Resolve_By_Email()
    {
        // Arrange

        const string emailValue =
            "john.doe@example.com";

        var email =
            new EmailAddress(
                emailValue);

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByEmailAsync(
                    It.Is<EmailAddress>(
                        value =>
                            value.Value ==
                            email.Value),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ResolveAsync(
                emailValue);

        // Assert

        result
            .Should()
            .BeSameAs(user);

        _userAccountRepository.Verify(
            x => x.GetByEmailAsync(
                It.Is<EmailAddress>(
                    value =>
                        value.Value ==
                        email.Value),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userAccountRepository.Verify(
            x => x.GetByUsernameAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies email identities are normalized
    /// before repository resolution.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_Normalize_Email_Before_Resolution()
    {
        // Arrange

        const string suppliedIdentity =
            "  JOHN.DOE@EXAMPLE.COM  ";

        const string normalizedEmail =
            "john.doe@example.com";

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByEmailAsync(
                    It.Is<EmailAddress>(
                        value =>
                            value.Value ==
                            normalizedEmail),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ResolveAsync(
                suppliedIdentity);

        // Assert

        result
            .Should()
            .BeSameAs(user);

        _userAccountRepository.Verify(
            x => x.GetByEmailAsync(
                It.Is<EmailAddress>(
                    value =>
                        value.Value ==
                        normalizedEmail),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies unresolved email identities return null.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_Return_Null_When_Email_Is_Not_Found()
    {
        // Arrange

        const string emailValue =
            "unknown@example.com";

        _userAccountRepository
            .Setup(x =>
                x.GetByEmailAsync(
                    It.Is<EmailAddress>(
                        value =>
                            value.Value ==
                            emailValue),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserAccount?)null);

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ResolveAsync(
                emailValue);

        // Assert

        result
            .Should()
            .BeNull();

        _userAccountRepository.Verify(
            x => x.GetByEmailAsync(
                It.Is<EmailAddress>(
                    value =>
                        value.Value ==
                        emailValue),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies malformed email identities are treated
    /// as unresolved identities.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_Return_Null_When_Email_Is_Invalid()
    {
        // Arrange

        const string invalidEmail =
            "invalid-email@";

        var sut =
            CreateSut();

        // Act

        var result =
            await sut.ResolveAsync(
                invalidEmail);

        // Assert

        result
            .Should()
            .BeNull();

        _userAccountRepository.Verify(
            x => x.GetByEmailAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.GetByUsernameAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // ============================================================
    // ResolveAsync
    // Guard Clauses
    // ============================================================

    /// <summary>
    /// Verifies null identity is rejected.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_ThrowArgumentNullException_When_Identity_Is_Null()
    {
        // Arrange

        var sut =
            CreateSut();

        // Act

        Func<Task> act =
            () => sut.ResolveAsync(
                null!);

        // Assert

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(
                "identity");
    }

    /// <summary>
    /// Verifies whitespace identity is rejected.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_ThrowArgumentException_When_Identity_Is_Whitespace()
    {
        // Arrange

        var sut =
            CreateSut();

        // Act

        Func<Task> act =
            () => sut.ResolveAsync(
                "   ");

        // Assert

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithParameterName(
                "identity");
    }

    /// <summary>
    /// Verifies the cancellation token is forwarded
    /// to username repository resolution.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_Forward_CancellationToken_For_Username()
    {
        // Arrange

        const string username =
            "john.doe";

        var cancellationToken =
            new CancellationTokenSource()
                .Token;

        var user =
            UserAccountFixture.Create();

        _userAccountRepository
            .Setup(x =>
                x.GetByUsernameAsync(
                    username,
                    cancellationToken))
            .ReturnsAsync(user);

        var sut =
            CreateSut();

        // Act

        await sut.ResolveAsync(
            username,
            cancellationToken);

        // Assert

        _userAccountRepository.Verify(
            x => x.GetByUsernameAsync(
                username,
                cancellationToken),
            Times.Once);
    }

    /// <summary>
    /// Verifies cancellation is honored before
    /// repository access.
    /// </summary>
    [Fact]
    public async Task ResolveAsync_Should_ThrowOperationCanceledException_When_Cancellation_Is_Requested()
    {
        // Arrange

        const string username =
            "john.doe";

        using var cancellationTokenSource =
            new CancellationTokenSource();

        cancellationTokenSource.Cancel();

        var sut =
            CreateSut();

        // Act

        Func<Task> act =
            () => sut.ResolveAsync(
                username,
                cancellationTokenSource.Token);

        // Assert

        await act.Should()
            .ThrowAsync<OperationCanceledException>();

        _userAccountRepository.Verify(
            x => x.GetByUsernameAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _userAccountRepository.Verify(
            x => x.GetByEmailAsync(
                It.IsAny<EmailAddress>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}