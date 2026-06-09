// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Enums/MFAMethod.cs
// ===========================================

namespace Platform.Identity.Domain.ValueObjects;

/// <summary>
/// Represents supported multi-factor authentication methods.
/// 
/// Supported Methods:
/// - None
/// - TOTP (RFC 6238)
/// - Email OTP
/// - SMS OTP
/// - WhatsApp OTP
/// 
/// Invariants:
/// - Validation enforced in aggregate.
/// </summary>
public enum MFAMethod
{
    /// <summary>
    /// MFA disabled.
    /// </summary>
    None = 0,

    /// <summary>
    /// Time-based One-Time Password.
    /// </summary>
    TOTP = 1,

    /// <summary>
    /// Email One-Time Password.
    /// </summary>
    Email = 2,

    /// <summary>
    /// SMS One-Time Password.
    /// </summary>
    SMS = 3,

    /// <summary>
    /// WhatsApp One-Time Password.
    /// </summary>
    WhatsApp = 4
}