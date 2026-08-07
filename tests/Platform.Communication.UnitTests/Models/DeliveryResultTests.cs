using Platform.Communication.Models;

namespace Platform.Communication.UnitTests.Models;

/// <summary>
/// Contains unit tests for <see cref="DeliveryResult"/>.
/// </summary>
public sealed class DeliveryResultTests
{
    /// <summary>
    /// Verifies that the constructor creates
    /// a successful delivery result.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_When_Succeeded()
    {
        // Arrange

        // Act
        DeliveryResult result = new(
            succeeded: true,
            providerMessageId: "MSG-001");

        // Assert
        result.Succeeded.Should().BeTrue();
        result.ProviderMessageId.Should().Be("MSG-001");
        result.ErrorMessage.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the constructor creates
    /// a failed delivery result.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_When_Failed()
    {
        // Arrange

        // Act
        DeliveryResult result = new(
            succeeded: false,
            providerMessageId: "MSG-001",
            errorMessage: "Delivery failed.");

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ProviderMessageId.Should().Be("MSG-001");
        result.ErrorMessage.Should().Be("Delivery failed.");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when a successful result contains an error message.
    /// </summary>
    [Theory]
    [InlineData("Error")]
    [InlineData("Failure")]
    public void Constructor_Should_ThrowArgumentException_When_SucceededContainsError(
        string errorMessage)
    {
        // Arrange

        // Act
        Action action = () =>
            _ = new DeliveryResult(
                succeeded: true,
                errorMessage: errorMessage);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errorMessage")
            .WithMessage(
                "A successful delivery result cannot contain an error message.*");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when a failed result does not contain an error message.
    /// </summary>
    /// <param name="errorMessage">
    /// Invalid error message.
    /// </param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    public void Constructor_Should_ThrowArgumentException_When_FailedWithoutError(
        string? errorMessage)
    {
        // Arrange

        // Act
        Action action = () =>
            _ = new DeliveryResult(
                succeeded: false,
                errorMessage: errorMessage);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errorMessage")
            .WithMessage(
                "A failed delivery result must contain an error message.*");
    }

    /// <summary>
    /// Verifies that whitespace provider message identifiers
    /// are normalized to null.
    /// </summary>
    /// <param name="providerMessageId">
    /// Provider message identifier.
    /// </param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_Should_NormalizeProviderMessageId_When_ValueIsWhitespace(
        string? providerMessageId)
    {
        // Arrange

        // Act
        DeliveryResult result = new(
            succeeded: true,
            providerMessageId: providerMessageId);

        // Assert
        result.ProviderMessageId.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the success factory method
    /// creates a successful result.
    /// </summary>
    [Fact]
    public void Success_Should_CreateSuccessfulResult()
    {
        // Arrange

        // Act
        DeliveryResult result =
            DeliveryResult.Success("MSG-001");

        // Assert
        result.Succeeded.Should().BeTrue();
        result.ProviderMessageId.Should().Be("MSG-001");
        result.ErrorMessage.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the success factory method
    /// normalizes whitespace provider identifiers.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Success_Should_NormalizeProviderMessageId_When_ValueIsWhitespace(
        string? providerMessageId)
    {
        // Arrange

        // Act
        DeliveryResult result =
            DeliveryResult.Success(providerMessageId);

        // Assert
        result.ProviderMessageId.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the failure factory method
    /// creates a failed result.
    /// </summary>
    [Fact]
    public void Failure_Should_CreateFailedResult()
    {
        // Arrange

        // Act
        DeliveryResult result =
            DeliveryResult.Failure(
                "Delivery failed.",
                "MSG-001");

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ProviderMessageId.Should().Be("MSG-001");
        result.ErrorMessage.Should().Be("Delivery failed.");
    }

    /// <summary>
    /// Verifies that equal delivery results
    /// are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange
        DeliveryResult left =
            DeliveryResult.Success("MSG-001");

        DeliveryResult right =
            DeliveryResult.Success("MSG-001");

        // Act / Assert
        left.Equals(right).Should().BeTrue();
        (left == right).Should().BeTrue();
        (left != right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that different delivery results
    /// are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValuesAreDifferent()
    {
        // Arrange
        DeliveryResult left =
            DeliveryResult.Success("MSG-001");

        DeliveryResult right =
            DeliveryResult.Success("MSG-002");

        // Act / Assert
        left.Equals(right).Should().BeFalse();
        (left == right).Should().BeFalse();
        (left != right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that equal delivery results
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValuesAreEqual()
    {
        // Arrange
        DeliveryResult left =
            DeliveryResult.Success("MSG-001");

        DeliveryResult right =
            DeliveryResult.Success("MSG-001");

        // Act / Assert
        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies that equality operators correctly
    /// handle null operands.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_HandleNullOperands()
    {
        // Arrange
        DeliveryResult? left = null;
        DeliveryResult? right = null;

        DeliveryResult value =
            DeliveryResult.Success("MSG-001");

        // Act / Assert
        (left == right).Should().BeTrue();
        (left != right).Should().BeFalse();

        (left == value).Should().BeFalse();
        (left != value).Should().BeTrue();

        (value == right).Should().BeFalse();
        (value != right).Should().BeTrue();
    }
}