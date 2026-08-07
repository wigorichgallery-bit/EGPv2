using FluentAssertions;
using Platform.Identity.Application.Features.Common;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Common;

/// <summary>
/// Unit tests for <see cref="ValidationConstants"/>.
/// </summary>
public sealed class ValidationConstantsTests
{
    /// <summary>
    /// Verifies username length constants.
    /// </summary>
    [Fact]
    public void Username_Length_Constants_Should_Be_Correct()
    {
        ValidationConstants.UsernameMinLength
            .Should()
            .Be(3);

        ValidationConstants.UsernameMaxLength
            .Should()
            .Be(100);
    }

    /// <summary>
    /// Verifies password length constants.
    /// </summary>
    [Fact]
    public void Password_Length_Constants_Should_Be_Correct()
    {
        ValidationConstants.PasswordMinLength
            .Should()
            .Be(8);

        ValidationConstants.MaximumPasswordLength
            .Should()
            .Be(256);
    }

    /// <summary>
    /// Verifies identity length constant.
    /// </summary>
    [Fact]
    public void Identity_Length_Constant_Should_Be_Correct()
    {
        ValidationConstants.MaximumIdentityLength
            .Should()
            .Be(256);
    }

    /// <summary>
    /// Verifies verification code length constant.
    /// </summary>
    [Fact]
    public void Verification_Code_Length_Constant_Should_Be_Correct()
    {
        ValidationConstants.VerificationCodeMaxLength
            .Should()
            .Be(32);
    }

    /// <summary>
    /// Verifies validation constants maintain a consistent relationship.
    /// </summary>
    [Fact]
    public void Validation_Constants_Should_Be_Consistent()
    {
        ValidationConstants.UsernameMinLength
            .Should()
            .BeLessThan(
                ValidationConstants.UsernameMaxLength);

        ValidationConstants.PasswordMinLength
            .Should()
            .BeLessThan(
                ValidationConstants.MaximumPasswordLength);

        ValidationConstants.MaximumIdentityLength
            .Should()
            .BeGreaterThan(
                ValidationConstants.UsernameMaxLength);
    }
}