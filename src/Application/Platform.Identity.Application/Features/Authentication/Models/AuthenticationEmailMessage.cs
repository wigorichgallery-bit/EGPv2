// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Models/
// AuthenticationEmailMessage.cs
// ===========================================

namespace Platform.Identity.Application.Features.Authentication.Models;

/// <summary>
/// Represents a formatted email message for an
/// authentication challenge.
///
/// <para>
/// This model is an internal application contract between
/// authentication formatters and authentication email
/// senders.
/// </para>
/// </summary>
/// <param name="Recipient">
/// Recipient email address.
/// </param>
/// <param name="Subject">
/// Email subject.
/// </param>
/// <param name="Body">
/// Email body.
/// </param>
/// <param name="IsHtml">
/// Indicates whether the body contains HTML.
/// </param>
public sealed record AuthenticationEmailMessage(
    string Recipient,
    string Subject,
    string Body,
    bool IsHtml);