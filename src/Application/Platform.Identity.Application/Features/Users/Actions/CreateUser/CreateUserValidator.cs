// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Features/Users/Actions/CreateUser/CreateUserValidator.cs
//
// STEP-7B
// LOCKED
// ===========================================
using Platform.Identity.Application.Features.Common;
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Validates CreateUserCommand.
///
/// RESPONSIBILITY:
/// - Validate command shape.
/// - Validate required fields.
/// - Validate email format.
/// - Validate phone number format.
/// - Validate password length.
///
/// BUSINESS RULES:
/// - Input validation only.
/// - No repository access.
/// - No database access.
/// - No domain invariant validation.
///
/// COMPLEXITY:
/// - O(1)
/// </summary>
public sealed class CreateUserValidator
    : ICommandValidator<CreateUserCommand>
{
    /// <summary>
    /// Validates CreateUserCommand.
    /// </summary>
    /// <param name="command">
    /// Command to validate.
    /// </param>
    /// <returns>
    /// Validation result.
    /// </returns>
    public ValidationResult Validate(
        CreateUserCommand command)
    {
        Guard.AgainstNull(
            command,
            nameof(command));

        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(command.Username))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.USERNAME_REQUIRED",
                    "Username is required."));
        }
        else
        {
            if (command.Username.Length <
                ValidationConstants.UsernameMinLength)
            {
                errors.Add(
                    new ValidationError(
                        "IDENTITY.USERNAME_TOO_SHORT",
                        $"Username must be at least {ValidationConstants.UsernameMinLength} characters."));
            }

            if (command.Username.Length >
                ValidationConstants.UsernameMaxLength)
            {
                errors.Add(
                    new ValidationError(
                        "IDENTITY.USERNAME_TOO_LONG",
                        $"Username must not exceed {ValidationConstants.UsernameMaxLength} characters."));
            }
        }

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.EMAIL_REQUIRED",
                    "Email is required."));
        }
        else
        {
            try
            {
                _ = new System.Net.Mail.MailAddress(command.Email);
            }
            catch
            {
                errors.Add(
                    new ValidationError(
                        "IDENTITY.INVALID_EMAIL",
                        "Email format is invalid."));
            }
        }

        if (string.IsNullOrWhiteSpace(command.PhoneNumber))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.PHONE_REQUIRED",
                    "Phone number is required."));
        }
        else if (!Regex.IsMatch(
                     command.PhoneNumber,
                     ValidationPatterns.E164Phone))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.INVALID_PHONE_NUMBER",
                    "Phone number must use E.164 format."));
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.PASSWORD_REQUIRED",
                    "Password is required."));
        }
        else if (command.Password.Length <
                 ValidationConstants.PasswordMinLength)
        {
            errors.Add(
                new ValidationError(
                    "IDENTITY.PASSWORD_TOO_SHORT",
                    $"Password must be at least {ValidationConstants.PasswordMinLength} characters."));
        }

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors);
    }
}