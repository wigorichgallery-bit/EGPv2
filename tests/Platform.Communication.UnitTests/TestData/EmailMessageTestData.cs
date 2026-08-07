using Platform.Communication.Models;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.TestData;

/// <summary>
/// Provides reusable test data for
/// <see cref="EmailMessage"/>.
/// </summary>
internal static class EmailMessageTestData
{
    /// <summary>
    /// Creates a valid email message.
    /// </summary>
    /// <returns>
    /// A valid <see cref="EmailMessage"/>.
    /// </returns>
    public static EmailMessage CreateValid()
    {
        return new EmailMessage(
        [
            new EmailAddress("user@example.com")
        ],
        "Subject",
        "Body");
    }

    /// <summary>
    /// Creates a valid HTML email message.
    /// </summary>
    /// <returns>
    /// A valid HTML <see cref="EmailMessage"/>.
    /// </returns>
    public static EmailMessage CreateHtml()
    {
        return new EmailMessage(
        [
            new EmailAddress("user@example.com")
        ],
        "Subject",
        "<h1>Hello</h1>",
        isHtml: true);
    }

    /// <summary>
    /// Creates a valid email message
    /// with CC recipients.
    /// </summary>
    /// <returns>
    /// A valid <see cref="EmailMessage"/>.
    /// </returns>
    public static EmailMessage CreateWithCc()
    {
        return new EmailMessage(
        [
            new EmailAddress("user@example.com")
        ],
        "Subject",
        "Body",
        cc:
        [
            new EmailAddress("cc@example.com")
        ]);
    }

    /// <summary>
    /// Creates a valid email message
    /// with BCC recipients.
    /// </summary>
    /// <returns>
    /// A valid <see cref="EmailMessage"/>.
    /// </returns>
    public static EmailMessage CreateWithBcc()
    {
        return new EmailMessage(
        [
            new EmailAddress("user@example.com")
        ],
        "Subject",
        "Body",
        bcc:
        [
            new EmailAddress("bcc@example.com")
        ]);
    }

    /// <summary>
    /// Creates a valid email message
    /// with an attachment.
    /// </summary>
    /// <returns>
    /// A valid <see cref="EmailMessage"/>.
    /// </returns>
    public static EmailMessage CreateWithAttachment()
    {
        return new EmailMessage(
        [
            new EmailAddress("user@example.com")
        ],
        "Subject",
        "Body",
        attachments:
        [
            new EmailAttachment(
                "document.txt",
                [1, 2, 3],
                "text/plain")
        ]);
    }

    /// <summary>
    /// Creates a fully populated email message.
    /// </summary>
    /// <returns>
    /// A complete <see cref="EmailMessage"/>.
    /// </returns>
    public static EmailMessage CreateComplete()
    {
        return new EmailMessage(
        [
            new EmailAddress("user@example.com")
        ],
        "Subject",
        "<h1>Hello</h1>",
        isHtml: true,
        cc:
        [
            new EmailAddress("cc@example.com")
        ],
        bcc:
        [
            new EmailAddress("bcc@example.com")
        ],
        attachments:
        [
            new EmailAttachment(
                "document.txt",
                [1, 2, 3],
                "text/plain")
        ]);
    }

    public static EmailMessage CreateMultipleRecipients()
    {
        return new EmailMessage(
        [
            new EmailAddress("user1@example.com"),
        new EmailAddress("user2@example.com"),
        new EmailAddress("user3@example.com")
        ],
        "Subject",
        "Body");
    }
}