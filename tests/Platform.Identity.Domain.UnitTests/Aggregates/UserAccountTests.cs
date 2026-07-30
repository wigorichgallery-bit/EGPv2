// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Aggregates/UserAccountTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.Events;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Aggregates;

/// <summary>
/// Contains unit tests for
/// <see cref="UserAccount"/>.
/// </summary>
public sealed partial class UserAccountTests
{
    #region Constructor Tests

    /// <summary>
    /// Verifies that the constructor
    /// initializes every property correctly.
    /// </summary>
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var id = Guid.NewGuid();

        var email =
            new EmailAddress("user@example.com");

        var phone =
            new PhoneNumber("+628123456789");

        var createdAt = DateTime.UtcNow;

        // Act
        var account = new UserAccount(
            id,
            "john",
            email,
            phone,
            "HASH",
            createdAt);

        // Assert
        account.Id.Should().Be(id);
        account.Username.Should().Be("john");
        account.Email.Should().Be(email);
        account.PhoneNumber.Should().Be(phone);
        account.PasswordHash.Should().Be("HASH");

        account.PasswordVersion.Should().Be(1);

        account.SecurityStamp.Should().NotBeNullOrWhiteSpace();

        account.LastPasswordChangedAt
            .Should()
            .Be(createdAt);

        account.EmailVerified.Should().BeFalse();
        account.PhoneVerified.Should().BeFalse();

        account.MFAEnabled.Should().BeFalse();
        account.MFAMethod.Should().Be(MFAMethod.None);

        account.FailedLoginCount.Should().Be(0);
        account.LockoutUntil.Should().BeNull();

        account.Status.Should().Be(UserStatus.Active);

        account.CreatedAt.Should().Be(createdAt);
        account.UpdatedAt.Should().Be(createdAt);

        account.RoleAssignments.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that the constructor
    /// emits a <see cref="UserCreatedDomainEvent"/>.
    /// </summary>
    [Fact]
    public void Constructor_ShouldRaiseUserCreatedDomainEvent()
    {
        // Arrange
        var id = Guid.NewGuid();

        var email =
            new EmailAddress("user@example.com");

        var phone =
            new PhoneNumber("+628123456789");

        var createdAt = DateTime.UtcNow;

        // Act
        var account = new UserAccount(
            id,
            "john",
            email,
            phone,
            "HASH",
            createdAt);

        // Assert
        account.DomainEvents
            .Should()
            .ContainSingle();

        account.DomainEvents
            .Single()
            .Should()
            .BeOfType<UserCreatedDomainEvent>();

        var domainEvent =
            (UserCreatedDomainEvent)
            account.DomainEvents.Single();

        domainEvent.AggregateId.Should().Be(id);
        domainEvent.OccurredOn.Should().Be(createdAt);
        domainEvent.Username.Should().Be("john");
        domainEvent.Email.Should().Be(email.Value);
    }

    /// <summary>
    /// Verifies that a null username
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenUsernameIsNull()
    {
        // Arrange

        // Act
        var action = () =>
            new UserAccount(
                Guid.NewGuid(),
                null!,
                new EmailAddress("user@example.com"),
                new PhoneNumber("+628123456789"),
                "HASH",
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an empty username
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenUsernameIsEmpty()
    {
        // Arrange

        // Act
        var action = () =>
            new UserAccount(
                Guid.NewGuid(),
                string.Empty,
                new EmailAddress("user@example.com"),
                new PhoneNumber("+628123456789"),
                "HASH",
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a whitespace username
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenUsernameIsWhitespace()
    {
        // Arrange

        // Act
        var action = () =>
            new UserAccount(
                Guid.NewGuid(),
                "   ",
                new EmailAddress("user@example.com"),
                new PhoneNumber("+628123456789"),
                "HASH",
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a null email
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenEmailIsNull()
    {
        // Arrange

        // Act
        var action = () =>
            new UserAccount(
                Guid.NewGuid(),
                "john",
                null!,
                new PhoneNumber("+628123456789"),
                "HASH",
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that a null phone number
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenPhoneNumberIsNull()
    {
        // Arrange

        // Act
        var action = () =>
            new UserAccount(
                Guid.NewGuid(),
                "john",
                new EmailAddress("user@example.com"),
                null!,
                "HASH",
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that a null password hash
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenPasswordHashIsNull()
    {
        // Arrange

        // Act
        var action = () =>
            new UserAccount(
                Guid.NewGuid(),
                "john",
                new EmailAddress("user@example.com"),
                new PhoneNumber("+628123456789"),
                null!,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a non-UTC creation
    /// timestamp is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenCreatedAtIsNotUtc()
    {
        // Arrange
        var localTime =
            DateTime.Now;

        // Act
        var action = () =>
            new UserAccount(
                Guid.NewGuid(),
                "john",
                new EmailAddress("user@example.com"),
                new PhoneNumber("+628123456789"),
                "HASH",
                localTime);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region ChangePassword Tests

    /// <summary>
    /// Verifies that changing the password
    /// updates all related state.
    /// </summary>
    [Fact]
    public void ChangePassword_ShouldUpdatePasswordInformation()
    {
        // Arrange
        var createdAt = DateTime.UtcNow.AddMinutes(-10);

        var account = new UserAccount(
            Guid.NewGuid(),
            "john",
            new EmailAddress("user@example.com"),
            new PhoneNumber("+628123456789"),
            "HASH-1",
            createdAt);        

        var previousStamp =
            account.SecurityStamp;

        var nowUtc =
            DateTime.UtcNow;

        // Act
        account.ChangePassword(
            "HASH-2",
            nowUtc);

        // Assert
        account.PasswordHash.Should()
            .Be("HASH-2");

        account.PasswordVersion.Should()
            .Be(2);

        account.SecurityStamp.Should()
            .NotBe(previousStamp);

        account.LastPasswordChangedAt.Should()
            .Be(nowUtc);

        account.UpdatedAt.Should()
            .Be(nowUtc);
    }

    /// <summary>
    /// Verifies that changing the password
    /// raises the expected domain events.
    /// </summary>
    [Fact]
    public void ChangePassword_ShouldRaiseDomainEvents()
    {
        // Arrange
        var account = CreateActiveUser();
        var before = account.DomainEvents.Count;                
        var nowUtc =
            DateTime.UtcNow;

        // Act
        account.ChangePassword(
            "HASH-2",
            nowUtc);

        // Assert
        var events = GetNewDomainEvents(account, before);
        events.Should().HaveCount(2);

        events.Should()
            .ContainSingle(e =>
                e is PasswordChangedDomainEvent);

        events.Should()
            .ContainSingle(e =>
                e is SessionInvalidatedDomainEvent);
    }

    /// <summary>
    /// Verifies that a null password hash
    /// is rejected.
    /// </summary>
    [Fact]
    public void ChangePassword_ShouldThrow_WhenPasswordHashIsNull()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.ChangePassword(
                null!,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an empty password hash
    /// is rejected.
    /// </summary>
    [Fact]
    public void ChangePassword_ShouldThrow_WhenPasswordHashIsEmpty()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.ChangePassword(
                string.Empty,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that password reuse
    /// is rejected.
    /// </summary>
    [Fact]
    public void ChangePassword_ShouldThrow_WhenPasswordIsReused()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.ChangePassword(
                "HASH",
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.PasswordReuse);
    }

    /// <summary>
    /// Verifies that changing the password
    /// requires an active user.
    /// </summary>
    [Fact]
    public void ChangePassword_ShouldThrow_WhenUserIsNotActive()
    {
        // Arrange
        var account = CreateActiveUser();

        account.Disable(
            DateTime.UtcNow);

        // Act
        var action = () =>
            account.ChangePassword(
                "HASH-NEW",
                DateTime.UtcNow.AddMinutes(1));

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void ChangePassword_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.ChangePassword(
                "HASH-NEW",
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region Test Helpers

    /// <summary>
    /// Creates a valid active user account.
    /// </summary>
    private static UserAccount CreateActiveUser()
    {
        return new UserAccount(
            Guid.NewGuid(),
            "john",
            new EmailAddress("user@example.com"),
            new PhoneNumber("+628123456789"),
            "HASH",
            DateTime.UtcNow);
    }

    private static IReadOnlyList<DomainEvent> GetNewDomainEvents(
    UserAccount account,
    int previousCount)
    {
        return account.DomainEvents
            .Skip(previousCount)
            .ToArray();
    }

    #endregion

    #region RegisterFailedLoginAttempt Tests

    /// <summary>
    /// Verifies that a failed login
    /// increments the failed login count.
    /// </summary>
    [Fact]
    public void RegisterFailedLoginAttempt_ShouldIncrementFailedLoginCount()
    {
        // Arrange
        var account = CreateActiveUser();

        var nowUtc = DateTime.UtcNow;

        // Act
        account.RegisterFailedLoginAttempt(
            threshold: 5,
            lockDuration: TimeSpan.FromMinutes(15),
            nowUtc);

        // Assert
        account.FailedLoginCount.Should().Be(1);
        account.Status.Should().Be(UserStatus.Active);
        account.LockoutUntil.Should().BeNull();
        account.UpdatedAt.Should().Be(nowUtc);
    }

    /// <summary>
    /// Verifies that reaching the lockout
    /// threshold locks the user.
    /// </summary>
    [Fact]
    public void RegisterFailedLoginAttempt_ShouldLockUser_WhenThresholdReached()
    {
        // Arrange
        var account = CreateActiveUser();
        var nowUtc = DateTime.UtcNow;
        var duration = TimeSpan.FromMinutes(30);
        var before = account.DomainEvents.Count;
        // Act
        account.RegisterFailedLoginAttempt(
            threshold: 1,
            duration,
            nowUtc);

        // Assert
        account.Status.Should().Be(UserStatus.Locked);

        account.LockoutUntil.Should()
            .Be(nowUtc.Add(duration));

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is UserLockedDomainEvent);
    }

    /// <summary>
    /// Verifies that a disabled user
    /// ignores failed login attempts.
    /// </summary>
    [Fact]
    public void RegisterFailedLoginAttempt_ShouldIgnore_WhenUserIsDisabled()
    {
        // Arrange
        var account = CreateActiveUser();

        account.Disable(DateTime.UtcNow);

        var failedCount =
            account.FailedLoginCount;

        // Act
        account.RegisterFailedLoginAttempt(
            threshold: 1,
            lockDuration: TimeSpan.FromMinutes(5),
            nowUtc: DateTime.UtcNow);

        // Assert
        account.FailedLoginCount.Should()
            .Be(failedCount);

        account.Status.Should()
            .Be(UserStatus.Disabled);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void RegisterFailedLoginAttempt_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.RegisterFailedLoginAttempt(
                threshold: 3,
                lockDuration: TimeSpan.FromMinutes(5),
                nowUtc: DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region ResetFailedLogin Tests

    /// <summary>
    /// Verifies that resetting failed login
    /// clears the lockout information.
    /// </summary>
    [Fact]
    public void ResetFailedLogin_ShouldResetLockoutState()
    {
        // Arrange
        var account = CreateActiveUser();

        account.RegisterFailedLoginAttempt(
            threshold: 1,
            lockDuration: TimeSpan.FromMinutes(10),
            nowUtc: DateTime.UtcNow);

        var resetTime =
            DateTime.UtcNow.AddMinutes(1);

        // Act
        account.ResetFailedLogin(resetTime);

        // Assert
        account.FailedLoginCount.Should().Be(0);
        account.LockoutUntil.Should().BeNull();
        account.UpdatedAt.Should().Be(resetTime);
    }

    /// <summary>
    /// Verifies that resetting failed login
    /// does not change the current user status.
    /// </summary>
    [Fact]
    public void ResetFailedLogin_ShouldNotChangeStatus()
    {
        // Arrange
        var account = CreateActiveUser();

        account.RegisterFailedLoginAttempt(
            threshold: 1,
            lockDuration: TimeSpan.FromMinutes(10),
            nowUtc: DateTime.UtcNow);

        var status =
            account.Status;

        // Act
        account.ResetFailedLogin(
            DateTime.UtcNow.AddMinutes(1));

        // Assert
        account.Status.Should().Be(status);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void ResetFailedLogin_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.ResetFailedLogin(
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region RecordSuccessfulLogin Tests

    /// <summary>
    /// Verifies that a successful login
    /// updates the basic audit information.
    /// </summary>
    [Fact]
    public void RecordSuccessfulLogin_ShouldUpdateAuditInformation()
    {
        // Arrange
        var account = CreateActiveUser();

        account.RegisterFailedLoginAttempt(
            threshold: 5,
            lockDuration: TimeSpan.FromMinutes(15),
            nowUtc: DateTime.UtcNow);

        var nowUtc =
            DateTime.UtcNow.AddMinutes(1);

        // Act
        account.RecordSuccessfulLogin(nowUtc);

        // Assert
        account.LastLoginAt.Should().Be(nowUtc);

        account.FailedLoginCount.Should().Be(0);

        account.LockoutUntil.Should().BeNull();

        account.LastLoginIp.Should().BeNull();

        account.LastLoginCountry.Should().BeNull();

        account.LastDeviceFingerprint.Should().BeNull();

        account.LastLatitude.Should().BeNull();

        account.LastLongitude.Should().BeNull();

        account.UpdatedAt.Should().Be(nowUtc);
    }

    /// <summary>
    /// Verifies that a successful login
    /// stores the supplied client context.
    /// </summary>
    [Fact]
    public void RecordSuccessfulLogin_ShouldStoreClientContext()
    {
        // Arrange
        var account = CreateActiveUser();

        var nowUtc =
            DateTime.UtcNow;

        // Act
        account.RecordSuccessfulLogin(
            nowUtc,
            "192.168.1.100",
            "Indonesia",
            "DEVICE-001",
            -6.2088,
            106.8456);

        // Assert
        account.LastLoginAt.Should().Be(nowUtc);

        account.LastLoginIp.Should()
            .Be("192.168.1.100");

        account.LastLoginCountry.Should()
            .Be("Indonesia");

        account.LastDeviceFingerprint.Should()
            .Be("DEVICE-001");

        account.LastLatitude.Should()
            .Be(-6.2088);

        account.LastLongitude.Should()
            .Be(106.8456);
    }

    /// <summary>
    /// Verifies that empty client context
    /// values are normalized to null.
    /// </summary>
    [Fact]
    public void RecordSuccessfulLogin_ShouldNormalizeEmptyStringsToNull()
    {
        // Arrange
        var account = CreateActiveUser();

        var nowUtc =
            DateTime.UtcNow;

        // Act
        account.RecordSuccessfulLogin(
            nowUtc,
            "",
            "   ",
            "",
            null,
            null);

        // Assert
        account.LastLoginIp.Should().BeNull();

        account.LastLoginCountry.Should().BeNull();

        account.LastDeviceFingerprint.Should().BeNull();

        account.LastLatitude.Should().BeNull();

        account.LastLongitude.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the overload without
    /// coordinates delegates correctly.
    /// </summary>
    [Fact]
    public void RecordSuccessfulLogin_ShouldSupportFourParameterOverload()
    {
        // Arrange
        var account = CreateActiveUser();

        var nowUtc =
            DateTime.UtcNow;

        // Act
        account.RecordSuccessfulLogin(
            nowUtc,
            "10.0.0.1",
            "Singapore",
            "DEVICE-A");

        // Assert
        account.LastLoginIp.Should()
            .Be("10.0.0.1");

        account.LastLoginCountry.Should()
            .Be("Singapore");

        account.LastDeviceFingerprint.Should()
            .Be("DEVICE-A");

        account.LastLatitude.Should().BeNull();

        account.LastLongitude.Should().BeNull();
    }

    /// <summary>
    /// Verifies that successful login
    /// requires an active user.
    /// </summary>
    [Fact]
    public void RecordSuccessfulLogin_ShouldThrow_WhenUserIsNotActive()
    {
        // Arrange
        var account = CreateActiveUser();

        account.Disable(
            DateTime.UtcNow);

        // Act
        var action = () =>
            account.RecordSuccessfulLogin(
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void RecordSuccessfulLogin_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.RecordSuccessfulLogin(
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that recording a successful
    /// login does not emit domain events.
    /// </summary>
    [Fact]
    public void RecordSuccessfulLogin_ShouldNotRaiseDomainEvents()
    {
        // Arrange
        var account = CreateActiveUser();

        var before = account.DomainEvents.Count;

        // Act
        account.RecordSuccessfulLogin(
            DateTime.UtcNow);

        // Assert        
        account.DomainEvents.Count.Should().Be(before);
    }

    #endregion

    #region EnableMFA Tests

    /// <summary>
    /// Verifies that email MFA can be enabled
    /// after the email has been verified.
    /// </summary>
    [Fact]
    public void EnableMFA_ShouldEnableEmailMFA()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);
        
        var previousStamp =
            account.SecurityStamp;

        var nowUtc =
            DateTime.UtcNow.AddMinutes(1);

        var before = account.DomainEvents.Count;

        // Act
        account.EnableMFA(
            MFAMethod.Email,
            nowUtc);

        // Assert
        account.MFAEnabled.Should().BeTrue();

        account.MFAMethod.Should()
            .Be(MFAMethod.Email);

        account.SecurityStamp.Should()
            .NotBe(previousStamp);

        account.UpdatedAt.Should()
            .Be(nowUtc);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is MFAEnabledDomainEvent);

        events.Should()
            .ContainSingle(e =>
                e is SessionInvalidatedDomainEvent);
    }

    /// <summary>
    /// Verifies that enabling TOTP MFA succeeds
    /// when a TOTP secret has been configured.
    /// </summary>
    [Fact]
    public void EnableMFA_ShouldEnableTotpMFA()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        account.SetTotpSecret(
            "ENCRYPTED_SECRET",
            DateTime.UtcNow);        

        // Act
        account.EnableMFA(
            MFAMethod.TOTP,
            DateTime.UtcNow);

        // Assert
        account.MFAEnabled.Should().BeTrue();

        account.MFAMethod.Should()
            .Be(MFAMethod.TOTP);
    }

    /// <summary>
    /// Verifies that MFA cannot be enabled twice.
    /// </summary>
    [Fact]
    public void EnableMFA_ShouldThrow_WhenAlreadyEnabled()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        account.EnableMFA(
            MFAMethod.Email,
            DateTime.UtcNow);

        // Act
        var action = () =>
            account.EnableMFA(
                MFAMethod.Email,
                DateTime.UtcNow.AddMinutes(1));

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that email MFA requires
    /// a verified email.
    /// </summary>
    [Fact]
    public void EnableMFA_ShouldThrow_WhenEmailNotVerified()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.EnableMFA(
                MFAMethod.Email,
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.EmailNotVerified);
    }

    /// <summary>
    /// Verifies that SMS MFA requires
    /// a verified phone number.
    /// </summary>
    [Theory]
    [InlineData(MFAMethod.SMS)]
    [InlineData(MFAMethod.WhatsApp)]
    public void EnableMFA_ShouldThrow_WhenPhoneNotVerified(
        MFAMethod method)
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.EnableMFA(
                method,
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.PhoneNotVerified);
    }

    /// <summary>
    /// Verifies that TOTP MFA requires
    /// a configured TOTP secret.
    /// </summary>
    [Fact]
    public void EnableMFA_ShouldThrow_WhenTotpSecretMissing()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        // Act
        var action = () =>
            account.EnableMFA(
                MFAMethod.TOTP,
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.TotpRequired);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void EnableMFA_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        // Act
        var action = () =>
            account.EnableMFA(
                MFAMethod.Email,
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region DisableMFA Tests

    /// <summary>
    /// Verifies that disabling MFA
    /// resets the MFA state.
    /// </summary>
    [Fact]
    public void DisableMFA_ShouldDisableMFA()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        account.EnableMFA(
            MFAMethod.Email,
            DateTime.UtcNow);        

        var previousStamp =
            account.SecurityStamp;

        var nowUtc =
            DateTime.UtcNow.AddMinutes(1);

        var before = account.DomainEvents.Count;

        // Act
        account.DisableMFA(nowUtc);

        // Assert
        account.MFAEnabled.Should().BeFalse();

        account.MFAMethod.Should()
            .Be(MFAMethod.None);

        account.SecurityStamp.Should()
            .NotBe(previousStamp);

        account.UpdatedAt.Should()
            .Be(nowUtc);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is MFADisabledDomainEvent);

        events.Should()
            .ContainSingle(e =>
                e is SessionInvalidatedDomainEvent);
    }

    /// <summary>
    /// Verifies that disabling MFA
    /// requires MFA to be enabled.
    /// </summary>
    [Fact]
    public void DisableMFA_ShouldThrow_WhenNotEnabled()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.DisableMFA(
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void DisableMFA_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        account.EnableMFA(
            MFAMethod.Email,
            DateTime.UtcNow);

        // Act
        var action = () =>
            account.DisableMFA(
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region AssignRole Tests

    /// <summary>
    /// Verifies that assigning a role
    /// adds a new role assignment.
    /// </summary>
    [Fact]
    public void AssignRole_ShouldAddRoleAssignment()
    {
        // Arrange
        var account = CreateActiveUser();        

        var roleId = Guid.NewGuid();
        var previousStamp = account.SecurityStamp;
        var nowUtc = DateTime.UtcNow;
        var before = account.DomainEvents.Count;

        // Act
        account.AssignRole(roleId, nowUtc);

        // Assert
        account.RoleAssignments.Should()
            .ContainSingle(r => r.RoleId == roleId);

        account.SecurityStamp.Should()
            .NotBe(previousStamp);

        account.UpdatedAt.Should()
            .Be(nowUtc);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is RoleAssignedDomainEvent);

        events.Should()
            .ContainSingle(e =>
                e is SessionInvalidatedDomainEvent);
    }

    /// <summary>
    /// Verifies that assigning the same role twice
    /// is rejected.
    /// </summary>
    [Fact]
    public void AssignRole_ShouldThrow_WhenRoleAlreadyAssigned()
    {
        // Arrange
        var account = CreateActiveUser();

        var roleId = Guid.NewGuid();

        account.AssignRole(
            roleId,
            DateTime.UtcNow);

        // Act
        var action = () =>
            account.AssignRole(
                roleId,
                DateTime.UtcNow.AddMinutes(1));

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.RoleAlreadyAssigned);
    }

    /// <summary>
    /// Verifies that an empty role identifier
    /// is rejected.
    /// </summary>
    [Fact]
    public void AssignRole_ShouldThrow_WhenRoleIdIsEmpty()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.AssignRole(
                Guid.Empty,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void AssignRole_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.AssignRole(
                Guid.NewGuid(),
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region RemoveRole Tests

    /// <summary>
    /// Verifies that removing an assigned role
    /// removes the role assignment.
    /// </summary>
    [Fact]
    public void RemoveRole_ShouldRemoveRoleAssignment()
    {
        // Arrange
        var account = CreateActiveUser();

        var roleId = Guid.NewGuid();

        account.AssignRole(
            roleId,
            DateTime.UtcNow);        

        var previousStamp =
            account.SecurityStamp;

        var nowUtc =
            DateTime.UtcNow.AddMinutes(1);

        var before = account.DomainEvents.Count;

        // Act
        account.RemoveRole(
            roleId,
            nowUtc);

        // Assert
        account.RoleAssignments.Should()
            .BeEmpty();

        account.SecurityStamp.Should()
            .NotBe(previousStamp);

        account.UpdatedAt.Should()
            .Be(nowUtc);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is RoleRemovedDomainEvent);

        events.Should()
            .ContainSingle(e =>
                e is SessionInvalidatedDomainEvent);
    }

    /// <summary>
    /// Verifies that removing a role that is
    /// not assigned is rejected.
    /// </summary>
    [Fact]
    public void RemoveRole_ShouldThrow_WhenRoleNotAssigned()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.RemoveRole(
                Guid.NewGuid(),
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.RoleNotAssigned);
    }

    /// <summary>
    /// Verifies that an empty role identifier
    /// is rejected.
    /// </summary>
    [Fact]
    public void RemoveRole_ShouldThrow_WhenRoleIdIsEmpty()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.RemoveRole(
                Guid.Empty,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void RemoveRole_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        var roleId = Guid.NewGuid();

        account.AssignRole(
            roleId,
            DateTime.UtcNow);

        // Act
        var action = () =>
            account.RemoveRole(
                roleId,
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region Unlock Tests

    /// <summary>
    /// Verifies that a locked user
    /// can be unlocked.
    /// </summary>
    [Fact]
    public void Unlock_ShouldRestoreActiveState()
    {
        // Arrange
        var account = CreateActiveUser();

        var lockTime = DateTime.UtcNow;

        account.RegisterFailedLoginAttempt(
            threshold: 1,
            lockDuration: TimeSpan.FromMinutes(15),
            lockTime);        

        var unlockTime =
            lockTime.AddMinutes(1);

        var before = account.DomainEvents.Count;

        // Act
        account.Unlock(unlockTime);

        // Assert
        account.Status.Should()
            .Be(UserStatus.Active);

        account.FailedLoginCount.Should()
            .Be(0);

        account.LockoutUntil.Should()
            .BeNull();

        account.UpdatedAt.Should()
            .Be(unlockTime);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is UserUnlockedDomainEvent);
    }

    /// <summary>
    /// Verifies that only locked users
    /// can be unlocked.
    /// </summary>
    [Fact]
    public void Unlock_ShouldThrow_WhenUserIsNotLocked()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.Unlock(
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Unlock_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        account.RegisterFailedLoginAttempt(
            threshold: 1,
            lockDuration: TimeSpan.FromMinutes(10),
            nowUtc: DateTime.UtcNow);

        // Act
        var action = () =>
            account.Unlock(
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region Disable Tests

    /// <summary>
    /// Verifies that an active user
    /// can be disabled.
    /// </summary>
    [Fact]
    public void Disable_ShouldDisableUser()
    {
        // Arrange
        var account = CreateActiveUser();        

        var nowUtc =
            DateTime.UtcNow;

        var before = account.DomainEvents.Count;

        // Act
        account.Disable(nowUtc);

        // Assert
        account.Status.Should()
            .Be(UserStatus.Disabled);

        account.UpdatedAt.Should()
            .Be(nowUtc);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is SessionInvalidatedDomainEvent);
    }

    /// <summary>
    /// Verifies that disabling an already
    /// disabled user is rejected.
    /// </summary>
    [Fact]
    public void Disable_ShouldThrow_WhenAlreadyDisabled()
    {
        // Arrange
        var account = CreateActiveUser();

        account.Disable(
            DateTime.UtcNow);

        // Act
        var action = () =>
            account.Disable(
                DateTime.UtcNow.AddMinutes(1));

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Disable_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.Disable(
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region Restore Tests

    /// <summary>
    /// Verifies that a disabled user
    /// can be restored.
    /// </summary>
    [Fact]
    public void Restore_ShouldActivateDisabledUser()
    {
        // Arrange
        var account = CreateActiveUser();

        account.Disable(
            DateTime.UtcNow);

        var nowUtc =
            DateTime.UtcNow.AddMinutes(1);

        var before = account.DomainEvents.Count;

        // Act
        account.Restore(nowUtc);

        // Assert
        account.Status.Should()
            .Be(UserStatus.Active);

        account.UpdatedAt.Should()
            .Be(nowUtc);

        account.DomainEvents.Count.Should()
            .Be(before);
    }

    /// <summary>
    /// Verifies that only disabled users
    /// can be restored.
    /// </summary>
    [Fact]
    public void Restore_ShouldThrow_WhenUserIsNotDisabled()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.Restore(
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Restore_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        account.Disable(
            DateTime.UtcNow);

        // Act
        var action = () =>
            account.Restore(
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region VerifyEmail Tests

    /// <summary>
    /// Verifies that verifying the email
    /// updates the account state.
    /// </summary>
    [Fact]
    public void VerifyEmail_ShouldVerifyEmail()
    {
        // Arrange
        var account = CreateActiveUser();

        var nowUtc = DateTime.UtcNow;

        var before = account.DomainEvents.Count;

        // Act
        account.VerifyEmail(nowUtc);

        // Assert
        account.EmailVerified.Should().BeTrue();

        account.UpdatedAt.Should()
            .Be(nowUtc);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is EmailVerifiedDomainEvent);
    }

    /// <summary>
    /// Verifies that verifying an already
    /// verified email is idempotent.
    /// </summary>
    [Fact]
    public void VerifyEmail_ShouldBeIdempotent()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        var before = account.DomainEvents.Count;

        // Act
        account.VerifyEmail(
            DateTime.UtcNow.AddMinutes(1));

        // Assert
        account.EmailVerified.Should().BeTrue();

        account.DomainEvents.Count.Should()
            .Be(before);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void VerifyEmail_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.VerifyEmail(DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region VerifyPhone Tests

    /// <summary>
    /// Verifies that verifying the phone
    /// updates the account state.
    /// </summary>
    [Fact]
    public void VerifyPhone_ShouldVerifyPhone()
    {
        // Arrange
        var account = CreateActiveUser();        

        var nowUtc = DateTime.UtcNow;

        var before = account.DomainEvents.Count;

        // Act
        account.VerifyPhone(nowUtc);

        // Assert
        account.PhoneVerified.Should().BeTrue();

        account.UpdatedAt.Should()
            .Be(nowUtc);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is PhoneVerifiedDomainEvent);
    }

    /// <summary>
    /// Verifies that verifying an already
    /// verified phone is idempotent.
    /// </summary>
    [Fact]
    public void VerifyPhone_ShouldBeIdempotent()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyPhone(DateTime.UtcNow);

        var before = account.DomainEvents.Count;

        // Act
        account.VerifyPhone(
            DateTime.UtcNow.AddMinutes(1));

        // Assert
        account.PhoneVerified.Should().BeTrue();

        account.DomainEvents.Count.Should()
            .Be(before);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void VerifyPhone_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.VerifyPhone(DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region SetTotpSecret Tests

    /// <summary>
    /// Verifies that a TOTP secret
    /// can be configured after at least one
    /// contact method has been verified.
    /// </summary>
    [Fact]
    public void SetTotpSecret_ShouldStoreSecret()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);        

        var nowUtc = DateTime.UtcNow;

        var before = account.DomainEvents.Count;

        // Act
        account.SetTotpSecret(
            "ENCRYPTED_SECRET",
            nowUtc);

        // Assert
        account.TOTPSecretEncrypted.Should()
            .Be("ENCRYPTED_SECRET");

        account.UpdatedAt.Should()
            .Be(nowUtc);

        var events = GetNewDomainEvents(account,before);

        events.Should()
            .ContainSingle(e =>
                e is TotpSecretSetDomainEvent);
    }

    /// <summary>
    /// Verifies that a verified phone number
    /// is also sufficient to configure TOTP.
    /// </summary>
    [Fact]
    public void SetTotpSecret_ShouldAllowVerifiedPhone()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyPhone(DateTime.UtcNow);

        // Act
        account.SetTotpSecret(
            "ENCRYPTED_SECRET",
            DateTime.UtcNow);

        // Assert
        account.TOTPSecretEncrypted.Should()
            .Be("ENCRYPTED_SECRET");
    }

    /// <summary>
    /// Verifies that configuring TOTP requires
    /// at least one verified contact method.
    /// </summary>
    [Fact]
    public void SetTotpSecret_ShouldThrow_WhenNoContactVerified()
    {
        // Arrange
        var account = CreateActiveUser();

        // Act
        var action = () =>
            account.SetTotpSecret(
                "ENCRYPTED_SECRET",
                DateTime.UtcNow);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.ContactNotVerified);
    }

    /// <summary>
    /// Verifies that a null secret
    /// is rejected.
    /// </summary>
    [Fact]
    public void SetTotpSecret_ShouldThrow_WhenSecretIsNull()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        // Act
        var action = () =>
            account.SetTotpSecret(
                null!,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an empty secret
    /// is rejected.
    /// </summary>
    [Fact]
    public void SetTotpSecret_ShouldThrow_WhenSecretIsEmpty()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        // Act
        var action = () =>
            account.SetTotpSecret(
                string.Empty,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void SetTotpSecret_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var account = CreateActiveUser();

        account.VerifyEmail(DateTime.UtcNow);

        // Act
        var action = () =>
            account.SetTotpSecret(
                "ENCRYPTED_SECRET",
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    
}