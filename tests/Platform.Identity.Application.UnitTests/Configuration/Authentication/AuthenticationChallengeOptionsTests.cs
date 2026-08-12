
using Platform.Identity.Application.Configuration.Authentication;

namespace Platform.Identity.Application.UnitTests.Configuration;

/// <summary>
/// Unit tests for <see cref="AuthenticationChallengeOptions"/>.
/// </summary>
public sealed class AuthenticationChallengeOptionsTests
{
    /// <summary>
    /// Verifies the default configuration values.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_Default_Values()
    {
        // Act
        var options = new AuthenticationChallengeOptions();

        // Assert
        options.LoginChallengeLifetime.Should().Be(TimeSpan.FromMinutes(5));
        options.MaximumFailedAttempts.Should().Be(5);
    }

    /// <summary>
    /// Verifies init properties can be assigned.
    /// </summary>
    [Fact]
    public void Init_Properties_Should_Be_Assignable()
    {
        // Act
        var options = new AuthenticationChallengeOptions
        {
            LoginChallengeLifetime = TimeSpan.FromMinutes(10),
            MaximumFailedAttempts = 8
        };

        // Assert
        options.LoginChallengeLifetime.Should().Be(TimeSpan.FromMinutes(10));
        options.MaximumFailedAttempts.Should().Be(8);
    }
}