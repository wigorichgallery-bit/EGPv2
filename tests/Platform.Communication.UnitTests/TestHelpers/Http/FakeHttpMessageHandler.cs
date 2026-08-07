using System.Net;
using System.Net.Http;

namespace Platform.Communication.UnitTests.TestHelpers.Http;

/// <summary>
/// Provides a configurable HTTP message handler for unit testing.
/// </summary>
internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="FakeHttpMessageHandler"/> class.
    /// </summary>
    /// <param name="handler">
    /// Delegate used to produce the HTTP response.
    /// </param>
    public FakeHttpMessageHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        _handler = handler;
    }

    /// <inheritdoc/>
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return _handler(
            request,
            cancellationToken);
    }

    /// <summary>
    /// Creates a successful JSON response.
    /// </summary>
    public static HttpResponseMessage Ok(
        string json)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                json,
                System.Text.Encoding.UTF8,
                "application/json")
        };
    }

    /// <summary>
    /// Creates an HTTP error response.
    /// </summary>
    public static HttpResponseMessage Error(
        HttpStatusCode statusCode)
    {
        return new HttpResponseMessage(statusCode);
    }
}