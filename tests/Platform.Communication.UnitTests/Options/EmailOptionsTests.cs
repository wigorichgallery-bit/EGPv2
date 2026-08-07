using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Enums;
using Platform.Communication.Options;

namespace Platform.Communication.UnitTests.Options;

/// <summary>
/// Contains unit tests for <see cref="EmailOptions"/>.
/// </summary>
public sealed class EmailOptionsTests
{
    /// <summary>
    /// Verifies that the constructor initializes
    /// the email configuration.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeConfiguration()
    {
        // Arrange

        // Act
        EmailOptions options = new();

        // Assert
        options.Configuration.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that the provider property
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Provider_Should_BeAssignable()
    {
        // Arrange
        EmailOptions options = new();

        // Act
        options.Provider = EmailProviderType.SendGrid;

        // Assert
        options.Provider.Should().Be(EmailProviderType.SendGrid);
    }

    /// <summary>
    /// Verifies that the configuration property
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Configuration_Should_BeAssignable()
    {
        // Arrange
        EmailOptions options = new();
        EmailConfiguration configuration = new();

        // Act
        options.Configuration = configuration;

        // Assert
        options.Configuration.Should().BeSameAs(configuration);
    }
}