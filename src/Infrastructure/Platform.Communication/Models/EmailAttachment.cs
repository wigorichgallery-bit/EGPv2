using System;

namespace Platform.Communication.Models;

/// <summary>
/// Represents an email attachment.
/// </summary>
public sealed record EmailAttachment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailAttachment"/> class.
    /// </summary>
    /// <param name="fileName">
    /// Attachment file name.
    /// </param>
    /// <param name="content">
    /// Attachment content.
    /// </param>
    /// <param name="contentType">
    /// MIME content type.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the file name or content type is empty, or when the content is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="content"/> is <see langword="null"/>.
    /// </exception>
    public EmailAttachment(
        string fileName,
        byte[] content,
        string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(content);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);

        if (content.Length == 0)
        {
            throw new ArgumentException(
                "Attachment content cannot be empty.",
                nameof(content));
        }

        FileName = fileName;
        Content = content;
        ContentType = contentType;
    }

    /// <summary>
    /// Gets the attachment file name.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Gets the attachment content.
    /// </summary>
    public byte[] Content { get; }

    /// <summary>
    /// Gets the MIME content type.
    /// </summary>
    public string ContentType { get; }
}