using Platform.Communication.Models;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.TestData;

/// <summary>
/// Provides reusable test data for
/// <see cref="WhatsAppMessage"/>.
/// </summary>
internal static class WhatsAppMessageTestData
{
    /// <summary>
    /// Creates a valid WhatsApp message
    /// with a single recipient.
    /// </summary>
    /// <returns>
    /// A valid <see cref="WhatsAppMessage"/>.
    /// </returns>
    public static WhatsAppMessage CreateValid()
    {
        return new WhatsAppMessage(
        [
            new WhatsAppNumber("+628123456789")
        ],
        "Hello World");
    }

    /// <summary>
    /// Creates a valid WhatsApp message
    /// with multiple recipients.
    /// </summary>
    /// <returns>
    /// A valid <see cref="WhatsAppMessage"/>.
    /// </returns>
    public static WhatsAppMessage CreateMultipleRecipients()
    {
        return new WhatsAppMessage(
        [
            new WhatsAppNumber("+628123456789"),
            new WhatsAppNumber("+628987654321")
        ],
        "Hello World");
    }

    /// <summary>
    /// Creates a valid WhatsApp message
    /// using the specified message text.
    /// </summary>
    /// <param name="message">
    /// WhatsApp message content.
    /// </param>
    /// <returns>
    /// A valid <see cref="WhatsAppMessage"/>.
    /// </returns>
    public static WhatsAppMessage Create(
        string message)
    {
        return new WhatsAppMessage(
        [
            new WhatsAppNumber("+628123456789")
        ],
        message);
    }
}