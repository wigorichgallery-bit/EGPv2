// ===========================================
// File Location:
// src/Infrastructure/
// Platform.Security.Infrastructure/
// Authentication/
// Provisioning/
// TotpProvisioningService.cs
// ===========================================

using Microsoft.Extensions.Options;
using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Configuration.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Security.Infrastructure.Authentication.Provisioning;

/// <summary>
/// Provides provisioning information for Time-based
/// One-Time Password (TOTP) authenticators.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Build a standards-compliant otpauth provisioning URI.
/// </description>
/// </item>
/// <item>
/// <description>
/// Return the shared secret for manual entry.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This implementation intentionally does not generate QR
/// code images. QR rendering is considered a presentation
/// concern.
/// </para>
/// </summary>
public sealed class TotpProvisioningService
    : ITotpProvisioningService
{
    private readonly TotpOptions _options;

    public TotpProvisioningService(
        IOptions<TotpOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options.Value;
    }

    /// <inheritdoc />
    public Task<TotpProvisioningResult> ProvisionAsync(
        AuthenticationChallengeDeliveryRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        string issuer =
            Uri.EscapeDataString(_options.Issuer);

        string account =
            Uri.EscapeDataString(
                request.User.Email.Value);

        string secret =
            request.PlainTextSecret;

        string provisioningUri =
            $"otpauth://totp/{issuer}:{account}" +
            $"?secret={secret}" +
            $"&issuer={issuer}" +
            $"&algorithm=SHA1" +
            $"&digits={_options.Digits}" +
            $"&period={_options.TimeStepSeconds}";

        TotpProvisioningResult result =
            new(
                ProvisioningUri: provisioningUri,
                ManualEntryKey: secret);

        return Task.FromResult(result);
    }
}