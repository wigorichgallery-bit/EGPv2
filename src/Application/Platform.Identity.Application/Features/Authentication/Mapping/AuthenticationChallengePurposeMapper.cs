// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Features/Authentication/Mapping/
// AuthenticationChallengePurposeMapper.cs
// ===========================================

using ContractChallengePurpose =
    Platform.Identity.Application.Contracts.Authentication.Enums.AuthenticationChallengePurpose;

using DomainChallengePurpose =
    Platform.Identity.Domain.Enums.AuthenticationChallengePurpose;

namespace Platform.Identity.Application.Features.Authentication.Mapping;

/// <summary>
/// Converts authentication challenge purposes from the identity
/// domain model into application contract values.
///
/// <para>
/// Responsibility:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Convert domain authentication challenge purposes into
/// application contract authentication challenge purposes.
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
/// Adding a new domain challenge purpose requires this mapper
/// to be updated explicitly, making unsupported values visible
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
internal static class AuthenticationChallengePurposeMapper
{
    /// <summary>
    /// Converts a domain authentication challenge purpose into
    /// its corresponding application contract value.
    /// </summary>
    /// <param name="value">
    /// The domain authentication challenge purpose.
    /// </param>
    /// <returns>
    /// The corresponding application contract authentication
    /// challenge purpose.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified domain authentication
    /// challenge purpose is not supported.
    /// </exception>
    public static ContractChallengePurpose ToContract(
        DomainChallengePurpose value)
    {
        return value switch
        {
            DomainChallengePurpose.Login =>
                ContractChallengePurpose.Login,

            DomainChallengePurpose.PasswordReset =>
                ContractChallengePurpose.PasswordReset,

            DomainChallengePurpose.EmailVerification =>
                ContractChallengePurpose.EmailVerification,

            DomainChallengePurpose.PhoneVerification =>
                ContractChallengePurpose.PhoneVerification,

            DomainChallengePurpose.SensitiveOperation =>
                ContractChallengePurpose.SensitiveOperation,

            DomainChallengePurpose.AccountRecovery =>
                ContractChallengePurpose.AccountRecovery,

            DomainChallengePurpose.Custom =>
                ContractChallengePurpose.Custom,

            _ => throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Unsupported authentication challenge purpose.")
        };
    }
}