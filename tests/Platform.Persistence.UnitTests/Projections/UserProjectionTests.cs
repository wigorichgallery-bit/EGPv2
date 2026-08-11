// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Projections/UserProjectionTests.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Projections;

namespace Platform.Persistence.UnitTests.Projections;

/// <summary>
/// Contains unit tests for the
/// <see cref="UserProjection"/> class.
///
/// Responsibility:
/// - Verify UserAccount to UserDto projection.
/// - Verify aggregate identity mapping.
/// - Verify value object flattening.
/// - Verify verification state mapping.
/// - Verify authentication state mapping.
/// - Verify null argument protection.
///
/// Testing Strategy:
/// - Exercise the public ToDto method.
/// - Use a real UserAccount aggregate.
/// - Verify the resulting immutable DTO.
/// - Avoid EF Core.
/// - Avoid database connections.
/// - Avoid mocking pure domain objects.
///
/// Architectural Rules:
/// - Projection tests only.
/// - No business logic testing.
/// - No persistence testing.
/// - No domain behavior testing.
/// </summary>
public sealed class UserProjectionTests
{
    /// <summary>
    /// Creates a valid
    /// <see cref="UserAccount"/> aggregate
    /// for projection testing.
    ///
    /// The aggregate is created through its
    /// public domain constructor so that the
    /// test observes the same state that the
    /// projection receives from the domain layer.
    /// </summary>
    /// <returns>
    /// A valid UserAccount aggregate.
    /// </returns>
    private static UserAccount CreateUser()
    {
        return new UserAccount(
            Guid.Parse(
                "11111111-1111-1111-1111-111111111111"),
            "john.doe",
            new EmailAddress(
                "John.Doe@Example.com"),
            new PhoneNumber(
                "+628123456789"),
            "password-hash",
            new DateTime(
                2026,
                8,
                11,
                9,
                0,
                0,
                DateTimeKind.Utc));
    }

    /// <summary>
    /// Verifies that ToDto throws
    /// <see cref="ArgumentNullException"/>
    /// when the supplied user aggregate
    /// is null.
    /// </summary>
    [Fact]
    public void ToDto_Should_ThrowArgumentNullException_When_UserIsNull()
    {
        // Arrange
        UserAccount user = null!;

        // Act
        Action act = () =>
            UserProjection.ToDto(user);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that the aggregate identifier
    /// and username are mapped to the corresponding
    /// DTO properties.
    /// </summary>
    [Fact]
    public void ToDto_Should_MapIdentityProperties()
    {
        // Arrange
        var user = CreateUser();

        // Act
        var result =
            UserProjection.ToDto(user);

        // Assert
        result.UserId
            .Should()
            .Be(user.Id);

        result.Username
            .Should()
            .Be(user.Username);
    }

    /// <summary>
    /// Verifies that EmailAddress and PhoneNumber
    /// value objects are flattened into their
    /// primitive string representations.
    /// </summary>
    [Fact]
    public void ToDto_Should_FlattenEmailAndPhoneValueObjects()
    {
        // Arrange
        var user = CreateUser();

        // Act
        var result =
            UserProjection.ToDto(user);

        // Assert
        result.Email
            .Should()
            .Be(user.Email.Value);

        result.PhoneNumber
            .Should()
            .Be(user.PhoneNumber.Value);
    }

    /// <summary>
    /// Verifies that email and phone verification
    /// states are mapped without modification.
    /// </summary>
    [Fact]
    public void ToDto_Should_MapVerificationStates()
    {
        // Arrange
        var user = CreateUser();

        // Act
        var result =
            UserProjection.ToDto(user);

        // Assert
        result.EmailVerified
            .Should()
            .Be(user.EmailVerified);

        result.PhoneVerified
            .Should()
            .Be(user.PhoneVerified);
    }

    /// <summary>
    /// Verifies that the user status is mapped
    /// without modification.
    /// </summary>
    [Fact]
    public void ToDto_Should_MapUserStatus()
    {
        // Arrange
        var user = CreateUser();

        // Act
        var result =
            UserProjection.ToDto(user);

        // Assert
        result.Status
            .Should()
            .Be(user.Status);
    }

    /// <summary>
    /// Verifies that the MFA enabled state and
    /// MFA method are mapped without modification.
    /// </summary>
    [Fact]
    public void ToDto_Should_MapMfaState()
    {
        // Arrange
        var user = CreateUser();

        // Act
        var result =
            UserProjection.ToDto(user);

        // Assert
        result.MfaEnabled
            .Should()
            .Be(user.MFAEnabled);

        result.MfaMethod
            .Should()
            .Be(user.MFAMethod);
    }

    /// <summary>
    /// Verifies that the complete UserAccount state
    /// exposed by UserDto is projected correctly.
    ///
    /// This test acts as an integration-style verification
    /// of the complete projection contract while remaining
    /// a pure unit test because no persistence infrastructure
    /// or external dependency is involved.
    /// </summary>
    [Fact]
    public void ToDto_Should_MapCompleteUserAccount()
    {
        // Arrange
        var user = CreateUser();

        // Act
        var result =
            UserProjection.ToDto(user);

        // Assert
        result.Should()
            .NotBeNull();

        result.UserId
            .Should()
            .Be(user.Id);

        result.Username
            .Should()
            .Be(user.Username);

        result.Email
            .Should()
            .Be(user.Email.Value);

        result.PhoneNumber
            .Should()
            .Be(user.PhoneNumber.Value);

        result.EmailVerified
            .Should()
            .Be(user.EmailVerified);

        result.PhoneVerified
            .Should()
            .Be(user.PhoneVerified);

        result.Status
            .Should()
            .Be(user.Status);

        result.MfaEnabled
            .Should()
            .Be(user.MFAEnabled);

        result.MfaMethod
            .Should()
            .Be(user.MFAMethod);
    }

    /// <summary>
    /// Verifies that the projection preserves
    /// the default authentication state established
    /// by the UserAccount constructor.
    ///
    /// The constructor initializes:
    /// - EmailVerified = false.
    /// - PhoneVerified = false.
    /// - MFAEnabled = false.
    /// - MFAMethod = None.
    /// - Status = Active.
    /// </summary>
    [Fact]
    public void ToDto_Should_PreserveDefaultAuthenticationState()
    {
        // Arrange
        var user = CreateUser();

        // Act
        var result =
            UserProjection.ToDto(user);

        // Assert
        result.EmailVerified
            .Should()
            .BeFalse();

        result.PhoneVerified
            .Should()
            .BeFalse();

        result.MfaEnabled
            .Should()
            .BeFalse();

        result.MfaMethod
            .Should()
            .Be(MFAMethod.None);

        result.Status
            .Should()
            .Be(UserStatus.Active);
    }
}