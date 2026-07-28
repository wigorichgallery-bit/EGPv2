// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/Authentication/Actions/Login/
// LoginValidator.cs
//
// STEP-7B
// LOCKED
// ===========================================

using Platform.Identity.Application.Features.Common;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Application.Features.Authentication.Actions;

/// <summary>
/// Validates LoginCommand.
///
/// RESPONSIBILITY:
/// - Validate command shape.
/// - Validate required fields.
/// - Validate identity length.
/// - Validate password length.
///
/// BUSINESS RULES:
/// - Input validation only.
/// - No repository access.
/// - No database access.
/// - No identity resolution.
/// - No password verification.
/// - No token generation.
///
/// COMPLEXITY:
/// - O(1)
/// </summary>
public sealed class LoginValidator
    : ICommandValidator<LoginCommand>
{
    /// <summary>
    /// Validates LoginCommand.
    /// </summary>
    /// <param name="command">
    /// Command to validate.
    /// </param>
    /// <returns>
    /// Validation result.
    /// </returns>
    public ValidationResult Validate(
        LoginCommand command)
    {
        Guard.AgainstNull(
            command,
            nameof(command));

        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(command.Identity))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.IDENTITY_REQUIRED",
                    "Identity is required."));
        }
        else if (command.Identity.Length >
                 ValidationConstants.MaximumIdentityLength)
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.IDENTITY_TOO_LONG",
                    $"Identity must not exceed {ValidationConstants.MaximumIdentityLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.PASSWORD_REQUIRED",
                    "Password is required."));
        }
        else
        {
            if (command.Password.Length <
                ValidationConstants.PasswordMinLength)
            {
                errors.Add(
                    new ValidationError(
                        "IDENTITY.PASSWORD_TOO_SHORT",
                        $"Password must be at least {ValidationConstants.PasswordMinLength} characters."));
            }

            if (command.Password.Length >
                ValidationConstants.MaximumPasswordLength)
            {
                errors.Add(
                    new ValidationError(
                        "IDENTITY.PASSWORD_TOO_LONG",
                        $"Password must not exceed {ValidationConstants.MaximumPasswordLength} characters."));
            }
        }

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors);
    }
}