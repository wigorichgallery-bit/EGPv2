using System;
using System.Collections.Generic;

using Platform.Communication.ValueObjects;

namespace Platform.Communication.Models;

/// <summary>
/// Represents an email message to be sent.
/// </summary>
public sealed record EmailMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailMessage"/> class.
    /// </summary>
    /// <param name="to">
    /// Primary recipients.
    /// </param>
    /// <param name="subject">
    /// Email subject.
    /// </param>
    /// <param name="body">
    /// Email body.
    /// </param>
    /// <param name="isHtml">
    /// Indicates whether the body contains HTML.
    /// </param>
    /// <param name="cc">
    /// Carbon copy recipients.
    /// </param>
    /// <param name="bcc">
    /// Blind carbon copy recipients.
    /// </param>
    /// <param name="attachments">
    /// Email attachments.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="to"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when recipients are empty, or subject/body are invalid.
    /// </exception>
    public EmailMessage(
        IReadOnlyCollection<EmailAddress> to,
        string subject,
        string body,
        bool isHtml = false,
        IReadOnlyCollection<EmailAddress>? cc = null,
        IReadOnlyCollection<EmailAddress>? bcc = null,
        IReadOnlyCollection<EmailAttachment>? attachments = null)
    {
        ArgumentNullException.ThrowIfNull(to);

        if (to.Count == 0)
        {
            throw new ArgumentException(
                "At least one recipient is required.",
                nameof(to));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        ArgumentException.ThrowIfNullOrWhiteSpace(body);

        To = to;
        Subject = subject;
        Body = body;
        IsHtml = isHtml;
        Cc = cc;
        Bcc = bcc;
        Attachments = attachments;
    }

    /// <summary>
    /// Gets the primary recipients.
    /// </summary>
    public IReadOnlyCollection<EmailAddress> To { get; }

    /// <summary>
    /// Gets the email subject.
    /// </summary>
    public string Subject { get; }

    /// <summary>
    /// Gets the email body.
    /// </summary>
    public string Body { get; }

    /// <summary>
    /// Gets a value indicating whether the body contains HTML.
    /// </summary>
    public bool IsHtml { get; }

    /// <summary>
    /// Gets the carbon copy recipients.
    /// </summary>
    public IReadOnlyCollection<EmailAddress>? Cc { get; }

    /// <summary>
    /// Gets the blind carbon copy recipients.
    /// </summary>
    public IReadOnlyCollection<EmailAddress>? Bcc { get; }

    /// <summary>
    /// Gets the email attachments.
    /// </summary>
    public IReadOnlyCollection<EmailAttachment>? Attachments { get; }
}