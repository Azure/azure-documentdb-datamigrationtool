using System;

namespace Microsoft.DataTransfer.ServiceModel.Errors
{
    /// <summary>
    /// Provides the error details.
    /// </summary>
    public interface IErrorDetailsProvider
    {
        /// <summary>
        /// Provides the details for non-critical data transfer error.
        /// </summary>
        /// <param name="error">An <see cref="Exception" /> that defines non-critical error.</param>
        /// <returns>Error details.</returns>
        string Get(Exception error);

        /// <summary>
        /// Provides the details for critical data transfer error.
        /// </summary>
        /// <param name="error">An <see cref="Exception" /> that defines critical error.</param>
        /// <returns>Error details.</returns>
        string GetCritical(Exception error);
    }
}
