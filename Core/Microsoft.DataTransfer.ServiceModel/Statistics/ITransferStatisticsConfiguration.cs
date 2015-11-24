using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.ServiceModel.Statistics
{
    /// <summary>
    /// Configuration for data transfer process statistics.
    /// </summary>
    public interface ITransferStatisticsConfiguration
    {
        /// <summary>
        /// Gets the name of the file to persist data transfer failures.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Statistics_ErrorLog")]
        string ErrorLog { get; }

        /// <summary>
        /// Gets the value that indicates whether error log file can be overwritten.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Statistics_OverwriteErrorLog")]
        bool OverwriteErrorLog { get; }

        /// <summary>
        /// Gets the data transfer progress update interval.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "Statistics_ProgressUpdateInterval")]
        TimeSpan? ProgressUpdateInterval { get; }
    }
}
