namespace Platform.Communication.Models;

/// <summary>
/// Represents the result returned by a communication vendor after
/// attempting to deliver a message.
///
/// <para>
/// This model is vendor-agnostic and serves as the communication
/// contract between the Client layer and the Provider layer.
/// </para>
/// </summary>
public sealed record VendorDeliveryResult
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="VendorDeliveryResult"/> class.
    /// </summary>
    /// <param name="isSuccess">
    /// Indicates whether the vendor operation succeeded.
    /// </param>
    /// <param name="messageId">
    /// The vendor-generated message identifier.
    /// </param>
    /// <param name="providerReference">
    /// An optional provider-specific reference identifier.
    /// </param>
    /// <param name="status">
    /// An optional provider delivery status.
    /// </param>
    /// <param name="errorMessage">
    /// An optional error message when the vendor reports
    /// a delivery failure.
    /// </param>
    /// <param name="rawResponse">
    /// The original vendor response for diagnostics.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the result state is inconsistent.
    /// </exception>
    public VendorDeliveryResult(
        bool isSuccess,
        string? messageId = null,
        string? providerReference = null,
        string? status = null,
        string? errorMessage = null,
        object? rawResponse = null)
    {
        if (isSuccess &&
            !string.IsNullOrWhiteSpace(errorMessage))
        {
            throw new ArgumentException(
                "A successful vendor result cannot contain an error message.",
                nameof(errorMessage));
        }

        if (!isSuccess &&
            string.IsNullOrWhiteSpace(errorMessage))
        {
            throw new ArgumentException(
                "A failed vendor result must contain an error message.",
                nameof(errorMessage));
        }

        IsSuccess = isSuccess;

        MessageId =
            string.IsNullOrWhiteSpace(messageId)
                ? null
                : messageId;

        ProviderReference =
            string.IsNullOrWhiteSpace(providerReference)
                ? null
                : providerReference;

        Status =
            string.IsNullOrWhiteSpace(status)
                ? null
                : status;

        ErrorMessage =
            string.IsNullOrWhiteSpace(errorMessage)
                ? null
                : errorMessage;

        RawResponse = rawResponse;
    }

    /// <summary>
    /// Gets a value indicating whether the vendor
    /// operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the vendor-generated message identifier.
    /// </summary>
    public string? MessageId { get; }

    /// <summary>
    /// Gets the provider-specific reference identifier.
    /// </summary>
    public string? ProviderReference { get; }

    /// <summary>
    /// Gets the provider delivery status.
    /// </summary>
    public string? Status { get; }

    /// <summary>
    /// Gets the vendor error message.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Gets the original vendor response.
    /// </summary>
    /// <remarks>
    /// This value exists only for diagnostics and logging.
    /// Business logic should never depend on it.
    /// </remarks>
    public object? RawResponse { get; }

    /// <summary>
    /// Creates a successful vendor delivery result.
    /// </summary>
    /// <param name="messageId">
    /// The vendor-generated message identifier.
    /// </param>
    /// <param name="providerReference">
    /// The provider reference.
    /// </param>
    /// <param name="status">
    /// The provider delivery status.
    /// </param>
    /// <param name="rawResponse">
    /// The original vendor response.
    /// </param>
    /// <returns>
    /// A successful <see cref="VendorDeliveryResult"/>.
    /// </returns>
    public static VendorDeliveryResult Success(
        string? messageId = null,
        string? providerReference = null,
        string? status = null,
        object? rawResponse = null)
    {
        return new VendorDeliveryResult(
            isSuccess: true,
            messageId: messageId,
            providerReference: providerReference,
            status: status,
            rawResponse: rawResponse);
    }

    /// <summary>
    /// Creates a failed vendor delivery result.
    /// </summary>
    /// <param name="errorMessage">
    /// The vendor error message.
    /// </param>
    /// <param name="providerReference">
    /// The provider reference.
    /// </param>
    /// <param name="status">
    /// The provider delivery status.
    /// </param>
    /// <param name="rawResponse">
    /// The original vendor response.
    /// </param>
    /// <returns>
    /// A failed <see cref="VendorDeliveryResult"/>.
    /// </returns>
    public static VendorDeliveryResult Failure(
        string errorMessage,
        string? providerReference = null,
        string? status = null,
        object? rawResponse = null)
    {
        return new VendorDeliveryResult(
            isSuccess: false,
            providerReference: providerReference,
            status: status,
            errorMessage: errorMessage,
            rawResponse: rawResponse);
    }
}