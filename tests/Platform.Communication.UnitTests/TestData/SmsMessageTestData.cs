using Platform.Communication.Models;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.TestData;

/// <summary>
/// Provides reusable test data for
/// <see cref="SmsMessage"/>.
/// </summary>
internal static class SmsMessageTestData
{
    /// <summary>
    /// Creates a valid SMS message with a single recipient.
    /// </summary>
    /// <returns>
    /// A valid <see cref="SmsMessage"/>.
    /// </returns>
    public static SmsMessage CreateValid()
    {
        return new SmsMessage(
        [
            new PhoneNumber("+628123456789")
        ],
        "Hello World");
    }

    /// <summary>
    /// Creates a valid SMS message with multiple recipients.
    /// </summary>
    /// <returns>
    /// A valid <see cref="SmsMessage"/>.
    /// </returns>
    public static SmsMessage CreateMultipleRecipients()
    {
        return new SmsMessage(
        [
            new PhoneNumber("+628123456789"),
            new PhoneNumber("+628987654321")
        ],
        "Hello World");
    }

    /// <summary>
    /// Creates a valid SMS message with custom content.
    /// </summary>
    /// <param name="message">
    /// SMS content.
    /// </param>
    /// <returns>
    /// A valid <see cref="SmsMessage"/>.
    /// </returns>
    public static SmsMessage Create(
        string message)
    {
        return new SmsMessage(
        [
            new PhoneNumber("+628123456789")
        ],
        message);
    }
}