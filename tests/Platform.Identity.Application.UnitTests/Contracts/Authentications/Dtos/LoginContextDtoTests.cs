using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Dtos;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Authentication.Dtos;

/// <summary>
/// Unit tests for <see cref="LoginContextDto"/>.
/// </summary>
public sealed class LoginContextDtoTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        const string ipAddress = "192.168.1.100";
        const string userAgent = "Mozilla/5.0";
        const string deviceFingerprint = "DEVICE-001";
        const string countryCode = "ID";
        const string city = "Jakarta";
        const double latitude = -6.2088;
        const double longitude = 106.8456;

        // Act
        var dto = new LoginContextDto(
            ipAddress,
            userAgent,
            deviceFingerprint,
            countryCode,
            city,
            latitude,
            longitude);

        // Assert
        dto.IpAddress.Should().Be(ipAddress);
        dto.UserAgent.Should().Be(userAgent);
        dto.DeviceFingerprint.Should().Be(deviceFingerprint);
        dto.CountryCode.Should().Be(countryCode);
        dto.City.Should().Be(city);
        dto.Latitude.Should().Be(latitude);
        dto.Longitude.Should().Be(longitude);
    }

    /// <summary>
    /// Verifies nullable properties accept null values.
    /// </summary>
    [Fact]
    public void Constructor_Should_Accept_Null_Optional_Values()
    {
        // Act
        var dto = new LoginContextDto(
            "127.0.0.1",
            "UnitTest",
            "DEVICE",
            null,
            null,
            null,
            null);

        // Assert
        dto.CountryCode.Should().BeNull();
        dto.City.Should().BeNull();
        dto.Latitude.Should().BeNull();
        dto.Longitude.Should().BeNull();
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var left = new LoginContextDto(
            "127.0.0.1",
            "Agent",
            "DEVICE",
            "ID",
            "Jakarta",
            -6.2,
            106.8);

        var right = new LoginContextDto(
            "127.0.0.1",
            "Agent",
            "DEVICE",
            "ID",
            "Jakarta",
            -6.2,
            106.8);

        // Assert
        left.Should().Be(right);
        left.Equals(right).Should().BeTrue();
        (left == right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies different records are not equal.
    /// </summary>
    [Fact]
    public void Different_Records_Should_Not_Be_Equal()
    {
        // Arrange
        var left = new LoginContextDto(
            "127.0.0.1",
            "Agent",
            "DEVICE-A",
            null,
            null,
            null,
            null);

        var right = new LoginContextDto(
            "127.0.0.2",
            "Agent",
            "DEVICE-B",
            null,
            null,
            null,
            null);

        // Assert
        left.Should().NotBe(right);
        (left == right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies deconstruction returns all property values.
    /// </summary>
    [Fact]
    public void Deconstruct_Should_Return_All_Property_Values()
    {
        // Arrange
        var dto = new LoginContextDto(
            "192.168.1.100",
            "Mozilla",
            "DEVICE-001",
            "ID",
            "Jakarta",
            -6.2088,
            106.8456);

        // Act
        var (
            ipAddress,
            userAgent,
            deviceFingerprint,
            countryCode,
            city,
            latitude,
            longitude) = dto;

        // Assert
        ipAddress.Should().Be("192.168.1.100");
        userAgent.Should().Be("Mozilla");
        deviceFingerprint.Should().Be("DEVICE-001");
        countryCode.Should().Be("ID");
        city.Should().Be("Jakarta");
        latitude.Should().Be(-6.2088);
        longitude.Should().Be(106.8456);
    }

    /// <summary>
    /// Verifies the string representation contains significant property values.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Values()
    {
        // Arrange
        var dto = new LoginContextDto(
            "127.0.0.1",
            "Mozilla",
            "DEVICE-001",
            "ID",
            "Jakarta",
            null,
            null);

        // Act
        var text = dto.ToString();

        // Assert
        text.Should().Contain(nameof(LoginContextDto.IpAddress));
        text.Should().Contain(nameof(LoginContextDto.UserAgent));
        text.Should().Contain(nameof(LoginContextDto.DeviceFingerprint));
        text.Should().Contain("127.0.0.1");
        text.Should().Contain("Mozilla");
    }
}