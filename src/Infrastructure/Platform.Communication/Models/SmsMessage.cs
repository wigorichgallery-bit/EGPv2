using System;
using System.Collections.Generic;

using Platform.Communication.ValueObjects;

namespace Platform.Communication.Models;

/// <summary>
/// Represents an SMS message to be sent.
/// </summary>
public sealed record SmsMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SmsMessage"/> class.
    /// </summary>
    /// <param name="to">
    /// SMS recipients.
    /// </param>
    /// <param name="message">
    /// SMS message content.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="to"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when recipients are empty or the message is invalid.
    /// </exception>
    public SmsMessage(
        IReadOnlyCollection<PhoneNumber> to,
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
    /// Gets the SMS recipients.
    /// </summary>
    public IReadOnlyCollection<PhoneNumber> To { get; }

    /// <summary>
    /// Gets the SMS message content.
    /// </summary>
    public string Message { get; }
}