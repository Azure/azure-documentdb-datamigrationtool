using Microsoft.DataTransfer.ServiceModel.Errors;
using System;

namespace Microsoft.DataTransfer.ServiceModel
{
    /// <summary>
    /// Contains default configuration for data transfer infrastructure.
    /// </summary>
    public interface IInfrastructureDefaults
    {
        /// <summary>
        /// Gets the default data transfer error details configuration.
        /// </summary>
        ErrorDetails ErrorDetails { get; }

        /// <summary>
        /// Gets the default data transfer progress update interval.
        /// </summary>
        TimeSpan ProgressUpdateInterval { get; }
    }
}
