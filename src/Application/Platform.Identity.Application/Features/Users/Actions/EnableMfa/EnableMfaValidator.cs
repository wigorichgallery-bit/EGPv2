// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/EnableMfa/EnableMfaValidator.cs
// ===========================================
using Platform.Identity.Domain.Enums;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Validates EnableMfaCommand.
/// </summary>
public sealed class EnableMfaValidator
    : ICommandValidator<EnableMfaCommand>
{
    /// <inheritdoc />
    public ValidationResult Validate(
        EnableMfaCommand command)
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

        if (!Enum.IsDefined(command.Method))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.INVALID_MFA_METHOD",
                    "Invalid MFA method."));
        }

        if (command.Method == MFAMethod.None)
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.INVALID_MFA_METHOD",
                    "MFAMethod.None cannot be enabled."));
        }

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors);
    }
}