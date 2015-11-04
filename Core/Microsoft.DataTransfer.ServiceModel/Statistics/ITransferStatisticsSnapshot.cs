using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.ServiceModel.Statistics
{
    /// <summary>
    /// Contains snapshot of data transfer statistics.
    /// </summary>
    public interface ITransferStatisticsSnapshot
    {
        /// <summary>
        /// Gets the time spent in data transfer.
        /// </summary>
        TimeSpan ElapsedTime { get; }

        /// <summary>
        /// Gets the number of transferred data artifacts.
        /// </summary>
        int Transferred { get; }

        /// <summary>
        /// Gets the number of failed data artifacts.
        /// </summary>
        int Failed { get; }

        /// <summary>
        /// Gets the collection of failed data artifacts with error information.
        /// </summary>
        /// <returns>Collection of failed data artifacts.</returns>
        IReadOnlyCollection<KeyValuePair<string, string>> GetErrors();
    }
}
