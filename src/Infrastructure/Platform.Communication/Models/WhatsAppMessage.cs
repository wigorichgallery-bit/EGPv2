using System;
using System.Collections.Generic;

using Platform.Communication.ValueObjects;

namespace Platform.Communication.Models;

/// <summary>
/// Represents a WhatsApp message to be sent.
/// </summary>
public sealed record WhatsAppMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WhatsAppMessage"/> class.
    /// </summary>
    /// <param name="to">
    /// WhatsApp recipients.
    /// </param>
    /// <param name="message">
    /// WhatsApp message content.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="to"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when recipients are empty or the message is invalid.
    /// </exception>
    public WhatsAppMessage(
        IReadOnlyCollection<WhatsAppNumber> to,
        string message)
    {
        ArgumentNullException.ThrowIfNull(to);

        if (to.Count == 0)
        {
            throw new ArgumentException(
                "At least one recipient is required.",
                nameof(to));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        To = to;
        Message = message;
    }

    /// <summary>
    /// Gets the WhatsApp recipients.
    /// </summary>
    public IReadOnlyCollection<WhatsAppNumber> To { get; }

    /// <summary>
    /// Gets the WhatsApp message content.
    /// </summary>
    public string Message { get; }
}