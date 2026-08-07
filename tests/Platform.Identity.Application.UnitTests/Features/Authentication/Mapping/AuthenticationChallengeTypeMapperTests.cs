using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Mapping;
using Xunit;

using ContractChallengeType =
    Platform.Identity.Application.Contracts.Authentication.Enums.AuthenticationChallengeType;

using DomainChallengeType =
    Platform.Identity.Domain.Enums.AuthenticationChallengeType;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Mapping;

/// <summary>
/// Contains unit tests for <see cref="AuthenticationChallengeTypeMapper"/>.
/// </summary>
public sealed class AuthenticationChallengeTypeMapperTests
{
    /// <summary>
    /// Gets all supported mapping cases.
    /// </summary>
    public static TheoryData<DomainChallengeType, ContractChallengeType> MappingCases =>
        new()
        {
            { DomainChallengeType.None, ContractChallengeType.None },
            { DomainChallengeType.Totp, ContractChallengeType.Totp },
            { DomainChallengeType.EmailOtp, ContractChallengeType.EmailOtp },
            { DomainChallengeType.SmsOtp, ContractChallengeType.SmsOtp },
            { DomainChallengeType.WhatsAppOtp, ContractChallengeType.WhatsAppOtp },
            { DomainChallengeType.Passkey, ContractChallengeType.Passkey },
            { DomainChallengeType.RecoveryCode, ContractChallengeType.RecoveryCode },
            { DomainChallengeType.MagicLink, ContractChallengeType.MagicLink },
            { DomainChallengeType.Custom, ContractChallengeType.Custom }
        };

    /// <summary>
    /// Verifies each supported domain challenge type is mapped
    /// to the expected application contract value.
    /// </summary>
    [Theory]
    [MemberData(nameof(MappingCases))]
    public void ToContract_Should_Return_Expected_Value(
        DomainChallengeType domainValue,
        ContractChallengeType expectedValue)
    {
        // Act
        var result =
            AuthenticationChallengeTypeMapper.ToContract(domainValue);

        // Assert
        result.Should().Be(expectedValue);
    }

    /// <summary>
    /// Verifies unsupported challenge types throw an exception.
    /// </summary>
    [Fact]
    public void ToContract_Should_Throw_ArgumentOutOfRangeException_When_Value_Is_Not_Supported()
    {
        // Arrange
        var unsupported =
            (DomainChallengeType)int.MaxValue;

        // Act
        var action =
            () => AuthenticationChallengeTypeMapper.ToContract(unsupported);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithParameterName("value");
    }
}