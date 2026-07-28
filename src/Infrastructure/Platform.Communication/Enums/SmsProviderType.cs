namespace Platform.Communication.Enums;

/// <summary>
/// Defines the supported SMS providers.
/// </summary>
public enum SmsProviderType
{
    /// <summary>
    /// Twilio SMS provider.
    /// </summary>
    Twilio = 0,

    /// <summary>
    /// Vonage (formerly Nexmo) SMS provider.
    /// </summary>
    Vonage = 1,

}