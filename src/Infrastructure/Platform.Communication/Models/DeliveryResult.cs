using System;

namespace Platform.Communication.Models;

/// <summary>
/// Represents the result of a communication delivery operation.
/// </summary>
public sealed record DeliveryResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeliveryResult"/> class.
    /// </summary>
    /// <param name="succeeded">
    /// Indicates whether the delivery operation succeeded.
    /// </param>
    /// <param name="providerMessageId">
    /// Provider-specific message identifier, if available.
    /// </param>
    /// <param name="errorMessage">
    /// Error description when the delivery operation fails.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the delivery result is inconsistent.
    /// </exception>
    public DeliveryResult(
        bool succeeded,
        string? providerMessageId = null,
        string? errorMessage = null)
    {
        if (succeeded &&
            !string.IsNullOrWhiteSpace(errorMessage))
        {
            throw new ArgumentException(
                "A successful delivery result cannot contain an error message.",
                nameof(errorMessage));
        }

        if (!succeeded &&
            string.IsNullOrWhiteSpace(errorMessage))
        {
            throw new ArgumentException(
                "A failed delivery result must contain an error message.",
                nameof(errorMessage));
        }

        Succeeded = succeeded;
        ProviderMessageId = string.IsNullOrWhiteSpace(providerMessageId)
            ? null
            : providerMessageId;
        ErrorMessage = string.IsNullOrWhiteSpace(errorMessage)
            ? null
            : errorMessage;
    }

    /// <summary>
    /// Gets a value indicating whether the delivery operation succeeded.
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// Gets the provider-specific message identifier, if available.
    /// </summary>
    public string? ProviderMessageId { get; }

    /// <summary>
    /// Gets the error description when the delivery operation fails.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Creates a successful delivery result.
    /// </summary>
    /// <param name="providerMessageId">
    /// Provider-specific message identifier, if available.
    /// </param>
    /// <returns>
    /// A successful <see cref="DeliveryResult"/>.
    /// </returns>
    public static DeliveryResult Success(
        string? providerMessageId = null)
    {
        return new DeliveryResult(
            succeeded: true,
            providerMessageId: providerMessageId);
    }

    /// <summary>
    /// Creates a failed delivery result.
    /// </summary>
    /// <param name="errorMessage">
    /// Error description.
    /// </param>
    /// <param name="providerMessageId">
    /// Provider-specific message identifier, if available.
    /// </param>
    /// <returns>
    /// A failed <see cref="DeliveryResult"/>.
    /// </returns>
    public static DeliveryResult Failure(
        string errorMessage,
        string? providerMessageId = null)
    {
        return new DeliveryResult(
            succeeded: false,
            providerMessageId: providerMessageId,
            errorMessage: errorMessage);
    }
}