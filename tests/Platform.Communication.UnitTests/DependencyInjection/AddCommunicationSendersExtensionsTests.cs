using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Platform.Communication.Channels.Email.Sender;
using Platform.Communication.Channels.Sms.Sender;
using Platform.Communication.Channels.WhatsApp.Sender;
using Platform.Communication.DependencyInjection;

namespace Platform.Communication.UnitTests.DependencyInjection;

/// <summary>
/// Contains unit tests for
/// <see cref="AddCommunicationSendersExtensions"/>.
/// </summary>
public sealed class AddCommunicationSendersExtensionsTests
{
    // ==========================================================
    // AddCommunicationSenders
    // ==========================================================

    /// <summary>
    /// Verifies that AddCommunicationSenders throws an
    /// <see cref="ArgumentNullException"/> when the service
    /// collection is null.
    /// </summary>
    [Fact]
    public void AddCommunicationSenders_Should_ThrowArgumentNullException_When_ServicesIsNull()
    {
        // Act

        Action action =
            () =>
                AddCommunicationSendersExtensions
                    .AddCommunicationSenders(
                        null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("services");
    }

    /// <summary>
    /// Verifies that AddCommunicationSenders returns
    /// the same service collection instance.
    /// </summary>
    [Fact]
    public void AddCommunicationSenders_Should_ReturnSameServiceCollection()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        IServiceCollection result =
            services.AddCommunicationSenders();

        // Assert

        result.Should()
            .BeSameAs(services);
    }

    // ==========================================================
    // Email Sender
    // ==========================================================

    /// <summary>
    /// Verifies that IEmailSender is registered with
    /// EmailSender as a transient implementation.
    /// </summary>
    [Fact]
    public void AddCommunicationSenders_Should_RegisterEmailSender_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationSenders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IEmailSender)
                    &&
                    descriptor.ImplementationType ==
                        typeof(EmailSender)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // SMS Sender
    // ==========================================================

    /// <summary>
    /// Verifies that ISmsSender is registered with
    /// SmsSender as a transient implementation.
    /// </summary>
    [Fact]
    public void AddCommunicationSenders_Should_RegisterSmsSender_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationSenders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(ISmsSender)
                    &&
                    descriptor.ImplementationType ==
                        typeof(SmsSender)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // WhatsApp Sender
    // ==========================================================

    /// <summary>
    /// Verifies that IWhatsAppSender is registered with
    /// WhatsAppSender as a transient implementation.
    /// </summary>
    [Fact]
    public void AddCommunicationSenders_Should_RegisterWhatsAppSender_AsTransient()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationSenders();

        // Assert

        services.Should()
            .ContainSingle(
                descriptor =>
                    descriptor.ServiceType ==
                        typeof(IWhatsAppSender)
                    &&
                    descriptor.ImplementationType ==
                        typeof(WhatsAppSender)
                    &&
                    descriptor.Lifetime ==
                        ServiceLifetime.Transient);
    }

    // ==========================================================
    // Registration Count
    // ==========================================================

    /// <summary>
    /// Verifies that exactly three sender registrations
    /// are added.
    /// </summary>
    [Fact]
    public void AddCommunicationSenders_Should_RegisterAllExpectedSenders()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationSenders();

        // Assert

        services.Should()
            .HaveCount(3);
    }
}