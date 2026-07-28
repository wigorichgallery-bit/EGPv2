// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Features/Users/Actions/VerifyPhone/VerifyPhoneValidator.cs
//
// STEP-7B
// LOCKED
// ===========================================

using Platform.Identity.Application.Features.Common;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Validates VerifyPhoneCommand.
/// </summary>
public sealed class VerifyPhoneValidator
    : ICommandValidator<VerifyPhoneCommand>
{
    /// <inheritdoc />
    public ValidationResult Validate(
        VerifyPhoneCommand command)
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

        if (string.IsNullOrWhiteSpace(command.VerificationCode))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.VERIFICATION_CODE_REQUIRED",
                    "Verification code is required."));
        }
        else if (command.VerificationCode.Length >
                 ValidationConstants.VerificationCodeMaxLength)
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.VERIFICATION_CODE_TOO_LONG",
                    $"Verification code must not exceed {ValidationConstants.VerificationCodeMaxLength} characters."));
        }

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors);
    }
}