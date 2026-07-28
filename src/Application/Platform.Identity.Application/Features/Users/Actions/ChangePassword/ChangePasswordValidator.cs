// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Features/Users/Actions/ChangePassword/ChangePasswordValidator.cs
//
// STEP-7B
// LOCKED
// ===========================================

using Platform.Identity.Application.Features.Common;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Validates ChangePasswordCommand.
/// </summary>
public sealed class ChangePasswordValidator
    : ICommandValidator<ChangePasswordCommand>
{
    /// <inheritdoc />
    public ValidationResult Validate(
        ChangePasswordCommand command)
    {
        Guard.AgainstNull(
            command,
            nameof(command));

        var errors = new List<ValidationError>();

        if (command.UserId == Guid.Empty)
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.USER_ID_REQUIRED",
                    "User identifier is required."));
        }

        if (string.IsNullOrWhiteSpace(command.CurrentPassword))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.CURRENT_PASSWORD_REQUIRED",
                    "Current password is required."));
        }

        if (string.IsNullOrWhiteSpace(command.NewPassword))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.NEW_PASSWORD_REQUIRED",
                    "New password is required."));
        }
        else if (command.NewPassword.Length <
                 ValidationConstants.PasswordMinLength)
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.NEW_PASSWORD_TOO_SHORT",
                    $"Password must be at least {ValidationConstants.PasswordMinLength} characters."));
        }

        if (!string.IsNullOrWhiteSpace(command.CurrentPassword) &&
            !string.IsNullOrWhiteSpace(command.NewPassword) &&
            string.Equals(
                command.CurrentPassword,
                command.NewPassword,
                StringComparison.Ordinal))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.PASSWORD_MUST_CHANGE",
                    "New password must be different from current password."));
        }

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors);
    }
}