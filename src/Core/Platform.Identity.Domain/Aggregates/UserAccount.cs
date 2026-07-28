// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Aggregates/UserAccount.cs
// ===========================================
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ErrorCodes;
using Platform.Identity.Domain.Events;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.Aggregates;

/// <summary>
/// Represents the UserAccount aggregate root.
/// 
/// RESPONSIBILITY:
/// - Maintains user identity lifecycle.
/// - Enforces authentication and security policies.
/// - Controls password, MFA, and lockout behavior.
/// - Manages role assignments.
/// - Emits domain events for security-sensitive operations.
/// 
/// ARCHITECTURAL RULE:
/// - Pure domain logic (no infrastructure dependency).
/// - State changes must go through methods.
/// 
/// EF CORE:
/// - Constructor binding aligned with property names.
/// - Private constructor required for materialization.
/// </summary>
public sealed class UserAccount : AggregateRoot
{
    // ============================================================
    // IDENTITY
    // ============================================================

    /// <summary>
    /// Gets the username.
    /// </summary>
    public string Username { get; private set; } = default!;

    /// <summary>
    /// Gets the email address value object.
    /// </summary>
    public EmailAddress Email { get; private set; } = default!;

    /// <summary>
    /// Gets the phone number value object.
    /// </summary>
    public PhoneNumber PhoneNumber { get; private set; } = default!;

    /// <summary>
    /// Gets whether email has been verified.
    /// </summary>
    public bool EmailVerified { get; private set; }

    /// <summary>
    /// Gets whether phone number has been verified.
    /// </summary>
    public bool PhoneVerified { get; private set; }

    // ============================================================
    // CREDENTIALS
    // ============================================================

    /// <summary>
    /// Gets the password hash.
    /// </summary>
    public string PasswordHash { get; private set; } = default!;

    /// <summary>
    /// Gets password version for rotation tracking.
    /// </summary>
    public int PasswordVersion { get; private set; }

    /// <summary>
    /// Gets the security stamp for session invalidation.
    /// </summary>
    public string SecurityStamp { get; private set; } = default!;

    /// <summary>
    /// Gets last password change timestamp (UTC).
    /// </summary>
    public DateTime LastPasswordChangedAt { get; private set; }

    // ============================================================
    // MFA
    // ============================================================

    /// <summary>
    /// Gets whether multi-factor authentication is enabled.
    /// </summary>
    public bool MFAEnabled { get; private set; }

    /// <summary>
    /// Gets the active MFA method.
    /// </summary>
    public MFAMethod MFAMethod { get; private set; }

    /// <summary>
    /// Gets encrypted TOTP secret (if applicable).
    /// </summary>
    public string? TOTPSecretEncrypted { get; private set; }

    // ============================================================
    // LOCKOUT
    // ============================================================

    /// <summary>
    /// Gets number of failed login attempts.
    /// </summary>
    public int FailedLoginCount { get; private set; }

    /// <summary>
    /// Gets lockout expiration timestamp (UTC).
    /// </summary>
    public DateTime? LockoutUntil { get; private set; }

    /// <summary>
    /// Gets current user status.
    /// </summary>
    public UserStatus Status { get; private set; }

    // ============================================================
    // ROLES
    // ============================================================

    private readonly List<RoleAssignment> _roleAssignments = [];

    /// <summary>
    /// Gets read-only role assignments.
    /// </summary>
    public IReadOnlyCollection<RoleAssignment> RoleAssignments => _roleAssignments.AsReadOnly();

    // ============================================================
    // AUDIT
    // ============================================================

    /// <summary>
    /// Gets creation timestamp (UTC).
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets last update timestamp (UTC).
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets last successful login timestamp (UTC).
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// Gets last login IP address.
    /// </summary>
    public string? LastLoginIp { get; private set; }

    /// <summary>
    /// Gets last login country.
    /// </summary>
    public string? LastLoginCountry { get; private set; }

    /// <summary>
    /// Gets last device fingerprint.
    /// </summary>
    public string? LastDeviceFingerprint { get; private set; }

    // ONLY SHOWING UPDATED PART (SAFE MERGE)

    /// <summary>
    /// Gets last login latitude.
    /// </summary>
    public double? LastLatitude { get; private set; }

    /// <summary>
    /// Gets last login longitude.
    /// </summary>
    public double? LastLongitude { get; private set; }

    // ============================================================
    // EF CONSTRUCTOR
    // ============================================================

    /// <summary>
    /// Initializes a new instance of <see cref="UserAccount"/> for EF Core.
    /// </summary>
    private UserAccount() : base(){}

    // ============================================================
    // DOMAIN CONSTRUCTOR
    // ============================================================

    /// <summary>
    /// Initializes a new instance of <see cref="UserAccount"/>.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="username">Username.</param>
    /// <param name="email">Email value object.</param>
    /// <param name="phoneNumber">Phone number value object.</param>
    /// <param name="passwordHash">Password hash.</param>
    /// <param name="createdAt">Creation timestamp (UTC).</param>
    /// <exception cref="DomainException">Thrown when invariant is violated.</exception>
    public UserAccount(
        Guid id,
        string username,
        EmailAddress email,
        PhoneNumber phoneNumber,
        string passwordHash,
        DateTime createdAt)
        : base(id)
    {
        Guard.AgainstNullOrWhiteSpace(username, nameof(username));
        Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(phoneNumber, nameof(phoneNumber));        

        Guard.AgainstNonUtc(createdAt, nameof(createdAt));

        Username = username;
        Email = email;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;

        PasswordVersion = 1;
        SecurityStamp = Guid.NewGuid().ToString("N");
        LastPasswordChangedAt = createdAt;

        EmailVerified = false;
        PhoneVerified = false;

        MFAEnabled = false;
        MFAMethod = MFAMethod.None;

        FailedLoginCount = 0;
        LockoutUntil = null;
        Status = UserStatus.Active;

        CreatedAt = createdAt;
        UpdatedAt = createdAt;

        AddDomainEvent(new UserCreatedDomainEvent(
            id,
            createdAt,
            username,
            email.Value));
    }

    // ============================================================
    // PASSWORD MANAGEMENT
    // ============================================================

    /// <summary>
    /// Changes the user password.
    /// </summary>
    public void ChangePassword(string newPasswordHash, DateTime nowUtc)
    {        
        Guard.AgainstNullOrWhiteSpace(newPasswordHash, nameof(newPasswordHash));

        if (Status != UserStatus.Active)
            throw new DomainException(IdentityDomainErrorCodes.InvalidState, "User must be active.");

        if (PasswordHash == newPasswordHash)
            throw new DomainException(IdentityDomainErrorCodes.PasswordReuse, "Password reuse not allowed.");

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));
        PasswordHash = newPasswordHash;
        PasswordVersion++;
        SecurityStamp = Guid.NewGuid().ToString("N");
        LastPasswordChangedAt = nowUtc;
        UpdatedAt = nowUtc;

        AddDomainEvent(new PasswordChangedDomainEvent(Id, nowUtc, PasswordVersion));
        AddDomainEvent(new SessionInvalidatedDomainEvent(Id, nowUtc, "Password changed."));
    }

    // ============================================================
    // LOCKOUT
    // ============================================================

    /// <summary>
    /// Registers failed login attempt.
    /// </summary>
    public void RegisterFailedLoginAttempt(int threshold, 
    TimeSpan lockDuration, DateTime nowUtc)
    {
        if (Status == UserStatus.Disabled)
            return;

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        FailedLoginCount++;
        UpdatedAt = nowUtc;

        if (FailedLoginCount >= threshold)
        {
            Status = UserStatus.Locked;
            LockoutUntil = nowUtc.Add(lockDuration);        
            AddDomainEvent(new UserLockedDomainEvent(Id, nowUtc, LockoutUntil.Value));
        }
    }

    /// <summary>
    /// Resets failed login count.
    /// </summary>
    public void ResetFailedLogin(DateTime nowUtc)
    {
        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        FailedLoginCount = 0;
        LockoutUntil = null;
        UpdatedAt = nowUtc;
    }

    // ============================================================
    // LOGIN AUDIT
    // ============================================================

    /// <summary>
    /// Records a successful login.
    ///
    /// <para>
    /// This overload records only the successful authentication
    /// timestamp and resets the failed login state.
    /// </para>
    /// </summary>
    /// <param name="nowUtc">
    /// The current UTC timestamp.
    /// </param>
    /// <exception cref="DomainException">
    /// Thrown when the user account is not active.
    /// </exception>
    public void RecordSuccessfulLogin(
        DateTime nowUtc)
    {
        RecordSuccessfulLogin(
            nowUtc,
            string.Empty,
            string.Empty,
            string.Empty,
            null,
            null);
    }

    /// <summary>
    /// Records a successful login together with client context.
    ///
    /// <para>
    /// This overload records the authentication context including
    /// the client IP address, country and device fingerprint.
    /// </para>
    /// </summary>
    /// <param name="nowUtc">
    /// The current UTC timestamp.
    /// </param>
    /// <param name="ipAddress">
    /// The client IP address.
    /// </param>
    /// <param name="country">
    /// The detected country.
    /// </param>
    /// <param name="deviceFingerprint">
    /// The client device fingerprint.
    /// </param>
    /// <exception cref="DomainException">
    /// Thrown when the user account is not active.
    /// </exception>
    public void RecordSuccessfulLogin(
        DateTime nowUtc,
        string ipAddress,
        string country,
        string deviceFingerprint)
    {
        RecordSuccessfulLogin(
            nowUtc,
            ipAddress,
            country,
            deviceFingerprint,
            null,
            null);
    }

    /// <summary>
    /// Records a successful login together with the complete
    /// authentication context.
    ///
    /// <para>
    /// This method represents the canonical implementation used by
    /// the authentication workflow. It resets the failed login state,
    /// updates the audit information and stores the latest client
    /// context used during authentication.
    /// </para>
    /// </summary>
    /// <param name="nowUtc">
    /// The current UTC timestamp.
    /// </param>
    /// <param name="ipAddress">
    /// The client IP address.
    /// </param>
    /// <param name="country">
    /// The detected country.
    /// </param>
    /// <param name="deviceFingerprint">
    /// The client device fingerprint.
    /// </param>
    /// <param name="latitude">
    /// The detected client latitude, when available.
    /// </param>
    /// <param name="longitude">
    /// The detected client longitude, when available.
    /// </param>
    /// <exception cref="DomainException">
    /// Thrown when the user account is not active.
    /// </exception>
    public void RecordSuccessfulLogin(
        DateTime nowUtc,
        string ipAddress,
        string country,
        string deviceFingerprint,
        double? latitude,
        double? longitude)
    {
        Guard.AgainstNonUtc(
            nowUtc,
            nameof(nowUtc));

        if (Status != UserStatus.Active)
        {
            throw new DomainException(
                IdentityDomainErrorCodes.InvalidState,
                "User must be active.");
        }

        FailedLoginCount = 0;
        LockoutUntil = null;

        LastLoginAt = nowUtc;

        LastLoginIp = string.IsNullOrWhiteSpace(ipAddress)
            ? null
            : ipAddress;

        LastLoginCountry = string.IsNullOrWhiteSpace(country)
            ? null
            : country;

        LastDeviceFingerprint = string.IsNullOrWhiteSpace(deviceFingerprint)
            ? null
            : deviceFingerprint;

        LastLatitude = latitude;
        LastLongitude = longitude;

        UpdatedAt = nowUtc;
    }
    
    // ============================================================
    // MFA
    // ============================================================

    /// <summary>
    /// Enables MFA.
    /// </summary>
    public void EnableMFA(MFAMethod method, DateTime nowUtc)
    {        
        if (MFAEnabled)
            throw new DomainException(IdentityDomainErrorCodes.InvalidState, "MFA is already enabled.");

        if (method == MFAMethod.Email && !EmailVerified)
            throw new DomainException(IdentityDomainErrorCodes.EmailNotVerified, "Email must be verified.");

        if ((method == MFAMethod.SMS || method == MFAMethod.WhatsApp) && !PhoneVerified)
            throw new DomainException(IdentityDomainErrorCodes.PhoneNotVerified, "Phone must be verified.");

        if (method == MFAMethod.TOTP && string.IsNullOrWhiteSpace(TOTPSecretEncrypted))
            throw new DomainException(IdentityDomainErrorCodes.TotpRequired, "TOTP secret required.");

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        MFAEnabled = true;
        MFAMethod = method;
        SecurityStamp = Guid.NewGuid().ToString("N");
        UpdatedAt = nowUtc;

        AddDomainEvent(new MFAEnabledDomainEvent(Id, nowUtc, method));
        AddDomainEvent(new SessionInvalidatedDomainEvent(Id, nowUtc, "MFA enabled."));
    }

    /// <summary>
    /// Disables MFA.
    /// </summary>
    public void DisableMFA(DateTime nowUtc)
    {
        if (!MFAEnabled)
            throw new DomainException(IdentityDomainErrorCodes.InvalidState, "MFA is not enabled.");

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        MFAEnabled = false;
        MFAMethod = MFAMethod.None;
        SecurityStamp = Guid.NewGuid().ToString("N");
        UpdatedAt = nowUtc;

        AddDomainEvent(new MFADisabledDomainEvent(Id, nowUtc));
        AddDomainEvent(new SessionInvalidatedDomainEvent(Id, nowUtc, "MFA disabled."));
    }

    // ============================================================
    // ROLE MANAGEMENT
    // ============================================================

    /// <summary>
    /// Assigns role to user.
    /// </summary>
    /// <param name="roleId">
    /// Role identifier.
    /// </param>
    /// <param name="nowUtc">
    /// Current UTC timestamp.
    /// </param>
    /// <exception cref="DomainException">
    /// Thrown when role is already assigned.
    /// </exception>
    public void AssignRole(Guid roleId, DateTime nowUtc)
        {
            Guard.AgainstEmpty(roleId, nameof(roleId));

            if (_roleAssignments.Any(r => r.RoleId == roleId))
            {
                throw new DomainException(
                    IdentityDomainErrorCodes.RoleAlreadyAssigned,
                    "Role is already assigned.");
            }

            Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

            _roleAssignments.Add(new RoleAssignment(roleId));

            SecurityStamp = Guid.NewGuid().ToString("N");
            UpdatedAt = nowUtc;

            AddDomainEvent(
                new RoleAssignedDomainEvent(
                    Id,
                    nowUtc,
                    roleId));

            AddDomainEvent(
                new SessionInvalidatedDomainEvent(
                    Id,
                    nowUtc,
                    "Role assigned."));
        }

    /// <summary>
    /// Removes role from user.
    /// </summary>
    /// <param name="roleId">
    /// Role identifier.
    /// </param>
    /// <param name="nowUtc">
    /// Current UTC timestamp.
    /// </param>
    /// <exception cref="DomainException">
    /// Thrown when role assignment does not exist.
    /// </exception>
    public void RemoveRole(Guid roleId, DateTime nowUtc)
    {
        Guard.AgainstEmpty(roleId, nameof(roleId));

        var existing = _roleAssignments
            .FirstOrDefault(r => r.RoleId == roleId);

        if (existing is null)
        {
            throw new DomainException(
                IdentityDomainErrorCodes.RoleNotAssigned,
                "Role assignment does not exist.");
        }

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        _roleAssignments.Remove(existing);

        SecurityStamp = Guid.NewGuid().ToString("N");
        UpdatedAt = nowUtc;

        AddDomainEvent(
            new RoleRemovedDomainEvent(
                Id,
                nowUtc,
                roleId));

        AddDomainEvent(
            new SessionInvalidatedDomainEvent(
                Id,
                nowUtc,
                "Role removed."));
    }

    // ============================================================
    // STATE TRANSITIONS
    // ============================================================

    /// <summary>
    /// Unlocks user account.
    /// </summary>
    public void Unlock(DateTime nowUtc)
    {
        if (Status != UserStatus.Locked)
            throw new DomainException(IdentityDomainErrorCodes.InvalidState, "Only locked user can be unlocked.");

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        Status = UserStatus.Active;
        FailedLoginCount = 0;
        LockoutUntil = null;
        UpdatedAt = nowUtc;

        AddDomainEvent(new UserUnlockedDomainEvent(Id, nowUtc));
    }

    /// <summary>
    /// Disables user account.
    /// </summary>
    public void Disable(DateTime nowUtc)
    {
        if (Status == UserStatus.Disabled)
            throw new DomainException(IdentityDomainErrorCodes.InvalidState, "Already disabled.");

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        Status = UserStatus.Disabled;
        UpdatedAt = nowUtc;

        AddDomainEvent(new SessionInvalidatedDomainEvent(Id, nowUtc, "Account disabled."));
    }

    /// <summary>
    /// Restores user account.
    /// </summary>
    public void Restore(DateTime nowUtc)
    {
        if (Status != UserStatus.Disabled)
            throw new DomainException(IdentityDomainErrorCodes.InvalidState, "Only disabled account can be restored.");

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        Status = UserStatus.Active;
        UpdatedAt = nowUtc;
    }

    /// <summary>
    /// Verifies user's email address.
    /// </summary>
    public void VerifyEmail(DateTime nowUtc)
    {
        if (EmailVerified)
            return;

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        EmailVerified = true;
        UpdatedAt = nowUtc;

        AddDomainEvent(new EmailVerifiedDomainEvent(Id, nowUtc, Email.Value));
    }

    /// <summary>
    /// Verifies user's phone number.
    /// </summary>
    public void VerifyPhone(DateTime nowUtc)
    {
        if (PhoneVerified)
            return;

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        PhoneVerified = true;
        UpdatedAt = nowUtc;

        AddDomainEvent(new PhoneVerifiedDomainEvent(Id, nowUtc, PhoneNumber.Value));
    }

    /// <summary>
    /// Sets TOTP secret for MFA.
    /// </summary>
    public void SetTotpSecret(string encryptedSecret, DateTime nowUtc)
    {
        Guard.AgainstNullOrWhiteSpace(encryptedSecret, nameof(encryptedSecret));
        
        if (!EmailVerified && !PhoneVerified)
            throw new DomainException(
                 IdentityDomainErrorCodes.ContactNotVerified,
                "At least one contact method must be verified before setting TOTP.");

        Guard.AgainstNonUtc(nowUtc, nameof(nowUtc));

        TOTPSecretEncrypted = encryptedSecret;
        UpdatedAt = nowUtc;

        AddDomainEvent(new TotpSecretSetDomainEvent(Id, nowUtc));
    }  
}