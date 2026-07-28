// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Services/
// AuthenticationChallengeDeliveryService.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Enums;

namespace Platform.Identity.Application.Features.Authentication.Services;

/// <summary>
/// Delivers authentication challenges through the
/// appropriate authentication delivery channel.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Route authentication challenges to the appropriate
/// delivery channel based on the authentication challenge
/// type.
/// </description>
/// </item>
/// <item>
/// <description>
/// Coordinate authentication-specific delivery services
/// without implementing transport-specific behavior.
/// </description>
/// </item>
/// <item>
/// <description>
/// Provide a single application service responsible for
/// authentication challenge delivery orchestration.
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
/// Must not generate authentication secrets.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not modify domain aggregates.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not persist application or domain data.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not implement transport-specific delivery logic.
/// </description>
/// </item>
/// </list>
/// </summary>
public sealed class AuthenticationChallengeDeliveryService
    : IAuthenticationChallengeDeliveryService
{
    private readonly IEmailAuthenticationChallengeSender
        _emailSender;

    private readonly ISmsAuthenticationChallengeSender
        _smsSender;

    private readonly IWhatsAppAuthenticationChallengeSender
        _whatsAppSender;

    private readonly ITotpProvisioningService
        _totpProvisioningService;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeDeliveryService"/>
    /// class.
    /// </summary>
    /// <param name="emailSender">
    /// Email authentication challenge sender.
    /// </param>
    /// <param name="smsSender">
    /// SMS authentication challenge sender.
    /// </param>
    /// <param name="whatsAppSender">
    /// WhatsApp authentication challenge sender.
    /// </param>
    /// <param name="totpProvisioningService">
    /// TOTP provisioning service.
    /// </param>
    public AuthenticationChallengeDeliveryService(
        IEmailAuthenticationChallengeSender emailSender,
        ISmsAuthenticationChallengeSender smsSender,
        IWhatsAppAuthenticationChallengeSender whatsAppSender,
        ITotpProvisioningService totpProvisioningService)
    {
        ArgumentNullException.ThrowIfNull(
            emailSender);

        ArgumentNullException.ThrowIfNull(
            smsSender);

        ArgumentNullException.ThrowIfNull(
            whatsAppSender);

        ArgumentNullException.ThrowIfNull(
            totpProvisioningService);

        _emailSender =
            emailSender;

        _smsSender =
            smsSender;

        _whatsAppSender =
            whatsAppSender;

        _totpProvisioningService =
            totpProvisioningService;
    }

    /// <inheritdoc />
    public async Task DeliverAsync(
        AuthenticationChallengeDeliveryRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            request);

        switch (request.Challenge.ChallengeType)
        {
            case AuthenticationChallengeType.EmailOtp:

                await _emailSender.SendAsync(
                    request,
                    cancellationToken);

                break;

            case AuthenticationChallengeType.SmsOtp:

                await _smsSender.SendAsync(
                    request,
                    cancellationToken);

                break;

            case AuthenticationChallengeType.WhatsAppOtp:

                await _whatsAppSender.SendAsync(
                    request,
                    cancellationToken);

                break;

            case AuthenticationChallengeType.Totp:

                await _totpProvisioningService
                    .ProvisionAsync(
                        request,
                        cancellationToken);

                break;

            default:

                throw new ArgumentOutOfRangeException(
                    nameof(request.Challenge.ChallengeType),
                    request.Challenge.ChallengeType,
                    "Unsupported authentication challenge type.");
        }
    }
}