using Platform.Communication.Options;

namespace Platform.Communication.UnitTests.Options;

/// <summary>
/// Contains unit tests for <see cref="CommunicationOptions"/>.
/// </summary>
public sealed class CommunicationOptionsTests
{
    /// <summary>
    /// Verifies that the section name
    /// matches the expected configuration section.
    /// </summary>
    [Fact]
    public void SectionName_Should_ReturnExpectedValue()
    {
        // Arrange / Act / Assert
        CommunicationOptions.SectionName
            .Should()
            .Be("Communication");
    }

    /// <summary>
    /// Verifies that the constructor initializes
    /// all nested options.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeNestedOptions()
    {
        // Arrange

        // Act
        CommunicationOptions options = new();

        // Assert
        options.Email.Should().NotBeNull();
        options.Sms.Should().NotBeNull();
        options.WhatsApp.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that the email options
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void Email_Should_BeAssignable()
    {
        // Arrange
        CommunicationOptions options = new();
        EmailOptions expected = new();

        // Act
        options.Email = expected;

        // Assert
        options.Email.Should().BeSameAs(expected);
    }

    /// <summary>
    /// Verifies that the SMS options
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void Sms_Should_BeAssignable()
    {
        // Arrange
        CommunicationOptions options = new();
        SmsOptions expected = new();

        // Act
        options.Sms = expected;

        // Assert
        options.Sms.Should().BeSameAs(expected);
    }

    /// <summary>
    /// Verifies that the WhatsApp options
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void WhatsApp_Should_BeAssignable()
    {
        // Arrange
        CommunicationOptions options = new();
        WhatsAppOptions expected = new();

        // Act
        options.WhatsApp = expected;

        // Assert
        options.WhatsApp.Should().BeSameAs(expected);
    }
}