namespace Platform.Communication.Exceptions;

/// <summary>
/// Represents an error that occurs while communicating
/// with an external communication provider.
/// </summary>
public sealed class CommunicationException
    : Exception
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CommunicationException"/> class.
    /// </summary>
    /// <param name="message">
    /// The exception message.
    /// </param>
    public CommunicationException(
        string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CommunicationException"/> class.
    /// </summary>
    /// <param name="message">
    /// The exception message.
    /// </param>
    /// <param name="innerException">
    /// The underlying exception.
    /// </param>
    public CommunicationException(
        string message,
        Exception innerException)
        : base(
            message,
            innerException)
    {
    }
}