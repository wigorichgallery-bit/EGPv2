using FluentAssertions;
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Users.Dtos;

/// <summary>
/// Unit tests for <see cref="UserDto"/>.
/// </summary>
public sealed class UserDtoTests
{
    /// <summary>
    /// Verifies constructor initializes every property.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_All_Properties()
    {
        // Arrange

        Guid userId =
            Guid.NewGuid();

        // Act

        var dto =
            new UserDto(
                userId,
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                true,
                false,
                UserStatus.Active,
                true,
                MFAMethod.TOTP);

        // Assert

        dto.UserId
            .Should()
            .Be(userId);

        dto.Username
            .Should()
            .Be("john.doe");

        dto.Email
            .Should()
            .Be("john.doe@example.com");

        dto.PhoneNumber
            .Should()
            .Be("+6281234567890");

        dto.EmailVerified
            .Should()
            .BeTrue();

        dto.PhoneVerified
            .Should()
            .BeFalse();

        dto.Status
            .Should()
            .Be(UserStatus.Active);

        dto.MfaEnabled
            .Should()
            .BeTrue();

        dto.MfaMethod
            .Should()
            .Be(MFAMethod.TOTP);
    }

    /// <summary>
    /// Verifies record equality for identical values.
    /// </summary>
    [Fact]
    public void Equality_Should_Return_True_For_Identical_Values()
    {
        // Arrange

        Guid userId =
            Guid.Parse(
                "11111111-1111-1111-1111-111111111111");

        var first =
            new UserDto(
                userId,
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                true,
                true,
                UserStatus.Active,
                true,
                MFAMethod.Email);

        var second =
            new UserDto(
                userId,
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                true,
                true,
                UserStatus.Active,
                true,
                MFAMethod.Email);

        // Assert

        first.Should().Be(second);
    }

    /// <summary>
    /// Verifies record inequality when values differ.
    /// </summary>
    [Fact]
    public void Equality_Should_Return_False_When_Values_Differ()
    {
        // Arrange

        var first =
            new UserDto(
                Guid.NewGuid(),
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                true,
                true,
                UserStatus.Active,
                true,
                MFAMethod.Email);

        var second =
            new UserDto(
                Guid.NewGuid(),
                "jane.doe",
                "jane.doe@example.com",
                "+6289876543210",
                false,
                false,
                UserStatus.Disabled,
                false,
                MFAMethod.None);

        // Assert

        first.Should().NotBe(second);
    }

    /// <summary>
    /// Verifies MFA information is preserved.
    /// </summary>
    [Fact]
    public void Constructor_Should_Preserve_Mfa_Information()
    {
        // Arrange & Act

        var dto =
            new UserDto(
                Guid.NewGuid(),
                "john.doe",
                "john.doe@example.com",
                "+6281234567890",
                true,
                true,
                UserStatus.Active,
                false,
                MFAMethod.None);

        // Assert

        dto.MfaEnabled
            .Should()
            .BeFalse();

        dto.MfaMethod
            .Should()
            .Be(MFAMethod.None);
    }
}