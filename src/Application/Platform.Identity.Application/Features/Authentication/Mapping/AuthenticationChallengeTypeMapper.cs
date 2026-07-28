// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Features/Authentication/Mapping/
// AuthenticationChallengeTypeMapper.cs
// ===========================================

using ContractChallengeType =
    Platform.Identity.Application.Contracts.Authentication.Enums.AuthenticationChallengeType;

using DomainChallengeType =
    Platform.Identity.Domain.Enums.AuthenticationChallengeType;

namespace Platform.Identity.Application.Features.Authentication.Mapping;

/// <summary>
/// Converts authentication challenge types from the identity
/// domain model into application contract values.
///
/// <para>
/// Responsibility:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Convert domain authentication challenge types into
/// application contract authentication challenge types.
/// </description>
/// </item>
/// <item>
/// <description>
/// Preserve the boundary between the Domain layer and the
/// Application contract layer.
/// </description>
/// </item>
/// <item>
/// <description>
/// Prevent direct enum casting between architectural layers.
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
/// May depend on Domain and Application Contracts.
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
/// <item>
/// <description>
/// Must not perform authentication logic.
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
/// Explicit mapping is intentionally preferred over direct enum
/// casting to preserve bounded-context isolation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Adding a new domain challenge type requires this mapper to
/// be updated explicitly, making unsupported values visible
/// during development.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Thread Safety:
/// This type is stateless and thread-safe.
/// </para>
/// </summary>
internal static class AuthenticationChallengeTypeMapper
{
    /// <summary>
    /// Converts a domain authentication challenge type into
    /// its corresponding application contract value.
    /// </summary>
    /// <param name="value">
    /// The domain authentication challenge type.
    /// </param>
    /// <returns>
    /// The corresponding application contract authentication
    /// challenge type.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified domain authentication
    /// challenge type is not supported.
    /// </exception>
    public static ContractChallengeType ToContract(
        DomainChallengeType value)
    {
        return value switch
        {
            DomainChallengeType.None =>
                ContractChallengeType.None,

            DomainChallengeType.Totp =>
                ContractChallengeType.Totp,

            DomainChallengeType.EmailOtp =>
                ContractChallengeType.EmailOtp,

            DomainChallengeType.SmsOtp =>
                ContractChallengeType.SmsOtp,

            DomainChallengeType.WhatsAppOtp =>
                ContractChallengeType.WhatsAppOtp,

            DomainChallengeType.Passkey =>
                ContractChallengeType.Passkey,

            DomainChallengeType.RecoveryCode =>
                ContractChallengeType.RecoveryCode,

            DomainChallengeType.MagicLink =>
                ContractChallengeType.MagicLink,

            DomainChallengeType.Custom =>
                ContractChallengeType.Custom,

            _ => throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Unsupported authentication challenge type.")
        };
    }
}