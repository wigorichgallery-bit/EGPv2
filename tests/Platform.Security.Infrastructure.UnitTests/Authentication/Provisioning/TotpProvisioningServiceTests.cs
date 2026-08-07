using FluentAssertions;
using Microsoft.Extensions.Options;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;
using Platform.Security.Infrastructure.Authentication.Configuration;
using Platform.Security.Infrastructure.Authentication.Provisioning;
using Xunit;

namespace Platform.Security.Infrastructure.UnitTests.Authentication.Provisioning;

/// <summary>
/// Unit tests for
/// <see cref="TotpProvisioningService"/>.
/// </summary>
public sealed class TotpProvisioningServiceTests
{
    private static readonly DateTime CreatedAtUtc =
        new(
            2026,
            1,
            1,
            12,
            0,
            0,
            DateTimeKind.Utc);

    private static TotpOptions CreateOptions()
    {
        return new TotpOptions
        {
            Issuer = "EGP Platform",
            Digits = 6,
            TimeStepSeconds = 30
        };
    }

    private static AuthenticationChallengeDeliveryRequest CreateRequest()
    {
        AuthenticationChallenge challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("EncryptedSecret"),
                CreatedAtUtc,
                CreatedAtUtc.AddMinutes(5));

        UserAccount user =
            new(
                Guid.NewGuid(),
                "john",
                new EmailAddress("john.doe@example.com"),
                new PhoneNumber("+628123456789"),
                "HASH",
                CreatedAtUtc);

        return new AuthenticationChallengeDeliveryRequest(
            challenge,
            user,
            "ABCDEF123456");
    }

    /// <summary>
    /// Verifies constructor rejects null options.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
    {
        // Act
        Action act =
            () => new TotpProvisioningService(
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies ProvisionAsync rejects null request.
    /// </summary>
    [Fact]
    public async Task ProvisionAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var sut =
            new TotpProvisioningService(
                Options.Create(
                    CreateOptions()));

        // Act
        Func<Task> act =
            () => sut.ProvisionAsync(null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies manual entry key equals the plaintext secret.
    /// </summary>
    [Fact]
    public async Task ProvisionAsync_ShouldReturnManualEntryKey()
    {
        // Arrange
        var request =
            CreateRequest();

        var sut =
            new TotpProvisioningService(
                Options.Create(
                    CreateOptions()));

        // Act
        TotpProvisioningResult result =
            await sut.ProvisionAsync(request);

        // Assert
        result.ManualEntryKey
            .Should()
            .Be(request.PlainTextSecret);
    }

    /// <summary>
    /// Verifies provisioning URI contains all required fields.
    /// </summary>
    [Fact]
    public async Task ProvisionAsync_ShouldGenerateProvisioningUri()
    {
        // Arrange
        var request =
            CreateRequest();

        var options =
            CreateOptions();

        var sut =
            new TotpProvisioningService(
                Options.Create(options));

        // Act
        TotpProvisioningResult result =
            await sut.ProvisionAsync(request);

        // Assert
        result.ProvisioningUri
            .Should()
            .StartWith("otpauth://totp/");

        result.ProvisioningUri
            .Should()
            .Contain("secret=ABCDEF123456");

        result.ProvisioningUri
            .Should()
            .Contain("&issuer=EGP%20Platform");

        result.ProvisioningUri
            .Should()
            .Contain("&algorithm=SHA1");

        result.ProvisioningUri
            .Should()
            .Contain("&digits=6");

        result.ProvisioningUri
            .Should()
            .Contain("&period=30");
    }

    /// <summary>
    /// Verifies issuer and account are URL encoded.
    /// </summary>
    [Fact]
    public async Task ProvisionAsync_ShouldEscapeIssuerAndAccount()
    {
        // Arrange
        var options =
            new TotpOptions
            {
                Issuer = "My Company",
                Digits = 8,
                TimeStepSeconds = 60
            };

        var request =
            CreateRequest();

        var sut =
            new TotpProvisioningService(
                Options.Create(options));

        // Act
        TotpProvisioningResult result =
            await sut.ProvisionAsync(request);

        // Assert
        result.ProvisioningUri
            .Should()
            .Contain("My%20Company");

        result.ProvisioningUri
            .Should()
            .Contain("john.doe%40example.com");
    }

    /// <summary>
    /// Verifies configured digits and period are used.
    /// </summary>
    [Fact]
    public async Task ProvisionAsync_ShouldUseConfiguredOptions()
    {
        // Arrange
        var options =
            new TotpOptions
            {
                Issuer = "Issuer",
                Digits = 8,
                TimeStepSeconds = 90
            };

        var request =
            CreateRequest();

        var sut =
            new TotpProvisioningService(
                Options.Create(options));

        // Act
        TotpProvisioningResult result =
            await sut.ProvisionAsync(request);

        // Assert
        result.ProvisioningUri
            .Should()
            .Contain("&digits=8");

        result.ProvisioningUri
            .Should()
            .Contain("&period=90");
    }

    /// <summary>
    /// Verifies cancellation token does not affect
    /// synchronous provisioning.
    /// </summary>
    [Fact]
    public async Task ProvisionAsync_ShouldIgnoreCancellationToken()
    {
        // Arrange
        var request =
            CreateRequest();

        var sut =
            new TotpProvisioningService(
                Options.Create(
                    CreateOptions()));

        using var cts =
            new CancellationTokenSource();

        // Act
        TotpProvisioningResult result =
            await sut.ProvisionAsync(
                request,
                cts.Token);

        // Assert
        result.Should()
            .NotBeNull();

        result.ManualEntryKey
            .Should()
            .Be(request.PlainTextSecret);
    }
}