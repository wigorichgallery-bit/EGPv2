
using Platform.Security.Infrastructure.Totp;


namespace Platform.Security.Infrastructure.UnitTests.Authentication.Totp;

/// <summary>
/// Unit tests for <see cref="TotpSecretGenerator"/>.
/// </summary>
public sealed class TotpSecretGeneratorTests
{
    private readonly TotpSecretGenerator _sut = new();

    /// <summary>
    /// Verifies GenerateSecret returns a non-empty secret.
    /// </summary>
    [Fact]
    public void GenerateSecret_ShouldReturnNonEmptySecret()
    {
        // Act
        var secret = _sut.GenerateSecret();

        // Assert
        secret.Should().NotBeNullOrWhiteSpace();
    }

    /// <summary>
    /// Verifies GenerateSecret returns a Base32 string
    /// with the expected RFC 4226 length.
    /// </summary>
    [Fact]
    public void GenerateSecret_ShouldReturnExpectedLength()
    {
        // Act
        var secret = _sut.GenerateSecret();

        // Assert
        secret.Should().HaveLength(32);
    }

    /// <summary>
    /// Verifies GenerateSecret contains only RFC 4648
    /// Base32 alphabet characters.
    /// </summary>
    [Fact]
    public void GenerateSecret_ShouldContainOnlyBase32Characters()
    {
        // Act
        var secret = _sut.GenerateSecret();

        // Assert
        const string alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        secret.All(alphabet.Contains)
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies GenerateSecret does not contain padding.
    /// </summary>
    [Fact]
    public void GenerateSecret_ShouldNotContainPadding()
    {
        // Act
        var secret = _sut.GenerateSecret();

        // Assert
        secret.Should().NotContain("=");
    }

    /// <summary>
    /// Verifies multiple generated secrets satisfy the
    /// expected Base32 format.
    /// </summary>
    [Fact]
    public void GenerateSecret_ShouldProduceValidSecretsAcrossMultipleInvocations()
    {
        // Act
        var secrets = Enumerable
            .Range(0, 100)
            .Select(_ => _sut.GenerateSecret())
            .ToList();

        // Assert
        const string alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        foreach (var secret in secrets)
        {
            secret.Should().HaveLength(32);

            secret.All(alphabet.Contains)
                .Should()
                .BeTrue();

            secret.Should().NotContain("=");
        }
    }

    /// <summary>
    /// Verifies generated secrets are not identical
    /// across multiple invocations.
    /// </summary>
    [Fact]
    public void GenerateSecret_ShouldProduceDifferentSecrets()
    {
        // Act
        var first = _sut.GenerateSecret();
        var second = _sut.GenerateSecret();

        // Assert
        first.Should().NotBe(second);
    }
}