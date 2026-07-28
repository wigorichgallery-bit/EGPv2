// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Features/Roles/Actions/AssignRole/AssignRoleValidator.cs
//
// STEP-7B
// LOCKED
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Application.Features.Roles.Actions;

/// <summary>
/// Validates AssignRoleCommand.
/// </summary>
public sealed class AssignRoleValidator
    : ICommandValidator<AssignRoleCommand>
{
    /// <inheritdoc />
    public ValidationResult Validate(
        AssignRoleCommand command)
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