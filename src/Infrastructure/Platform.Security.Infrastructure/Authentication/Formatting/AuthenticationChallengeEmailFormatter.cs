// ===========================================
// File Location:
// src/Infrastructure/
// Platform.Security.Infrastructure/
// Authentication/
// Formatting/
// AuthenticationChallengeEmailFormatter.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Configuration.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Security.Infrastructure.Authentication.Formatting;

/// <summary>
/// Formats authentication challenge email messages.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Build the default email subject.
/// </description>
/// </item>
/// <item>
/// <description>
/// Build the default email body.
/// </description>
/// </item>
/// <item>
/// <description>
/// Produce immutable
/// <see cref="AuthenticationEmailMessage"/> instances.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This formatter provides the default authentication
/// email template and does not perform message delivery.
/// </para>
/// </summary>
public sealed class AuthenticationChallengeEmailFormatter
    : IAuthenticationChallengeEmailFormatter
{
    private readonly AuthenticationMessageOptions _options;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeEmailFormatter"/>
    /// class.
    /// </summary>
    /// <param name="options">
    /// Authentication messaging configuration.
    /// </param>
    public AuthenticationChallengeEmailFormatter(
        AuthenticationMessageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    /// <inheritdoc />
    public AuthenticationEmailMessage Format(
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

        string subject =
            _options.VerificationCodeEmailSubject;

        string body =
$"""
Your verification code for {_options.ApplicationName} is:

{request.PlainTextSecret}

This verification code will expire in {expirationMinutes} minute(s).

{_options.IgnoreMessage}
""";

        return new AuthenticationEmailMessage(
            request.User.Email.Value,
            subject,
            body,
            IsHtml: false);
    }
}