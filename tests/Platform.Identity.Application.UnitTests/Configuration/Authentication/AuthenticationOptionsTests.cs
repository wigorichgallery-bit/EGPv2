using FluentAssertions;
using Platform.Identity.Application.Configuration.Authentication;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Configuration.Authentication;

/// <summary>
/// Unit tests for <see cref="AuthenticationOptions"/>.
/// </summary>
public sealed class AuthenticationOptionsTests
{
    /// <summary>
    /// Verifies default values.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_Default_Values()
    {
        // Act
        var options = new AuthenticationOptions();

        // Assert
        options.LockoutThreshold.Should().Be(5);
        options.LockoutDuration.Should().Be(TimeSpan.FromMinutes(15));
    }

    /// <summary>
    /// Verifies init properties can be assigned.
    /// </summary>
    [Fact]
    public void Init_Properties_Should_Be_Assignable()
    {
        // Arrange & Act
        var options = new AuthenticationOptions
        {
            LockoutThreshold = 10,
            LockoutDuration = TimeSpan.FromMinutes(30)
        };

        // Assert
        options.LockoutThreshold.Should().Be(10);
        options.LockoutDuration.Should().Be(TimeSpan.FromMinutes(30));
    }
}