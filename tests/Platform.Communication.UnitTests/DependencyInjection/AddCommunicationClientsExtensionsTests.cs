using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Channels.Sms.Clients;
using Platform.Communication.Channels.WhatsApp.Clients;
using Platform.Communication.DependencyInjection;

namespace Platform.Communication.UnitTests.DependencyInjection;

/// <summary>
/// Contains unit tests for
/// <see cref="AddCommunicationClientsExtensions"/>.
/// </summary>
public sealed class AddCommunicationClientsExtensionsTests
{
    // ==========================================================
    // AddCommunicationClients
    // ==========================================================

    /// <summary>
    /// Verifies that AddCommunicationClients throws an
    /// <see cref="ArgumentNullException"/> when the service
    /// collection is null.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_ThrowArgumentNullException_When_ServicesIsNull()
    {
        // Act

        Action action =
            () =>
                AddCommunicationClientsExtensions
                    .AddCommunicationClients(
                        null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("services");
    }

    /// <summary>
    /// Verifies that AddCommunicationClients returns
    /// the same service collection instance.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_ReturnSameServiceCollection()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        IServiceCollection result =
            services.AddCommunicationClients();

        // Assert

        result.Should()
            .BeSameAs(services);
    }

    // ==========================================================
    // Email Registrations
    // ==========================================================

    /// <summary>
    /// Verifies that the MailKit SMTP SDK client factory
    /// is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterMailKitSmtpSdkClientFactory_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IMailKitSmtpSdkClientFactory)
                    &&
                    descriptor.ImplementationType ==
                        typeof(MailKitSmtpSdkClientFactory)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the SendGrid SDK client factory
    /// is registered as singleton.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterSendGridSdkClientFactory_AsSingleton()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(ISendGridSdkClientFactory)
                    &&
                    descriptor.ImplementationType ==
                        typeof(SendGridSdkClientFactory)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Singleton);
    }

    /// <summary>
    /// Verifies that the MailKit SMTP client
    /// is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterMailKitSmtpClient_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IMailKitSmtpClient)
                    &&
                    descriptor.ImplementationType ==
                        typeof(MailKitSmtpClient)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the SendGrid client
    /// is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterSendGridClient_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(ISendGridClient)
                    &&
                    descriptor.ImplementationType ==
                        typeof(SendGridClient)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the Microsoft Graph client
    /// is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterGraphClient_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IGraphClient)
                    &&
                    descriptor.ImplementationType ==
                        typeof(GraphClient)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // SMS Registrations
    // ==========================================================

    /// <summary>
    /// Verifies that the Twilio SMS client
    /// is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterTwilioSmsClient_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(ITwilioSmsClient)
                    &&
                    descriptor.ImplementationType ==
                        typeof(TwilioSmsClient)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the Vonage SMS client
    /// is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterVonageSmsClient_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IVonageSmsClient)
                    &&
                    descriptor.ImplementationType ==
                        typeof(VonageSmsClient)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // WhatsApp Registrations
    // ==========================================================

    /// <summary>
    /// Verifies that the Meta Cloud WhatsApp client
    /// is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterMetaCloudWhatsAppClient_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IMetaCloudWhatsAppClient)
                    &&
                    descriptor.ImplementationType ==
                        typeof(MetaCloudWhatsAppClient)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    /// <summary>
    /// Verifies that the Twilio WhatsApp client
    /// is registered as transient.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterTwilioWhatsAppClient_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(ITwilioWhatsAppClient)
                    &&
                    descriptor.ImplementationType ==
                        typeof(TwilioWhatsAppClient)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // Registration Count
    // ==========================================================

    /// <summary>
    /// Verifies that all communication client registrations
    /// defined by AddCommunicationClients are registered.
    /// </summary>
    [Fact]
    public void AddCommunicationClients_Should_RegisterAllExpectedClients()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationClients();

        // Assert

        services.Should()
            .HaveCount(9);
    }
}