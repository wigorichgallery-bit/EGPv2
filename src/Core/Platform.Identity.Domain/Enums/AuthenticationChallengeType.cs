// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// Enums/AuthenticationChallengeType.cs
// ===========================================

namespace Platform.Identity.Domain.Enums;

/// <summary>
/// Represents the authentication challenge mechanism used by
/// the domain authentication engine.
///
/// <para>
/// This enumeration defines every authentication challenge
/// capability supported by the identity domain.
/// </para>
///
/// <para>
/// The values are used by the domain model to control the
/// authentication workflow independently from external
/// application contracts.
/// </para>
///
/// <para>
/// The numeric values defined by this enumeration are part of
/// the domain contract and must remain stable to preserve
/// persistence compatibility.
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
    /// challenge provided by an external or
    /// proprietary provider is required.
    /// </summary>
    Custom = 8
}