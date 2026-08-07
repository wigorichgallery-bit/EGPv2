using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Enums;
using Platform.Identity.Application.Features.Authentication.Mapping;
using Xunit;

using ContractChallengePurpose =
    Platform.Identity.Application.Contracts.Authentication.Enums.AuthenticationChallengePurpose;

using DomainChallengePurpose =
    Platform.Identity.Domain.Enums.AuthenticationChallengePurpose;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Mapping;

/// <summary>
/// Unit tests for <see cref="AuthenticationChallengePurposeMapper"/>.
/// </summary>
public sealed class AuthenticationChallengePurposeMapperTests
{
    /// <summary>
    /// Gets all supported mapping cases.
    /// </summary>
    public static TheoryData<DomainChallengePurpose, ContractChallengePurpose> MappingCases =>
        new()
        {
            { DomainChallengePurpose.Login, ContractChallengePurpose.Login },
            { DomainChallengePurpose.PasswordReset, ContractChallengePurpose.PasswordReset },
            { DomainChallengePurpose.EmailVerification, ContractChallengePurpose.EmailVerification },
            { DomainChallengePurpose.PhoneVerification, ContractChallengePurpose.PhoneVerification },
            { DomainChallengePurpose.SensitiveOperation, ContractChallengePurpose.SensitiveOperation },
            { DomainChallengePurpose.AccountRecovery, ContractChallengePurpose.AccountRecovery },
            { DomainChallengePurpose.Custom, ContractChallengePurpose.Custom }
        };

    /// <summary>
    /// Verifies every supported domain value maps to the expected contract value.
    /// </summary>
    [Theory]
    [MemberData(nameof(MappingCases))]
    public void ToContract_Should_Return_Expected_Value(
        DomainChallengePurpose domainValue,
        ContractChallengePurpose expected)
    {
        // Act
        var result = AuthenticationChallengePurposeMapper.ToContract(domainValue);

        // Assert
        result.Should().Be(expected);
    }

    /// <summary>
    /// Verifies unsupported values throw an exception.
    /// </summary>
    [Fact]
    public void ToContract_Should_Throw_When_Value_Is_Not_Supported()
    {
        // Arrange
        var unsupported =
            (DomainChallengePurpose)int.MaxValue;

        // Act
        var action = () =>
            AuthenticationChallengePurposeMapper.ToContract(unsupported);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithParameterName("value");
    }
}