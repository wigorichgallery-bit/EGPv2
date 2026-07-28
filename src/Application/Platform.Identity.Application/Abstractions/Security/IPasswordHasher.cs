// ===========================================
// File Location : src/Application/Platform.Identity.Application/Abstractions/Security/IPasswordHasher.cs
// ===========================================
namespace Platform.Identity.Application.Abstractions.Security;

/// <summary>
/// Provides password hashing and verification services.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Creates a secure password hash.
    /// </summary>
    /// <param name="plainTextPassword">
    /// Plain text password.
    /// </param>
    /// <returns>
    /// Secure password hash.
    /// </returns>
    string Hash(string plainTextPassword);

    /// <summary>
    /// Verifies a password against an existing hash.
    /// </summary>
    /// <param name="plainTextPassword">
    /// Plain text password.
    /// </param>a
    /// <param name="passwordHash">
    /// Existing password hash.
    /// </param>
    /// <returns>
    /// True when password is valid.
    /// </returns>
    bool Verify(
        string plainTextPassword,
        string passwordHash);
}