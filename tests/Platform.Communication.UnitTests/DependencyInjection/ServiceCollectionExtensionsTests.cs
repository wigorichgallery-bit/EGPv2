using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Channels.Email.Sender;
using Platform.Communication.Channels.Sms.Providers;
using Platform.Communication.Channels.Sms.Sender;
using Platform.Communication.Channels.WhatsApp.Providers;
using Platform.Communication.Channels.WhatsApp.Sender;
using Platform.Communication.DependencyInjection;
using Platform.Communication.Options;

namespace Platform.Communication.UnitTests.DependencyInjection;

/// <summary>
/// Contains unit tests for
/// <see cref="ServiceCollectionExtensions"/>.
/// </summary>
public sealed class ServiceCollectionExtensionsTests
{
    // ==========================================================
    // AddPlatformCommunication
    // ==========================================================

    /// <summary>
    /// Verifies that AddPlatformCommunication throws an
    /// <see cref="ArgumentNullException"/> when the service
    /// collection is null.
    /// </summary>
    [Fact]
    public void AddPlatformCommunication_Should_ThrowArgumentNullException_When_ServicesIsNull()
    {
        // Arrange

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        Action action =
            () =>
                ServiceCollectionExtensions
                    .AddPlatformCommunication(
                        null!,
                        configuration);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("services");
    }

    /// <summary>
    /// Verifies that AddPlatformCommunication throws an
    /// <see cref="ArgumentNullException"/> when configuration
    /// is null.
    /// </summary>
    [Fact]
    public void AddPlatformCommunication_Should_ThrowArgumentNullException_When_ConfigurationIsNull()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        Action action =
            () =>
                services.AddPlatformCommunication(
                    null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("configuration");
    }

    /// <summary>
    /// Verifies that AddPlatformCommunication returns the
    /// same service collection instance.
    /// </summary>
    [Fact]
    public void AddPlatformCommunication_Should_ReturnSameServiceCollection()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        IServiceCollection result =
            services.AddPlatformCommunication(
                configuration);

        // Assert

        result.Should()
            .BeSameAs(services);
    }

    // ==========================================================
    // Options
    // ==========================================================

    // /// <summary>
    // /// Verifies that CommunicationOptions are registered.
    // /// </summary>
    // [Fact]
    // public void AddPlatformCommunication_Should_RegisterCommunicationOptions()
    // {
    //     // Arrange

    //     ServiceCollection services =
    //         new();

    //     IConfiguration configuration =
    //         CreateConfiguration();

    //     // Act

    //     services.AddPlatformCommunication(
    //         configuration);

    //     // Assert

    //     services.Should()
    //         .Contain(
    //             descriptor =>
    //                 descriptor.ServiceType ==
    //                 typeof(
    //                     IOptions<CommunicationOptions>));
    // }

    // ==========================================================
    // Providers
    // ==========================================================

    /// <summary>
    /// Verifies that all email providers are registered.
    /// </summary>
    [Fact]
    public void AddPlatformCommunication_Should_RegisterEmailProviders()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        services.AddPlatformCommunication(
            configuration);

        // Assert

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(SmtpEmailProvider));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(SendGridEmailProvider));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(MicrosoftGraphEmailProvider));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(IEmailProvider));
    }

    /// <summary>
    /// Verifies that all SMS providers are registered.
    /// </summary>
    [Fact]
    public void AddPlatformCommunication_Should_RegisterSmsProviders()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        services.AddPlatformCommunication(
            configuration);

        // Assert

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(TwilioSmsProvider));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(VonageSmsProvider));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(ISmsProvider));
    }

    /// <summary>
    /// Verifies that all WhatsApp providers are registered.
    /// </summary>
    [Fact]
    public void AddPlatformCommunication_Should_RegisterWhatsAppProviders()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        services.AddPlatformCommunication(
            configuration);

        // Assert

        _ = services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(MetaCloudWhatsAppProvider));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(TwilioWhatsAppProvider));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(IWhatsAppProvider));
    }

    // ==========================================================
    // Senders
    // ==========================================================

    /// <summary>
    /// Verifies that all communication senders are registered.
    /// </summary>
    [Fact]
    public void AddPlatformCommunication_Should_RegisterSenders()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        services.AddPlatformCommunication(
            configuration);

        // Assert

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(IEmailSender));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(ISmsSender));

        services.Should()
            .Contain(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(IWhatsAppSender));
    }

    // ==========================================================
    // Options Binding
    // ==========================================================

    /// <summary>
    /// Verifies that CommunicationOptions can be resolved
    /// after the complete Platform.Communication registration.
    /// </summary>
    [Fact]
    public void AddPlatformCommunication_Should_ResolveCommunicationOptions()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        services.AddPlatformCommunication(
            configuration);

        ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        // Assert

        IOptions<CommunicationOptions> options =
            serviceProvider
                .GetRequiredService<
                    IOptions<CommunicationOptions>>();

        options.Should()
            .NotBeNull();

        options.Value.Should()
            .NotBeNull();
    }

    // ==========================================================
    // Helpers
    // ==========================================================

    /// <summary>
    /// Creates a minimal configuration required by
    /// the communication options registration.
    /// </summary>
    private static IConfiguration CreateConfiguration()
    {
        Dictionary<string, string?> values =
            new()
            {
                [
                    $"{CommunicationOptions.SectionName}:Email:Provider"
                ] = "Smtp",

                [
                    $"{CommunicationOptions.SectionName}:Sms:Provider"
                ] = "Twilio",

                [
                    $"{CommunicationOptions.SectionName}:WhatsApp:Provider"
                ] = "MetaCloud"
            };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}