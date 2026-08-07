using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Identity.Application.UnitTests.Fixtures.Builders;

/// <summary>
/// Builds valid <see cref="UserAccount"/> instances for unit tests.
/// </summary>
public sealed class UserAccountBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _username = "john.doe";
    private EmailAddress _email = new("john.doe@example.com");
    private PhoneNumber _phoneNumber = new("+6281234567890");
    private string _passwordHash = "PASSWORD_HASH";
    private DateTime _createdAt = DateTime.UtcNow;

    /// <summary>
    /// Creates a builder with default values.
    /// </summary>
    public static UserAccountBuilder Default => new();

    /// <summary>
    /// Sets the identifier.
    /// </summary>
    public UserAccountBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    /// <summary>
    /// Sets the username.
    /// </summary>
    public UserAccountBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    /// <summary>
    /// Sets the email.
    /// </summary>
    public UserAccountBuilder WithEmail(string email)
    {
        _email = new EmailAddress(email);
        return this;
    }

    /// <summary>
    /// Sets the phone number.
    /// </summary>
    public UserAccountBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = new PhoneNumber(phoneNumber);
        return this;
    }

    /// <summary>
    /// Sets the password hash.
    /// </summary>
    public UserAccountBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    /// <summary>
    /// Sets the creation timestamp.
    /// </summary>
    public UserAccountBuilder WithCreatedAt(DateTime createdAtUtc)
    {
        _createdAt = createdAtUtc;
        return this;
    }

    /// <summary>
    /// Builds a valid <see cref="UserAccount"/>.
    /// </summary>
    public UserAccount Build()
    {
        return new UserAccount(
            _id,
            _username,
            _email,
            _phoneNumber,
            _passwordHash,
            _createdAt);
    }
    
}