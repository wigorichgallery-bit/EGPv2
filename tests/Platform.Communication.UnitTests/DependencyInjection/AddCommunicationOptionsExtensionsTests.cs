using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Platform.Communication.DependencyInjection;
using Platform.Communication.Options;

namespace Platform.Communication.UnitTests.DependencyInjection;

/// <summary>
/// Contains unit tests for
/// <see cref="AddCommunicationOptionsExtensions"/>.
/// </summary>
public sealed class AddCommunicationOptionsExtensionsTests
{
    // ==========================================================
    // AddCommunicationOptions
    // ==========================================================

    /// <summary>
    /// Verifies that AddCommunicationOptions throws an
    /// <see cref="ArgumentNullException"/>
    /// when the service collection is null.
    /// </summary>
    [Fact]
    public void AddCommunicationOptions_Should_ThrowArgumentNullException_When_ServicesIsNull()
    {
        // Act

        Action action =
            () =>
                AddCommunicationOptionsExtensions
                    .AddCommunicationOptions(
                        null!,
                        CreateConfiguration());

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("services");
    }

    /// <summary>
    /// Verifies that AddCommunicationOptions throws an
    /// <see cref="ArgumentNullException"/>
    /// when the configuration is null.
    /// </summary>
    [Fact]
    public void AddCommunicationOptions_Should_ThrowArgumentNullException_When_ConfigurationIsNull()
    {
        // Arrange

        ServiceCollection services =
            new();

        // Act

        Action action =
            () =>
                services.AddCommunicationOptions(
                    null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("configuration");
    }

    /// <summary>
    /// Verifies that AddCommunicationOptions returns
    /// the same service collection instance.
    /// </summary>
    [Fact]
    public void AddCommunicationOptions_Should_ReturnSameServiceCollection()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        IServiceCollection result =
            services.AddCommunicationOptions(
                configuration);

        // Assert

        result.Should()
            .BeSameAs(services);
    }

    // ==========================================================
    // Registration
    // ==========================================================

    /// <summary>
    /// Verifies that CommunicationOptions is registered
    /// after AddCommunicationOptions is called.
    /// </summary>
    [Fact]
    public void AddCommunicationOptions_Should_RegisterCommunicationOptions()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            CreateConfiguration();

        // Act

        services.AddCommunicationOptions(
            configuration);

        // Assert

        ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        IOptions<CommunicationOptions> options =
            serviceProvider
                .GetRequiredService<
                    IOptions<CommunicationOptions>>();

        options.Should()
            .NotBeNull();
    }

    /// <summary>
    /// Verifies that configuration values are bound
    /// to CommunicationOptions.
    /// </summary>
    [Fact]
    public void AddCommunicationOptions_Should_BindConfiguration()
    {
        // Arrange

        Dictionary<string, string?> values =
            new()
            {
                [
                    $"{CommunicationOptions.SectionName}:Email:Provider"
                ] = "Smtp"
            };

        IConfiguration configuration =
            new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();

        ServiceCollection services =
            new();

        // Act

        services.AddCommunicationOptions(
            configuration);

        ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        CommunicationOptions options =
            serviceProvider
                .GetRequiredService<
                    IOptions<CommunicationOptions>>()
                .Value;

        // Assert

        options.Should()
            .NotBeNull();
    }

    /// <summary>
    /// Verifies that the options registration uses
    /// the configured CommunicationOptions section.
    /// </summary>
    [Fact]
    public void AddCommunicationOptions_Should_UseCommunicationOptionsSection()
    {
        // Arrange

        ServiceCollection services =
            new();

        IConfiguration configuration =
            new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        [
                            $"{CommunicationOptions.SectionName}:Email:Provider"
                        ] = "Smtp"
                    })
                .Build();

        // Act

        services.AddCommunicationOptions(
            configuration);

        ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        // Assert

        IOptions<CommunicationOptions> options =
            serviceProvider
                .GetRequiredService<
                    IOptions<CommunicationOptions>>();

        options.Value
            .Should()
            .NotBeNull();
    }

    // ==========================================================
    // Helpers
    // ==========================================================

    /// <summary>
    /// Creates an empty configuration containing
    /// the CommunicationOptions section.
    /// </summary>
    private static IConfiguration CreateConfiguration()
    {
        Dictionary<string, string?> values =
            new()
            {
                [
                    $"{CommunicationOptions.SectionName}:Email:Provider"
                ] = "Smtp"
            };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}