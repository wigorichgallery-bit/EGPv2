using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Mapping;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Mapping;

/// <summary>
/// Contains unit tests for <see cref="AuthenticationChallengeTypeResolver"/>.
/// </summary>
public sealed class AuthenticationChallengeTypeResolverTests
{
    /// <summary>
    /// Gets all supported MFA method mappings.
    /// </summary>
    public static TheoryData<MFAMethod, AuthenticationChallengeType> ResolveCases =>
        new()
        {
            { MFAMethod.TOTP, AuthenticationChallengeType.Totp },
            { MFAMethod.Email, AuthenticationChallengeType.EmailOtp },
            { MFAMethod.SMS, AuthenticationChallengeType.SmsOtp },
            { MFAMethod.WhatsApp, AuthenticationChallengeType.WhatsAppOtp }
        };

    /// <summary>
    /// Verifies supported MFA methods resolve to the expected
    /// authentication challenge type.
    /// </summary>
    [Theory]
    [MemberData(nameof(ResolveCases))]
    public void Resolve_Should_Return_Expected_Challenge_Type(
        MFAMethod method,
        AuthenticationChallengeType expected)
    {
        // Act
        var result =
            AuthenticationChallengeTypeResolver.Resolve(method);

        // Assert
        result.Should().Be(expected);
    }

    /// <summary>
    /// Verifies unsupported MFA methods throw an exception.
    /// </summary>
    [Fact]
    public void Resolve_Should_Throw_ArgumentOutOfRangeException_When_Method_Is_Not_Supported()
    {
        // Arrange
        var unsupported =
            (MFAMethod)int.MaxValue;

        // Act
        var action =
            () => AuthenticationChallengeTypeResolver.Resolve(unsupported);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithParameterName("method");
    }
}