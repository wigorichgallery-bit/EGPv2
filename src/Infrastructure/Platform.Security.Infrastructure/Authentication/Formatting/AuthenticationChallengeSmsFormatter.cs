// ===========================================
// File Location:
// src/Infrastructure/
// Platform.Security.Infrastructure/
// Authentication/
// Formatting/
// AuthenticationChallengeSmsFormatter.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Configuration;
using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Security.Infrastructure.Authentication.Formatting;

/// <summary>
/// Formats authentication challenge SMS messages.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Build the default SMS message.
/// </description>
/// </item>
/// <item>
/// <description>
/// Produce immutable
/// <see cref="AuthenticationSmsMessage"/> instances.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This formatter provides the default authentication
/// SMS template and does not perform message delivery.
/// </para>
/// </summary>
public sealed class AuthenticationChallengeSmsFormatter
    : IAuthenticationChallengeSmsFormatter
{
    private readonly AuthenticationMessageOptions _options;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeSmsFormatter"/>
    /// class.
    /// </summary>
    /// <param name="options">
    /// Authentication messaging configuration.
    /// </param>
    public AuthenticationChallengeSmsFormatter(
        AuthenticationMessageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    /// <inheritdoc />
    public AuthenticationSmsMessage Format(
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
            $"{_options.VerificationCodeSmsPrefix} " +
            $"{request.PlainTextSecret}. " +
            $"Expires in {expirationMinutes} minute(s).";

        return new AuthenticationSmsMessage(
            request.User.PhoneNumber.Value,
            body);
    }
}