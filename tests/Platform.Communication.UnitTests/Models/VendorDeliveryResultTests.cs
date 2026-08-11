using FluentAssertions;

using Platform.Communication.Models;

namespace Platform.Communication.UnitTests.Models;

/// <summary>
/// Contains unit tests for
/// <see cref="VendorDeliveryResult"/>.
/// </summary>
public sealed class VendorDeliveryResultTests
{
    // ==========================================================
    // Constructor - Success
    // ==========================================================

    /// <summary>
    /// Verifies that the constructor stores
    /// all supplied success values.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_When_SuccessArgumentsAreValid()
    {
        // Arrange

        object rawResponse =
            new();

        // Act

        VendorDeliveryResult result =
            new(
                isSuccess: true,
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered",
                rawResponse: rawResponse);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        result.MessageId
            .Should()
            .Be("MSG-001");

        result.ProviderReference
            .Should()
            .Be("REF-001");

        result.Status
            .Should()
            .Be("Delivered");

        result.ErrorMessage
            .Should()
            .BeNull();

        result.RawResponse
            .Should()
            .BeSameAs(rawResponse);
    }

    /// <summary>
    /// Verifies that the constructor allows
    /// null optional values for a successful result.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetNullProperties_When_SuccessOptionalArgumentsAreNull()
    {
        // Act

        VendorDeliveryResult result =
            new(
                isSuccess: true);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        result.MessageId
            .Should()
            .BeNull();

        result.ProviderReference
            .Should()
            .BeNull();

        result.Status
            .Should()
            .BeNull();

        result.ErrorMessage
            .Should()
            .BeNull();

        result.RawResponse
            .Should()
            .BeNull();
    }

    // ==========================================================
    // Constructor - Failure
    // ==========================================================

    /// <summary>
    /// Verifies that the constructor stores
    /// all supplied failure values.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_When_FailureArgumentsAreValid()
    {
        // Arrange

        object rawResponse =
            new();

        // Act

        VendorDeliveryResult result =
            new(
                isSuccess: false,
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Failed",
                errorMessage: "Delivery failed.",
                rawResponse: rawResponse);

        // Assert

        result.IsSuccess
            .Should()
            .BeFalse();

        result.MessageId
            .Should()
            .Be("MSG-001");

        result.ProviderReference
            .Should()
            .Be("REF-001");

        result.Status
            .Should()
            .Be("Failed");

        result.ErrorMessage
            .Should()
            .Be("Delivery failed.");

        result.RawResponse
            .Should()
            .BeSameAs(rawResponse);
    }

    /// <summary>
    /// Verifies that a failed result requires
    /// an error message.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentException_When_FailureHasNoErrorMessage()
    {
        // Act

        Action action =
            () =>
                new VendorDeliveryResult(
                    isSuccess: false);

        // Assert

        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(
                "errorMessage")
            .WithMessage(
                "A failed vendor result must contain an error message.*");
    }

    /// <summary>
    /// Verifies that a successful result cannot
    /// contain an error message.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentException_When_SuccessHasErrorMessage()
    {
        // Act

        Action action =
            () =>
                new VendorDeliveryResult(
                    isSuccess: true,
                    errorMessage: "Unexpected error.");

        // Assert

        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(
                "errorMessage")
            .WithMessage(
                "A successful vendor result cannot contain an error message.*");
    }

    // ==========================================================
    // Constructor - Whitespace Normalization
    // ==========================================================

    /// <summary>
    /// Verifies that whitespace-only optional string values
    /// are normalized to null.
    /// </summary>
    [Fact]
    public void Constructor_Should_NormalizeWhitespaceValues_ToNull()
    {
        // Act

        VendorDeliveryResult result =
            new(
                isSuccess: true,
                messageId: " ",
                providerReference: " ",
                status: " ");

        // Assert

        result.MessageId
            .Should()
            .BeNull();

        result.ProviderReference
            .Should()
            .BeNull();

        result.Status
            .Should()
            .BeNull();
    }

    // ==========================================================
    // Success Factory
    // ==========================================================

    /// <summary>
    /// Verifies that the success factory method
    /// creates a successful result.
    /// </summary>
    [Fact]
    public void Success_Should_CreateSuccessfulVendorDeliveryResult()
    {
        // Arrange

        object rawResponse =
            new();

        // Act

        VendorDeliveryResult result =
            VendorDeliveryResult.Success(
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered",
                rawResponse: rawResponse);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        result.MessageId
            .Should()
            .Be("MSG-001");

        result.ProviderReference
            .Should()
            .Be("REF-001");

        result.Status
            .Should()
            .Be("Delivered");

        result.ErrorMessage
            .Should()
            .BeNull();

        result.RawResponse
            .Should()
            .BeSameAs(rawResponse);
    }

    /// <summary>
    /// Verifies that the success factory method
    /// allows all optional values to be omitted.
    /// </summary>
    [Fact]
    public void Success_Should_CreateResult_When_OptionalValuesAreOmitted()
    {
        // Act

        VendorDeliveryResult result =
            VendorDeliveryResult.Success();

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        result.MessageId
            .Should()
            .BeNull();

        result.ProviderReference
            .Should()
            .BeNull();

        result.Status
            .Should()
            .BeNull();

        result.ErrorMessage
            .Should()
            .BeNull();

        result.RawResponse
            .Should()
            .BeNull();
    }

    // ==========================================================
    // Failure Factory
    // ==========================================================

    /// <summary>
    /// Verifies that the failure factory method
    /// creates a failed result.
    /// </summary>
    [Fact]
    public void Failure_Should_CreateFailedVendorDeliveryResult()
    {
        // Arrange

        object rawResponse =
            new();

        // Act

        VendorDeliveryResult result =
            VendorDeliveryResult.Failure(
                errorMessage: "Delivery failed.",
                providerReference: "REF-001",
                status: "Failed",
                rawResponse: rawResponse);

        // Assert

        result.IsSuccess
            .Should()
            .BeFalse();

        result.MessageId
            .Should()
            .BeNull();

        result.ProviderReference
            .Should()
            .Be("REF-001");

        result.Status
            .Should()
            .Be("Failed");

        result.ErrorMessage
            .Should()
            .Be("Delivery failed.");

        result.RawResponse
            .Should()
            .BeSameAs(rawResponse);
    }

    /// <summary>
    /// Verifies that the failure factory method
    /// requires an error message.
    /// </summary>
    [Fact]
    public void Failure_Should_ThrowArgumentException_When_ErrorMessageIsNull()
    {
        // Act

        Action action =
            () =>
                VendorDeliveryResult.Failure(
                    null!);

        // Assert

        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(
                "errorMessage");
    }

    /// <summary>
    /// Verifies that the failure factory method
    /// rejects an empty error message.
    /// </summary>
    [Fact]
    public void Failure_Should_ThrowArgumentException_When_ErrorMessageIsEmpty()
    {
        // Act

        Action action =
            () =>
                VendorDeliveryResult.Failure(
                    string.Empty);

        // Assert

        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(
                "errorMessage");
    }

    /// <summary>
    /// Verifies that the failure factory method
    /// rejects a whitespace error message.
    /// </summary>
    [Fact]
    public void Failure_Should_ThrowArgumentException_When_ErrorMessageIsWhitespace()
    {
        // Act

        Action action =
            () =>
                VendorDeliveryResult.Failure(
                    " ");

        // Assert

        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(
                "errorMessage");
    }

    // ==========================================================
    // Equality
    // ==========================================================

    /// <summary>
    /// Verifies that two vendor delivery results
    /// having identical values are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange

        VendorDeliveryResult left =
            VendorDeliveryResult.Success(
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered");

        VendorDeliveryResult right =
            VendorDeliveryResult.Success(
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered");

        // Act

        bool result =
            left.Equals(right);

        // Assert

        result.Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that two vendor delivery results
    /// having different message identifiers are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValuesAreDifferent()
    {
        // Arrange

        VendorDeliveryResult left =
            VendorDeliveryResult.Success(
                messageId: "MSG-001");

        VendorDeliveryResult right =
            VendorDeliveryResult.Success(
                messageId: "MSG-002");

        // Act

        bool result =
            left.Equals(right);

        // Assert

        result.Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that success and failure results
    /// with otherwise identical values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_IsSuccessIsDifferent()
    {
        // Arrange

        VendorDeliveryResult success =
            VendorDeliveryResult.Success(
                messageId: "MSG-001");

        VendorDeliveryResult failure =
            VendorDeliveryResult.Failure(
                errorMessage: "Delivery failed.");

        // Act

        bool result =
            success.Equals(failure);

        // Assert

        result.Should()
            .BeFalse();
    }

    // ==========================================================
    // Hash Code
    // ==========================================================

    /// <summary>
    /// Verifies that equal vendor delivery results
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValuesAreEqual()
    {
        // Arrange

        VendorDeliveryResult left =
            VendorDeliveryResult.Success(
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered");

        VendorDeliveryResult right =
            VendorDeliveryResult.Success(
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered");

        // Act

        int leftHashCode =
            left.GetHashCode();

        int rightHashCode =
            right.GetHashCode();

        // Assert

        leftHashCode
            .Should()
            .Be(rightHashCode);
    }

    // ==========================================================
    // Equality Operators
    // ==========================================================

    /// <summary>
    /// Verifies that the equality operator returns
    /// true for equal values.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange

        VendorDeliveryResult left =
            VendorDeliveryResult.Success(
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered");

        VendorDeliveryResult right =
            VendorDeliveryResult.Success(
                messageId: "MSG-001",
                providerReference: "REF-001",
                status: "Delivered");

        // Act

        bool result =
            left == right;

        // Assert

        result.Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// true for different values.
    /// </summary>
    [Fact]
    public void InequalityOperator_Should_ReturnTrue_When_ValuesAreDifferent()
    {
        // Arrange

        VendorDeliveryResult left =
            VendorDeliveryResult.Success(
                messageId: "MSG-001");

        VendorDeliveryResult right =
            VendorDeliveryResult.Success(
                messageId: "MSG-002");

        // Act

        bool result =
            left != right;

        // Assert

        result.Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that equality operators correctly
    /// handle null operands.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_HandleNullOperands()
    {
        // Arrange

        VendorDeliveryResult? left =
            null;

        VendorDeliveryResult? right =
            null;

        VendorDeliveryResult value =
            VendorDeliveryResult.Success(
                messageId: "MSG-001");

        // Act & Assert

        (left == right)
            .Should()
            .BeTrue();

        (left != right)
            .Should()
            .BeFalse();

        (left == value)
            .Should()
            .BeFalse();

        (left != value)
            .Should()
            .BeTrue();

        (value == right)
            .Should()
            .BeFalse();

        (value != right)
            .Should()
            .BeTrue();
    }
}