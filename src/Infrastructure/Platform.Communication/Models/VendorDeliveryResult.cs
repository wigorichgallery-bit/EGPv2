namespace Platform.Communication.Models;

/// <summary>
/// Represents the result returned by a communication vendor after
/// attempting to deliver a message.
///
/// <para>
/// This model is vendor-agnostic and serves as the communication contract
/// between the Client layer and the Provider layer.
/// </para>
/// </summary>
public sealed record VendorDeliveryResult
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="VendorDeliveryResult"/> class.
    /// </summary>
    /// <param name="messageId">
    /// The unique identifier assigned by the communication provider.
    /// </param>
    /// <param name="providerReference">
    /// An optional provider-specific reference identifier.
    /// </param>
    /// <param name="status">
    /// An optional provider delivery status.
    /// </param>
    /// <param name="rawResponse">
    /// An optional raw response object returned by the provider.
    /// This value is intended only for diagnostics and should not be
    /// consumed by business logic.
    /// </param>
    public VendorDeliveryResult(
        string? messageId,
        string? providerReference = null,
        string? status = null,
        object? rawResponse = null)
    {
        MessageId = messageId;
        ProviderReference = providerReference;
        Status = status;
        RawResponse = rawResponse;
    }

    /// <summary>
    /// Gets the provider-generated message identifier.
    /// </summary>
    public string? MessageId { get; }

    /// <summary>
    /// Gets an optional provider-specific reference identifier.
    /// </summary>
    public string? ProviderReference { get; }

    /// <summary>
    /// Gets the provider delivery status.
    /// </summary>
    public string? Status { get; }

    /// <summary>
    /// Gets the original provider response for diagnostics.
    /// </summary>
    /// <remarks>
    /// Consumers should avoid depending on this value in business logic.
    /// It exists only for troubleshooting and logging scenarios.
    /// </remarks>
    public object? RawResponse { get; }

    /// <summary>
    /// Creates a successful vendor delivery result.
    /// </summary>
    /// <param name="messageId">
    /// The provider-generated message identifier.
    /// </param>
    /// <param name="providerReference">
    /// An optional provider reference.
    /// </param>
    /// <param name="status">
    /// An optional delivery status.
    /// </param>
    /// <param name="rawResponse">
    /// An optional raw provider response.
    /// </param>
    /// <returns>
    /// A successful <see cref="VendorDeliveryResult"/>.
    /// </returns>
    public static VendorDeliveryResult Success(
        string? messageId,
        string? providerReference = null,
        string? status = null,
        object? rawResponse = null)
    {
        return new VendorDeliveryResult(
            messageId,
            providerReference,
            status,
            rawResponse);
    }
}