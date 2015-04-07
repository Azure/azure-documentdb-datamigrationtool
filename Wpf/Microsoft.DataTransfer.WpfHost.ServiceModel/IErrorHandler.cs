using System;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel
{
    /// <summary>
    /// Represents default error handling policy.
    /// </summary>
    public interface IErrorHandler
    {
        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="error">Exception to handle.</param>
        void Handle(Exception error);
    }
}
