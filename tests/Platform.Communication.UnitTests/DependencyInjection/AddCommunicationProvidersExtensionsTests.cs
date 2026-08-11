using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Channels.Sms.Providers;
using Platform.Communication.Channels.WhatsApp.Providers;
using Platform.Communication.DependencyInjection;
using Platform.Communication.Enums;
using Platform.Communication.Options;

namespace Platform.Communication.UnitTests.DependencyInjection;

/// <summary>
/// Contains unit tests for
/// <see cref="AddCommunicationProvidersExtensions"/>.
/// </summary>
public sealed class AddCommunicationProvidersExtensionsTests
{
    // ==========================================================
    // AddCommunicationProviders
    // ==========================================================

    /// <summary>
    /// Verifies that AddCommunicationProviders throws an
    /// <see cref="ArgumentNullException"/> when the service
    /// collection is null.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_ThrowArgumentNullException_When_ServicesIsNull()
    {
        // Act

        Action action =
            () =>
                AddCommunicationProvidersExtensions
                    .AddCommunicationProviders(
                        null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("services");
    }

    /// <summary>
    /// Verifies that AddCommunicationProviders returns
    /// the same service collection instance.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_ReturnSameServiceCollection()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        IServiceCollection result =
            services.AddCommunicationProviders();

        // Assert

        result.Should()
            .BeSameAs(services);
    }

    // ==========================================================
    // Email Provider Registrations
    // ==========================================================

    /// <summary>
    /// Verifies that the SMTP email provider is registered
    /// as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterSmtpEmailProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(SmtpEmailProvider)
                    &&
                    descriptor.ImplementationType ==
                        typeof(SmtpEmailProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the SendGrid email provider is registered
    /// as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterSendGridEmailProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(SendGridEmailProvider)
                    &&
                    descriptor.ImplementationType ==
                        typeof(SendGridEmailProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the Microsoft Graph email provider is
    /// registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterMicrosoftGraphEmailProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(MicrosoftGraphEmailProvider)
                    &&
                    descriptor.ImplementationType ==
                        typeof(MicrosoftGraphEmailProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // SMS Provider Registrations
    // ==========================================================

    /// <summary>
    /// Verifies that the Twilio SMS provider is registered
    /// as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterTwilioSmsProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(TwilioSmsProvider)
                    &&
                    descriptor.ImplementationType ==
                        typeof(TwilioSmsProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the Vonage SMS provider is registered
    /// as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterVonageSmsProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(VonageSmsProvider)
                    &&
                    descriptor.ImplementationType ==
                        typeof(VonageSmsProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // WhatsApp Provider Registrations
    // ==========================================================

    /// <summary>
    /// Verifies that the Meta Cloud WhatsApp provider is
    /// registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterMetaCloudWhatsAppProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(MetaCloudWhatsAppProvider)
                    &&
                    descriptor.ImplementationType ==
                        typeof(MetaCloudWhatsAppProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the Twilio WhatsApp provider is
    /// registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterTwilioWhatsAppProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(TwilioWhatsAppProvider)
                    &&
                    descriptor.ImplementationType ==
                        typeof(TwilioWhatsAppProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // Interface Registrations
    // ==========================================================

    /// <summary>
    /// Verifies that IEmailProvider is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterIEmailProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IEmailProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that ISmsProvider is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterISmsProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(ISmsProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that IWhatsAppProvider is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterIWhatsAppProvider_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IWhatsAppProvider)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // Registration Count
    // ==========================================================

    /// <summary>
    /// Verifies that all expected provider registrations
    /// are added.
    /// </summary>
    [Fact]
    public void AddCommunicationProviders_Should_RegisterAllExpectedProviders()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationProviders();

        // Assert

        services.Should()
            .HaveCount(10);
    }
}