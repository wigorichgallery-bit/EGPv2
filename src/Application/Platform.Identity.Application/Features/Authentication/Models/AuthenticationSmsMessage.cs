// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Models/
// AuthenticationSmsMessage.cs
// ===========================================

namespace Platform.Identity.Application.Features.Authentication.Models;

/// <summary>
/// Represents a formatted SMS message for an
/// authentication challenge.
///
/// <para>
/// This model is an internal application contract between
/// authentication formatters and authentication SMS
/// senders.
/// </para>
/// </summary>
/// <param name="Recipient">
/// Recipient phone number.
/// </param>
/// <param name="Body">
/// SMS message body.
/// </param>
public sealed record AuthenticationSmsMessage(
    string Recipient,
    string Body);