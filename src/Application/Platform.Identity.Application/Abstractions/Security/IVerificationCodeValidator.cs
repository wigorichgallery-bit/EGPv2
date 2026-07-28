// ===========================================
// File Location : src/Application/Platform.Identity.Application/Abstractions/Security/IVerificationCodeValidator.cs
// ===========================================
namespace Platform.Identity.Application.Abstractions.Security;
/// <summary>
/// Validates verification codes used by identity workflows.
/// </summary>
public interface IVerificationCodeValidator
{
    /// <summary>
    /// Validates a verification code for a user.
    /// </summary>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="verificationCode">
    /// Verification code supplied by caller.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// True when verification code is valid.
    /// </returns>
    Task<bool> ValidateAsync(
        Guid userId,
        string verificationCode,
        CancellationToken cancellationToken = default);
}