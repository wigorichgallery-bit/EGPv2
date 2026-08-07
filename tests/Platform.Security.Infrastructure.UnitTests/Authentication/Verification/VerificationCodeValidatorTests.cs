using FluentAssertions;
using Platform.Security.Infrastructure.Verification;
using Xunit;

namespace Platform.Security.Infrastructure.UnitTests.Authentication.Verification;

/// <summary>
/// Unit tests for <see cref="VerificationCodeValidator"/>.
/// </summary>
public sealed class VerificationCodeValidatorTests
{
    /// <summary>
    /// Verifies ValidateAsync throws when verification code is null.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldThrowArgumentNullException_WhenVerificationCodeIsNull()
    {
        // Arrange
        var sut = new VerificationCodeValidator();

        // Act
        Func<Task> act = async () =>
            await sut.ValidateAsync(
                Guid.NewGuid(),
                null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies ValidateAsync returns true for an empty verification code.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldReturnTrue_WhenVerificationCodeIsEmpty()
    {
        // Arrange
        var sut = new VerificationCodeValidator();

        // Act
        bool result = await sut.ValidateAsync(
            Guid.NewGuid(),
            string.Empty);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies ValidateAsync returns true for a whitespace verification code.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldReturnTrue_WhenVerificationCodeIsWhitespace()
    {
        // Arrange
        var sut = new VerificationCodeValidator();

        // Act
        bool result = await sut.ValidateAsync(
            Guid.NewGuid(),
            "   ");

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies ValidateAsync returns true for a valid verification code.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldReturnTrue_WhenVerificationCodeIsProvided()
    {
        // Arrange
        var sut = new VerificationCodeValidator();

        // Act
        bool result = await sut.ValidateAsync(
            Guid.NewGuid(),
            "123456");

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies cancellation token does not affect current implementation.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldReturnTrue_WhenCancellationTokenIsCancelled()
    {
        // Arrange
        var sut = new VerificationCodeValidator();

        using var cancellationTokenSource =
            new CancellationTokenSource();

        cancellationTokenSource.Cancel();

        // Act
        bool result = await sut.ValidateAsync(
            Guid.NewGuid(),
            "123456",
            cancellationTokenSource.Token);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies different user identifiers produce the same result.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldIgnoreUserId_InCurrentImplementation()
    {
        // Arrange
        var sut = new VerificationCodeValidator();

        // Act
        bool result1 = await sut.ValidateAsync(
            Guid.NewGuid(),
            "ABC123");

        bool result2 = await sut.ValidateAsync(
            Guid.Empty,
            "ABC123");

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeTrue();
    }
}