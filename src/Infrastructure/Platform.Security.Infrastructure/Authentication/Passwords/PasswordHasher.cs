// ===========================================
// File Location :
// src/Infrastructure/Platform.Security.Infrastructure/
// Passwords/PasswordHasher.cs
// ===========================================

using System.Security.Cryptography;
using Platform.Identity.Application.Abstractions.Security;

namespace Platform.Security.Infrastructure.Passwords;

/// <summary>
/// Provides PBKDF2 password hashing services.
///
/// Responsibility:
/// - Generate secure password hashes.
/// - Verify password hashes.
/// - Protect credential storage.
/// - Encapsulate hashing implementation.
///
/// Security Model:
/// - PBKDF2-HMACSHA512.
/// - Random salt per password.
/// - Constant-time verification.
/// - Salt stored with hash payload.
///
/// Hash Format:
/// {iterations}.{saltBase64}.{hashBase64}
///
/// Algorithm:
/// Hash:
/// 1. Generate cryptographic salt.
/// 2. Derive key using PBKDF2.
/// 3. Encode salt and hash.
/// 4. Persist composite payload.
///
/// Verify:
/// 1. Parse payload.
/// 2. Recompute PBKDF2 hash.
/// 3. Compare using fixed-time equality.
///
/// Complexity:
/// O(iterations)
///
/// Side Effects:
/// - None.
/// </summary>
public sealed class PasswordHasher
    : IPasswordHasher
{
    /// <summary>
    /// PBKDF2 iteration count.
    /// </summary>
    private const int Iterations = 100_000;

    /// <summary>
    /// Salt size in bytes.
    /// </summary>
    private const int SaltSize = 32;

    /// <summary>
    /// Hash size in bytes.
    /// </summary>
    private const int HashSize = 64;

    /// <inheritdoc />
    public string Hash(
        string plainTextPassword)
    {
        ArgumentNullException.ThrowIfNull(
            plainTextPassword);

        var salt =
            RandomNumberGenerator.GetBytes(
                SaltSize);

        var hash =
            Rfc2898DeriveBytes.Pbkdf2(
                password: plainTextPassword,
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: HashSize);

        return string.Concat(
            Iterations,
            ".",
            Convert.ToBase64String(salt),
            ".",
            Convert.ToBase64String(hash));
    }

    /// <inheritdoc />
    public bool Verify(
        string plainTextPassword,
        string passwordHash)
    {
        ArgumentNullException.ThrowIfNull(
            plainTextPassword);

        ArgumentNullException.ThrowIfNull(
            passwordHash);

        var parts =
            passwordHash.Split(
                '.',
                StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
        {
            return false;
        }

        if (!int.TryParse(
                parts[0],
                out var iterations))
        {
            return false;
        }

        byte[] salt;
        byte[] expectedHash;

        try
        {
            salt =
                Convert.FromBase64String(
                    parts[1]);

            expectedHash =
                Convert.FromBase64String(
                    parts[2]);
        }
        catch (FormatException)
        {
            return false;
        }

        var actualHash =
            Rfc2898DeriveBytes.Pbkdf2(
                password: plainTextPassword,
                salt: salt,
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: expectedHash.Length);

        return CryptographicOperations
            .FixedTimeEquals(
                actualHash,
                expectedHash);
    }
}