using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.Identity.Application.Features.Authentication.Policies;
using Platform.Identity.Application.Features.Authentication.Policies.Models;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.Aggregates;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Policies;

/// <summary>
/// Unit tests for <see cref="VerificationPolicy"/>.
/// </summary>
public sealed class VerificationPolicyTests
{
    private readonly VerificationPolicy _sut = new();

    /// <summary>
    /// Gets users that satisfy the verification policy.
    /// </summary>
    public static TheoryData<UserAccount> VerifiedUsers =>
        new()
        {
            UserAccountFixture.CreateEmailVerified(),
            UserAccountFixture.CreatePhoneVerified()
        };

    /// <summary>
    /// Verifies a null context throws an exception.
    /// </summary>
    [Fact]
    public async Task EvaluateAsync_Should_ThrowArgumentNullException_When_Context_Is_Null()
    {
        // Act
        Func<Task> act = () => _sut.EvaluateAsync(null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("context");
    }

    /// <summary>
    /// Verifies a cancelled token throws an exception.
    /// </summary>
    [Fact]
    public async Task EvaluateAsync_Should_ThrowOperationCanceledException_When_Cancellation_Is_Requested()
    {
        // Arrange
        var context = CreateContext();

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        Func<Task> act =
            () => _sut.EvaluateAsync(
                context,
                cts.Token);

        // Assert
        await act.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Verifies authentication stops when no contact method
    /// has been verified.
    /// </summary>
    [Fact]
    public async Task EvaluateAsync_Should_Stop_When_No_Contact_Is_Verified()
    {
        // Arrange
        var context = CreateContext();

        // Act
        var result = await _sut.EvaluateAsync(context);

        // Assert
        result.IsSuccessful.Should().BeFalse();

        result.ShouldContinue.Should().BeFalse();

        result.Decision.Decision.Should()
            .Be(AuthenticationDecisionType.RequireVerification);

        result.Decision.Reason.Should()
            .Be("At least one verified contact method is required.");
    }

    /// <summary>
    /// Verifies authentication continues when at least one
    /// contact method has been verified.
    /// </summary>
    [Theory]
    [MemberData(nameof(VerifiedUsers))]
    public async Task EvaluateAsync_Should_Continue_When_At_Least_One_Contact_Is_Verified(
        UserAccount user)
    {
        // Arrange
        var context = CreateContext(user);

        // Act
        var result = await _sut.EvaluateAsync(context);

        // Assert
        result.IsSuccessful.Should().BeTrue();

        result.ShouldContinue.Should().BeTrue();

        result.Decision.Decision.Should()
            .Be(AuthenticationDecisionType.Allow);

        result.Decision.Reason.Should().BeNull();
    }

    /// <summary>
    /// Creates an authentication context.
    /// </summary>
    /// <param name="user">
    /// Optional user.
    /// </param>
    private static AuthenticationContext CreateContext(
        UserAccount? user = null)
    {
        return new AuthenticationContext(
            user ?? UserAccountFixture.Create(),
            new LoginRequest(
                "john",
                "password"),
            DateTimeOffset.UtcNow);
    }
}