// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Models/
// TotpProvisioningResult.cs
// ===========================================

namespace Platform.Identity.Application.Features.Authentication.Models;

/// <summary>
/// Represents the result of provisioning a Time-based
/// One-Time Password (TOTP) authenticator.
///
/// <para>
/// Encapsulates all information required by a client to
/// enroll an authenticator application.
/// </para>
/// </summary>
/// <param name="ProvisioningUri">
/// The RFC 6238 provisioning URI (otpauth://).
/// </param>
/// <param name="ManualEntryKey">
/// The shared secret presented for manual entry.
/// </param>
public sealed record TotpProvisioningResult(
    string ProvisioningUri,
    string ManualEntryKey);