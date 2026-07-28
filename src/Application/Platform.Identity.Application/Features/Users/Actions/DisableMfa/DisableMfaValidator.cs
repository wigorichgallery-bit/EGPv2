// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/DisableMfa/DisableMfaValidator.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Validates DisableMfaCommand.
/// </summary>
public sealed class DisableMfaValidator
    : ICommandValidator<DisableMfaCommand>
{
    /// <inheritdoc />
    public ValidationResult Validate(
        DisableMfaCommand command)
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

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors);
    }
}