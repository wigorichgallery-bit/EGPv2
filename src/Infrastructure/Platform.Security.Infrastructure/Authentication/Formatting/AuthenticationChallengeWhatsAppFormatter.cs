// ===========================================
// File Location:
// src/Infrastructure/
// Platform.Security.Infrastructure/
// Authentication/
// Formatting/
// AuthenticationChallengeWhatsAppFormatter.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Configuration.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Security.Infrastructure.Authentication.Formatting;

/// <summary>
/// Formats authentication challenge WhatsApp messages.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Build the default WhatsApp message.
/// </description>
/// </item>
/// <item>
/// <description>
/// Produce immutable
/// <see cref="AuthenticationWhatsAppMessage"/> instances.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This formatter provides the default authentication
/// WhatsApp template and does not perform message delivery.
/// </para>
/// </summary>
public sealed class AuthenticationChallengeWhatsAppFormatter
    : IAuthenticationChallengeWhatsAppFormatter
{
    private readonly AuthenticationMessageOptions _options;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeWhatsAppFormatter"/>
    /// class.
    /// </summary>
    /// <param name="options">
    /// Authentication messaging configuration.
    /// </param>
    public AuthenticationChallengeWhatsAppFormatter(
        AuthenticationMessageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    /// <inheritdoc />
    public AuthenticationWhatsAppMessage Format(
        AuthenticationChallengeDeliveryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        TimeSpan remaining =
            request.Challenge.ExpiresAtUtc -
            request.Challenge.CreatedAtUtc;

        int expirationMinutes =
            Math.Max(
                1,
                (int)Math.Ceiling(
                    remaining.TotalMinutes));

        string body =
            $"{_options.VerificationCodeWhatsAppPrefix} " +
            $"{request.PlainTextSecret}\n\n" +
            $"This code expires in {expirationMinutes} minute(s).\n\n" +
            $"{_options.IgnoreMessage}";

        return new AuthenticationWhatsAppMessage(
            request.User.PhoneNumber.Value,
            body);
    }
}