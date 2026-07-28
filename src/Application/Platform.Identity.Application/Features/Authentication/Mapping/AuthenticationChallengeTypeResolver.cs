// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/Authentication/
// Mapping/
// AuthenticationChallengeTypeResolver.cs
// ===========================================

using Platform.Identity.Domain.Enums;

namespace Platform.Identity.Application.Features.Authentication.Mapping;

/// <summary>
/// Resolves domain authentication challenge types from
/// configured multi-factor authentication methods.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Convert <see cref="MFAMethod"/> values into their
/// corresponding <see cref="AuthenticationChallengeType"/>.
/// </description>
/// </item>
/// <item>
/// <description>
/// Centralize MFA-to-challenge type mapping used by
/// application authentication workflows.
/// </description>
/// </item>
/// <item>
/// <description>
/// Eliminate duplicated mapping logic across application
/// use cases.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Architectural Rules:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Belongs to the Application layer.
/// </description>
/// </item>
/// <item>
/// <description>
/// May depend only on Domain enumerations.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not contain business rules.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not access repositories or infrastructure services.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Design Notes:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Explicit mapping is intentionally preferred over direct
/// enum casting to preserve architectural independence and
/// make unsupported values fail fast.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Thread Safety:
/// This type is stateless and thread-safe.
/// </para>
/// </summary>
internal static class AuthenticationChallengeTypeResolver
{
    /// <summary>
    /// Resolves the authentication challenge type associated
    /// with the specified multi-factor authentication method.
    /// </summary>
    /// <param name="method">
    /// The configured multi-factor authentication method.
    /// </param>
    /// <returns>
    /// The corresponding authentication challenge type.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified MFA method is not supported.
    /// </exception>
    public static AuthenticationChallengeType Resolve(
        MFAMethod method)
    {
        return method switch
        {
            MFAMethod.TOTP =>
                AuthenticationChallengeType.Totp,

            MFAMethod.Email =>
                AuthenticationChallengeType.EmailOtp,

            MFAMethod.SMS =>
                AuthenticationChallengeType.SmsOtp,

            MFAMethod.WhatsApp =>
                AuthenticationChallengeType.WhatsAppOtp,

            _ => throw new ArgumentOutOfRangeException(
                nameof(method),
                method,
                "Unsupported multi-factor authentication method.")
        };
    }
}