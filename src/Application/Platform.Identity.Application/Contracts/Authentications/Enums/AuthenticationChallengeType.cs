// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Contracts/Authentication/Enums/
// AuthenticationChallengeType.cs
// ===========================================

namespace Platform.Identity.Application.Contracts.Authentication.Enums;

/// <summary>
/// Represents the authentication challenge mechanism required
/// to complete an authentication workflow.
///
/// <para>
/// This enumeration is part of the public application contract
/// and is returned by authentication responses whenever an
/// additional verification step is required.
/// </para>
///
/// <para>
/// Although this enumeration currently mirrors the domain
/// authentication capabilities, it is intentionally maintained
/// as a separate application contract to preserve clean
/// architecture boundaries.
/// </para>
///
/// <para>
/// The numeric values defined by this enumeration are part of
/// the public contract and must remain stable to preserve
/// backward compatibility.
/// </para>
/// </summary>
public enum AuthenticationChallengeType
{
    /// <summary>
    /// Indicates that no additional authentication challenge
    /// is required.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates that a Time-based One-Time Password (TOTP)
    /// challenge is required.
    /// </summary>
    Totp = 1,

    /// <summary>
    /// Indicates that an email-based one-time password (OTP)
    /// challenge is required.
    /// </summary>
    EmailOtp = 2,

    /// <summary>
    /// Indicates that an SMS-based one-time password (OTP)
    /// challenge is required.
    /// </summary>
    SmsOtp = 3,

    /// <summary>
    /// Indicates that a WhatsApp-based one-time password (OTP)
    /// challenge is required.
    /// </summary>
    WhatsAppOtp = 4,

    /// <summary>
    /// Indicates that a passkey (WebAuthn/FIDO2)
    /// authentication challenge is required.
    /// </summary>
    Passkey = 5,

    /// <summary>
    /// Indicates that a recovery code challenge
    /// is required.
    /// </summary>
    RecoveryCode = 6,

    /// <summary>
    /// Indicates that a magic link authentication
    /// challenge is required.
    /// </summary>
    MagicLink = 7,

    /// <summary>
    /// Indicates that a custom authentication
    /// challenge is required.
    ///
    /// <para>
    /// This value allows applications to interoperate with
    /// proprietary or external authentication providers that
    /// expose challenge mechanisms outside the standard set.
    /// </para>
    /// </summary>
    Custom = 8
}