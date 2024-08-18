using Microsoft.AspNetCore.Identity.UI.Services;

namespace Parkomat.Models
{
    /// <summary>
    /// Provides an implementation of the <see cref="IEmailSender"/> interface for sending emails.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Sends an email asynchronously. This is a placeholder implementation that does not send an actual email.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlMessage">The HTML content of the email message.</param>
        /// <returns>A completed task, representing a placeholder implementation.</returns>
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
