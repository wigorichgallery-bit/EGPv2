using System.Text.RegularExpressions;

namespace Platform.SharedKernel.Validation;

/// <summary>
/// Provides structural validation for email addresses.
///
/// Responsibility:
/// - Validate email address syntax.
/// - Stateless.
/// - Reusable across bounded contexts.
///
/// This validator does not perform:
/// - DNS lookup
/// - MX lookup
/// - SMTP validation
/// - Mailbox existence verification
/// </summary>
public static partial class EmailAddressValidator
{
    /// <summary>
    /// Determines whether the specified email address is structurally valid.
    /// </summary>
    /// <param name="value">
    /// Email address.
    /// </param>
    /// <returns>
    /// <c>true</c> when the email address is structurally valid;
    /// otherwise <c>false</c>.
    /// </returns>
    public static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return EmailRegex().IsMatch(value);
    }

    [GeneratedRegex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();
}