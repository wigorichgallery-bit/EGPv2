// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// ValueObjects/ChallengeSecretTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.ValueObjects;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="ChallengeSecret"/>
/// value object.
/// </summary>
public sealed class ChallengeSecretTests
{
    #region Constructor Tests

    /// <summary>
    /// Verifies that a valid challenge secret creates a new
    /// <see cref="ChallengeSecret"/> instance.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsValid_ShouldCreateInstance()
    {
        // Arrange
        const string secret = "ABC123";

        // Act
        var challengeSecret = new ChallengeSecret(secret);

        // Assert
        challengeSecret.Value.Should().Be(secret);
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the challenge secret is <see langword="null"/>.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        string? value = null;

        // Act
        Action act = () => new ChallengeSecret(value!);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the challenge secret is empty.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        const string value = "";

        // Act
        Action act = () => new ChallengeSecret(value);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the challenge secret contains only whitespace.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsWhiteSpace_ShouldThrowArgumentException()
    {
        // Arrange
        const string value = " ";

        // Act
        Action act = () => new ChallengeSecret(value);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the challenge secret contains only a tab character.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsTab_ShouldThrowArgumentException()
    {
        // Arrange
        const string value = "\t";

        // Act
        Action act = () => new ChallengeSecret(value);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the challenge secret contains only a newline character.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsNewLine_ShouldThrowArgumentException()
    {
        // Arrange
        const string value = "\n";

        // Act
        Action act = () => new ChallengeSecret(value);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that Unicode characters are accepted as a valid
    /// challenge secret.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsUnicode_ShouldCreateInstance()
    {
        // Arrange
        const string secret = "秘密123";

        // Act
        var challengeSecret = new ChallengeSecret(secret);

        // Assert
        challengeSecret.Value.Should().Be(secret);
    }

    /// <summary>
    /// Verifies that a long challenge secret is accepted without
    /// modification.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsLong_ShouldCreateInstance()
    {
        // Arrange
        var secret = new string('A', 1024);

        // Act
        var challengeSecret = new ChallengeSecret(secret);

        // Assert
        challengeSecret.Value.Should().Be(secret);
    }

    /// <summary>
    /// Verifies that special characters are preserved when creating
    /// a <see cref="ChallengeSecret"/> instance.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueContainsSpecialCharacters_ShouldCreateInstance()
    {
        // Arrange
        const string secret = "!@#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        var challengeSecret = new ChallengeSecret(secret);

        // Assert
        challengeSecret.Value.Should().Be(secret);
    }

    #endregion

    #region Property Tests

    /// <summary>
    /// Verifies that the original challenge secret value is preserved
    /// after construction.
    /// </summary>
    [Fact]
    public void Value_WhenConstructed_ShouldReturnOriginalValue()
    {
        // Arrange
        const string secret = "OTP-SECRET";

        // Act
        var challengeSecret = new ChallengeSecret(secret);

        // Assert
        challengeSecret.Value.Should().Be(secret);
    }

    /// <summary>
    /// Verifies that the <see cref="ChallengeSecret.Value"/>
    /// property is immutable.
    /// </summary>
    [Fact]
    public void Value_ShouldBeImmutable()
    {
        // Arrange
        var property = typeof(ChallengeSecret)
            .GetProperty(nameof(ChallengeSecret.Value));

        // Act

        // Assert
        property.Should().NotBeNull();
        property!.CanWrite.Should().BeFalse();
    }

    #endregion

    #region Equality Tests

    /// <summary>
    /// Verifies that two instances having the same challenge secret
    /// are structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var left = new ChallengeSecret("ABC123");
        var right = new ChallengeSecret("ABC123");

        // Act
        var result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two instances having different challenge secrets
    /// are not structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new ChallengeSecret("ABC123");
        var right = new ChallengeSecret("XYZ789");

        // Act
        var result = left.Equals(right);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that comparing with <see langword="null"/>
    /// returns <see langword="false"/>.
    /// </summary>
    [Fact]
    public void Equals_WhenComparedWithNull_ShouldReturnFalse()
    {
        // Arrange
        var challengeSecret = new ChallengeSecret("ABC123");

        // Act
        var result = challengeSecret.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that comparing with an object of a different type
    /// returns <see langword="false"/>.
    /// </summary>
    [Fact]
    public void Equals_WhenComparedWithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var challengeSecret = new ChallengeSecret("ABC123");

        // Act
        var result = challengeSecret.Equals(new object());

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="true"/> for equal values.
    /// </summary>
    [Fact]
    public void EqualityOperator_WhenValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var left = new ChallengeSecret("ABC123");
        var right = new ChallengeSecret("ABC123");

        // Act
        var result = left == right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="false"/> for different values.
    /// </summary>
    [Fact]
    public void EqualityOperator_WhenValuesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new ChallengeSecret("ABC123");
        var right = new ChallengeSecret("XYZ789");

        // Act
        var result = left == right;

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="true"/> for different values.
    /// </summary>
    [Fact]
    public void InequalityOperator_WhenValuesAreDifferent_ShouldReturnTrue()
    {
        // Arrange
        var left = new ChallengeSecret("ABC123");
        var right = new ChallengeSecret("XYZ789");

        // Act
        var result = left != right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="false"/> for equal values.
    /// </summary>
    [Fact]
    public void InequalityOperator_WhenValuesAreEqual_ShouldReturnFalse()
    {
        // Arrange
        var left = new ChallengeSecret("ABC123");
        var right = new ChallengeSecret("ABC123");

        // Act
        var result = left != right;

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region HashCode Tests

    /// <summary>
    /// Verifies that equal challenge secrets produce the same
    /// hash code.
    /// </summary>
    [Fact]
    public void GetHashCode_WhenValuesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var left = new ChallengeSecret("ABC123");
        var right = new ChallengeSecret("ABC123");

        // Act
        var leftHash = left.GetHashCode();
        var rightHash = right.GetHashCode();

        // Assert
        leftHash.Should().Be(rightHash);
    }

    #endregion

    #region ToString Tests

    /// <summary>
    /// Verifies that <see cref="ChallengeSecret.ToString"/>
    /// always returns a masked representation.
    /// </summary>
    [Fact]
    public void ToString_ShouldReturnMaskedValue()
    {
        // Arrange
        var challengeSecret = new ChallengeSecret("SUPER-SECRET");

        // Act
        var result = challengeSecret.ToString();

        // Assert
        result.Should().Be("********");
    }

    /// <summary>
    /// Verifies that <see cref="ChallengeSecret.ToString"/>
    /// never exposes the underlying challenge secret.
    /// </summary>
    [Fact]
    public void ToString_ShouldNotExposeUnderlyingSecret()
    {
        // Arrange
        const string secret = "SUPER-SECRET";
        var challengeSecret = new ChallengeSecret(secret);

        // Act
        var result = challengeSecret.ToString();

        // Assert
        result.Should().NotBe(secret);
        result.Should().NotContain(secret);
    }

    #endregion
}