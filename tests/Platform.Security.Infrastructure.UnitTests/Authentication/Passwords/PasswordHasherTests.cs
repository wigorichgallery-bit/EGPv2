using FluentAssertions;
using Platform.Security.Infrastructure.Passwords;
using Xunit;

namespace Platform.Security.Infrastructure.UnitTests.Authentication.Passwords;

/// <summary>
/// Unit tests for <see cref="PasswordHasher"/>.
/// </summary>
public sealed class PasswordHasherTests
{
    private readonly PasswordHasher _sut = new();

    /// <summary>
    /// Verifies that Hash throws when password is null.
    /// </summary>
    [Fact]
    public void Hash_ShouldThrowArgumentNullException_WhenPasswordIsNull()
    {
        // Arrange
        string password = null!;

        // Act
        Action act = () => _sut.Hash(password);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that Hash returns a valid composite payload.
    /// </summary>
    [Fact]
    public void Hash_ShouldReturnCompositeHash_WhenPasswordIsValid()
    {
        // Arrange
        const string password = "MyPassword123!";

        // Act
        var result = _sut.Hash(password);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();

        var parts = result.Split('.');

        parts.Should().HaveCount(3);
        parts[0].Should().Be("100000");

        Convert.FromBase64String(parts[1])
            .Should()
            .HaveCount(32);

        Convert.FromBase64String(parts[2])
            .Should()
            .HaveCount(64);
    }

    /// <summary>
    /// Verifies that different hashes are produced for the same password.
    /// </summary>
    [Fact]
    public void Hash_ShouldGenerateDifferentHashes_ForSamePassword()
    {
        // Arrange
        const string password = "Password123!";

        // Act
        var hash1 = _sut.Hash(password);
        var hash2 = _sut.Hash(password);

        // Assert
        hash1.Should().NotBe(hash2);

        _sut.Verify(password, hash1).Should().BeTrue();
        _sut.Verify(password, hash2).Should().BeTrue();
    }

    /// <summary>
    /// Verifies empty password can be hashed and verified.
    /// </summary>
    [Fact]
    public void Hash_ShouldSupportEmptyPassword()
    {
        // Arrange
        const string password = "";

        // Act
        var hash = _sut.Hash(password);

        // Assert
        _sut.Verify(password, hash).Should().BeTrue();
    }

    /// <summary>
    /// Verifies unicode passwords are supported.
    /// </summary>
    [Fact]
    public void Hash_ShouldSupportUnicodePassword()
    {
        // Arrange
        const string password = "Pässw🔒rd日本語";

        // Act
        var hash = _sut.Hash(password);

        // Assert
        _sut.Verify(password, hash).Should().BeTrue();
    }

    /// <summary>
    /// Verifies Verify throws when password is null.
    /// </summary>
    [Fact]
    public void Verify_ShouldThrowArgumentNullException_WhenPasswordIsNull()
    {
        // Arrange
        string password = null!;
        var hash = _sut.Hash("abc");

        // Act
        Action act = () => _sut.Verify(password, hash);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies Verify throws when hash is null.
    /// </summary>
    [Fact]
    public void Verify_ShouldThrowArgumentNullException_WhenHashIsNull()
    {
        // Arrange
        string hash = null!;

        // Act
        Action act = () => _sut.Verify("abc", hash);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies malformed payload returns false.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("1.2")]
    [InlineData("1.2.3.4")]
    public void Verify_ShouldReturnFalse_WhenPayloadFormatIsInvalid(string payload)
    {
        // Act
        var result = _sut.Verify("password", payload);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies invalid iteration returns false.
    /// </summary>
    [Fact]
    public void Verify_ShouldReturnFalse_WhenIterationIsInvalid()
    {
        // Arrange
        var payload = "abc.YWJj.ZGVm";

        // Act
        var result = _sut.Verify("password", payload);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies invalid Base64 salt returns false.
    /// </summary>
    [Fact]
    public void Verify_ShouldReturnFalse_WhenSaltIsInvalidBase64()
    {
        // Arrange
        var payload = "100000.@@@@.AAAA";

        // Act
        var result = _sut.Verify("password", payload);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies invalid Base64 hash returns false.
    /// </summary>
    [Fact]
    public void Verify_ShouldReturnFalse_WhenHashIsInvalidBase64()
    {
        // Arrange
        var salt = Convert.ToBase64String(new byte[32]);
        var payload = $"100000.{salt}.@@@@";

        // Act
        var result = _sut.Verify("password", payload);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies wrong password fails verification.
    /// </summary>
    [Fact]
    public void Verify_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        // Arrange
        var hash = _sut.Hash("CorrectPassword");

        // Act
        var result = _sut.Verify("WrongPassword", hash);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies tampered payload fails verification.
    /// </summary>
    [Fact]
    public void Verify_ShouldReturnFalse_WhenPayloadIsTampered()
    {
        // Arrange
        var hash = _sut.Hash("password");

        var parts = hash.Split('.');
        parts[2] = Convert.ToBase64String(new byte[64]);

        var tampered = string.Join(".", parts);

        // Act
        var result = _sut.Verify("password", tampered);

        // Assert
        result.Should().BeFalse();
    }
}