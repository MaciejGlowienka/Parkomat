namespace Parkomat.Models
{

    /// <summary>
    /// Represents the model for displaying error information in the view.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request ID associated with the error.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request ID should be shown.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the <see cref="RequestId"/> is not null or empty; otherwise, <c>false</c>.
        /// </returns>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
