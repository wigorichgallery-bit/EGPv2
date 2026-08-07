using Microsoft.Graph.Users.Item.SendMail;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Provides an abstraction over the Microsoft Graph SDK.
/// </summary>
internal interface IGraphSdkClient
{
    /// <summary>
    /// Sends an email message using Microsoft Graph.
    /// </summary>
    /// <param name="userId">
    /// The Microsoft Graph user identifier that sends the email.
    /// </param>
    /// <param name="request">
    /// The send mail request.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="userId"/> is empty
    /// or whitespace.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="request"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is cancelled.
    /// </exception>
    Task SendMailAsync(
        string userId,
        SendMailPostRequestBody request,
        CancellationToken cancellationToken = default);
}