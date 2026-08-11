using FluentAssertions;

using Platform.Communication.Exceptions;

namespace Platform.Communication.UnitTests.Exceptions;

/// <summary>
/// Contains unit tests for
/// <see cref="CommunicationException"/>.
/// </summary>
public sealed class CommunicationExceptionTests
{
    // ==========================================================
    // Constructor
    // ==========================================================

    /// <summary>
    /// Verifies that the constructor stores
    /// the supplied exception message.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetMessage_When_MessageIsProvided()
    {
        // Arrange

        const string message =
            "Communication failed.";

        // Act

        CommunicationException exception =
            new(message);

        // Assert

        exception.Message
            .Should()
            .Be(message);
    }

    /// <summary>
    /// Verifies that the constructor does not assign
    /// an inner exception when only a message is supplied.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetInnerExceptionToNull_When_OnlyMessageIsProvided()
    {
        // Arrange

        const string message =
            "Communication failed.";

        // Act

        CommunicationException exception =
            new(message);

        // Assert

        exception.InnerException
            .Should()
            .BeNull();
    }

    /// <summary>
    /// Verifies that the constructor stores both
    /// the supplied message and inner exception.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetMessageAndInnerException_When_InnerExceptionIsProvided()
    {
        // Arrange

        const string message =
            "Communication failed.";

        InvalidOperationException innerException =
            new("Provider operation failed.");

        // Act

        CommunicationException exception =
            new(
                message,
                innerException);

        // Assert

        exception.Message
            .Should()
            .Be(message);

        exception.InnerException
            .Should()
            .BeSameAs(innerException);
    }

    // ==========================================================
    // Exception Inheritance
    // ==========================================================

    /// <summary>
    /// Verifies that CommunicationException derives
    /// from the standard Exception type.
    /// </summary>
    [Fact]
    public void CommunicationException_Should_DeriveFromException()
    {
        // Arrange

        CommunicationException exception =
            new("Communication failed.");

        // Act

        Type exceptionType =
            exception.GetType();

        // Assert

        exceptionType
            .Should()
            .Be(typeof(CommunicationException));

        exception
            .Should()
            .BeAssignableTo<Exception>();
    }
}