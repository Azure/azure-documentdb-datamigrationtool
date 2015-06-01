using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.DocumentDb.Shared
{
    /// <summary>
    /// Contains basic configuration for DocumentDB data adapters.
    /// </summary>
    public interface IDocumentDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string for the DocumentDB account.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ConnectionString")]
        string ConnectionString { get; }

        /// <summary>
        /// Gets the DocumentDB connection mode.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "ConnectionMode")]
        DocumentDbConnectionMode? ConnectionMode { get; }

        /// <summary>
        /// Gets the number of retries to perform on transient failures.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "Retries")]
        int? Retries { get; }

        /// <summary>
        /// Gets the time interval between retries on transient failures.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "RetryInterval")]
        TimeSpan? RetryInterval { get; }
    }
}
