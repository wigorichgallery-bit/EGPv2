using Platform.Communication.Models;

namespace Platform.Communication.UnitTests.Models;

/// <summary>
/// Contains unit tests for <see cref="VendorDeliveryResult"/>.
/// </summary>
public sealed class VendorDeliveryResultTests
{
    /// <summary>
    /// Verifies that the constructor stores
    /// all supplied values.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_When_ArgumentsAreValid()
    {
        // Arrange
        object rawResponse = new();

        // Act
        VendorDeliveryResult result = new(
            messageId: "MSG-001",
            providerReference: "REF-001",
            status: "Delivered",
            rawResponse: rawResponse);

        // Assert
        result.MessageId.Should().Be("MSG-001");
        result.ProviderReference.Should().Be("REF-001");
        result.Status.Should().Be("Delivered");
        result.RawResponse.Should().BeSameAs(rawResponse);
    }

    /// <summary>
    /// Verifies that the constructor allows
    /// null values for all optional properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetNullProperties_When_ArgumentsAreNull()
    {
        // Arrange

        // Act
        VendorDeliveryResult result = new(
            messageId: null);

        // Assert
        result.MessageId.Should().BeNull();
        result.ProviderReference.Should().BeNull();
        result.Status.Should().BeNull();
        result.RawResponse.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the success factory method
    /// creates a populated result.
    /// </summary>
    [Fact]
    public void Success_Should_CreateVendorDeliveryResult()
    {
        // Arrange
        object rawResponse = new();

        // Act
        VendorDeliveryResult result =
            VendorDeliveryResult.Success(
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered",
                rawResponse: rawResponse);

        // Assert
        result.MessageId.Should().Be("MSG-001");
        result.ProviderReference.Should().Be("REF-001");
        result.Status.Should().Be("Delivered");
        result.RawResponse.Should().BeSameAs(rawResponse);
    }

    /// <summary>
    /// Verifies that two vendor delivery results
    /// having identical values are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange
        object rawResponse = new();

        VendorDeliveryResult left = new(
            "MSG-001",
            "REF-001",
            "Delivered",
            rawResponse);

        VendorDeliveryResult right = new(
            "MSG-001",
            "REF-001",
            "Delivered",
            rawResponse);

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two vendor delivery results
    /// having different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValuesAreDifferent()
    {
        // Arrange
        VendorDeliveryResult left = new(
            "MSG-001");

        VendorDeliveryResult right = new(
            "MSG-002");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that equal vendor delivery results
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValuesAreEqual()
    {
        // Arrange
        VendorDeliveryResult left = new(
            "MSG-001",
            "REF-001",
            "Delivered");

        VendorDeliveryResult right = new(
            "MSG-001",
            "REF-001",
            "Delivered");

        // Act

        // Assert
        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// true for equal values.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange
        VendorDeliveryResult left = new(
            "MSG-001",
            "REF-001",
            "Delivered");

        VendorDeliveryResult right = new(
            "MSG-001",
            "REF-001",
            "Delivered");

        // Act
        bool result = left == right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// true for different values.
    /// </summary>
    [Fact]
    public void InequalityOperator_Should_ReturnTrue_When_ValuesAreDifferent()
    {
        // Arrange
        VendorDeliveryResult left = new(
            "MSG-001");

        VendorDeliveryResult right = new(
            "MSG-002");

        // Act
        bool result = left != right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that equality operators correctly
    /// handle null operands.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_HandleNullOperands()
    {
        // Arrange
        VendorDeliveryResult? left = null;
        VendorDeliveryResult? right = null;

        VendorDeliveryResult value = new(
            "MSG-001");

        // Act

        // Assert
        (left == right).Should().BeTrue();
        (left != right).Should().BeFalse();

        (left == value).Should().BeFalse();
        (left != value).Should().BeTrue();

        (value == right).Should().BeFalse();
        (value != right).Should().BeTrue();
    }
}