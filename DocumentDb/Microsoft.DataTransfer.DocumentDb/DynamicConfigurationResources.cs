using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Sink;
using System;
using System.Globalization;

namespace Microsoft.DataTransfer.DocumentDb
{
    /// <summary>
    /// Contains dynamic resources for data adapters configuration.
    /// </summary>
    public sealed class DynamicConfigurationResources : DynamicResourcesBase
    {
        /// <summary>
        /// Gets the description for connection mode configuration property.
        /// </summary>
        public static string ConnectionMode
        {
            get { return Format(ConfigurationResources.ConnectionModeFormat, Defaults.Current.ConnectionMode,
                    String.Join(", ", Enum.GetNames(typeof(DocumentDbConnectionMode)))); }
        }

        /// <summary>
        /// Gets the description for retries configuration property.
        /// </summary>
        public static string Retries
        {
            get { return Format(ConfigurationResources.RetriesFormat, Defaults.Current.NumberOfRetries); }
        }

        /// <summary>
        /// Gets the description for retry interval configuration property.
        /// </summary>
        public static string RetryInterval
        {
            get { return Format(ConfigurationResources.RetryIntervalFormat, Defaults.Current.RetryInterval); }
        }

        /// <summary>
        /// Gets the description for collection throughput configuration property.
        /// </summary>
        public static string Sink_CollectionThroughput
        {
            get { return Format(ConfigurationResources.Sink_CollectionThroughputFormat, Defaults.Current.SinkCollectionThroughput); }
        }

        /// <summary>
        /// Gets the description for date and time handling configuration property.
        /// </summary>
        public static string Sink_Dates
        {
            get { return Format(ConfigurationResources.Sink_DatesFormat, Defaults.Current.SinkDateTimeHandling,
                String.Join(", ", Enum.GetNames(typeof(DateTimeHandling)))); }
        }

        /// <summary>
        /// Gets the description for batch size configuration property.
        /// </summary>
        public static string BulkSink_BatchSize
        {
            get { return Format(ConfigurationResources.BulkSink_BatchSizeFormat, Defaults.Current.BulkSinkBatchSize); }
        }

        /// <summary>
        /// Gets the description for max script size configuration property.
        /// </summary>
        public static string BulkSink_MaxScriptSize
        {
            get { return Format(ConfigurationResources.BulkSink_MaxScriptSizeFormat, Defaults.Current.BulkSinkMaxScriptSize); }
        }

        /// <summary>
        /// Gets the description for bulk insert stored procedure file configuration property.
        /// </summary>
        public static string BulkSink_StoredProcFile
        {
            get { return Format(ConfigurationResources.BulkSink_StoredProcFileFormat, Defaults.Current.BulkSinkStoredProcFile); }
        }

        /// <summary>
        /// Gets the description for parallel requests configuration property.
        /// </summary>
        public static string ParallelSink_ParallelRequests
        {
            get { return Format(ConfigurationResources.ParallelSink_ParallelRequestsFormat, Defaults.Current.ParallelSinkNumberOfParallelRequests); }
        }

        private DynamicConfigurationResources() { }
    }
}
