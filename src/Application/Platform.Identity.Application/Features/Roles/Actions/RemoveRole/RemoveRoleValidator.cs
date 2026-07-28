// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Features/Roles/Actions/RemoveRole/RemoveRoleValidator.cs
//
// STEP-7B
// LOCKED
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Application.Features.Roles.Actions;

/// <summary>
/// Validates RemoveRoleCommand.
/// </summary>
public sealed class RemoveRoleValidator
    : ICommandValidator<RemoveRoleCommand>
{
    /// <inheritdoc />
    public ValidationResult Validate(
        RemoveRoleCommand command)
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

        if (command.RoleId == Guid.Empty)
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.ROLE_ID_REQUIRED",
                    "Role identifier is required."));
        }

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors);
    }
}