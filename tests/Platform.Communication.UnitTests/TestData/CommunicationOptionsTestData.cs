using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Channels.Sms.Configuration;
using Platform.Communication.Channels.WhatsApp.Configuration;
using Platform.Communication.Enums;
using Platform.Communication.Options;

namespace Platform.Communication.UnitTests.TestData;

/// <summary>
/// Provides reusable communication options
/// for unit testing.
/// </summary>
internal static class CommunicationOptionsTestData
{
    /// <summary>
    /// Creates valid communication options
    /// configured for SMTP.
    /// </summary>
    public static CommunicationOptions CreateSmtp()
    {
        return new CommunicationOptions
        {
            Email = new EmailOptions
            {
                Provider = EmailProviderType.Smtp,
                Configuration = new EmailConfiguration
                {
                    Smtp = new SmtpConfiguration
                    {
                        Host = "smtp.example.com",
                        Port = 587,
                        Username = "user",
                        Password = "password",
                        EnableSsl = true,
                        SenderAddress = "sender@example.com",
                        SenderName = "Sender"
                    }
                }
            }
        };
    }

    /// <summary>
    /// Creates valid communication options
    /// configured for SendGrid.
    /// </summary>
    public static CommunicationOptions CreateSendGrid()
    {
        return new CommunicationOptions
        {
            Email = new EmailOptions
            {
                Provider = EmailProviderType.SendGrid,
                Configuration = new EmailConfiguration
                {
                    SendGrid = new SendGridConfiguration
                    {
                        ApiKey = "api-key",
                        SenderAddress = "sender@example.com",
                        SenderName = "Sender"
                    }
                }
            }
        };
    }

    /// <summary>
    /// Creates valid communication options
    /// configured for Microsoft Graph.
    /// </summary>
    public static CommunicationOptions CreateMicrosoftGraph()
    {
        return new CommunicationOptions
        {
            Email = new EmailOptions
            {
                Provider = EmailProviderType.MicrosoftGraph,
                Configuration = new EmailConfiguration
                {
                    MicrosoftGraph =
                        new MicrosoftGraphConfiguration
                        {
                            TenantId = "tenant-id",
                            ClientId = "client-id",
                            ClientSecret = "client-secret",
                            UserId = "user@example.com"
                        }
                }
            }
        };
    }

    /// <summary>
    /// Creates valid communication options
    /// configured for Twilio SMS.
    /// </summary>
    public static CommunicationOptions CreateTwilioSms()
    {
        return new CommunicationOptions
        {
            Sms = new SmsOptions
            {
                Provider = SmsProviderType.Twilio,
                Twilio = new TwilioSmsConfiguration
                {
                    AccountSid = "account-sid",
                    AuthToken = "auth-token",
                    FromNumber = "+1234567890"
                }
            }
        };
    }

    /// <summary>
    /// Creates valid communication options
    /// configured for Vonage SMS.
    /// </summary>
    public static CommunicationOptions CreateVonageSms()
    {
        return new CommunicationOptions
        {
            Sms = new SmsOptions
            {
                Provider = SmsProviderType.Vonage,
                Vonage = new VonageSmsConfiguration
                {
                    ApiKey = "api-key",
                    ApiSecret = "api-secret",
                    From = "Platform"
                }
            }
        };
    }

    /// <summary>
    /// Creates valid communication options
    /// configured for Meta Cloud WhatsApp.
    /// </summary>
    public static CommunicationOptions CreateMetaCloud()
    {
        return new CommunicationOptions
        {
            WhatsApp = new WhatsAppOptions
            {
                Provider = WhatsAppProviderType.MetaCloud,
                MetaCloud = new MetaCloudConfiguration
                {
                    AccessToken = "access-token",
                    PhoneNumberId = "phone-number-id",
                    BusinessAccountId = "business-account-id"
                }
            }
        };
    }

    /// <summary>
    /// Creates valid communication options
    /// configured for Twilio WhatsApp.
    /// </summary>
    public static CommunicationOptions CreateTwilioWhatsApp()
    {
        return new CommunicationOptions
        {
            WhatsApp = new WhatsAppOptions
            {
                Provider = WhatsAppProviderType.Twilio,
                Twilio = new TwilioWhatsAppConfiguration
                {
                    AccountSid = "account-sid",
                    AuthToken = "auth-token",
                    FromNumber = "+1234567890"
                }
            }
        };
    }
}