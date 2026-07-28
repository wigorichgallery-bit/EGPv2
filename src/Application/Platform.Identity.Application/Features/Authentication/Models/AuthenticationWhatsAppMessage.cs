// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Models/
// AuthenticationWhatsAppMessage.cs
// ===========================================

namespace Platform.Identity.Application.Features.Authentication.Models;

/// <summary>
/// Represents a formatted WhatsApp message for an
/// authentication challenge.
///
/// <para>
/// This model is an internal application contract between
/// authentication formatters and authentication WhatsApp
/// senders.
/// </para>
/// </summary>
/// <param name="Recipient">
/// Recipient phone number.
/// </param>
/// <param name="Body">
/// WhatsApp message body.
/// </param>
public sealed record AuthenticationWhatsAppMessage(
    string Recipient,
    string Body);