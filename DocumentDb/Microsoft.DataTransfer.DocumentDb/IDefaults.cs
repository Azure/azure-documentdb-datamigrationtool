using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Sink;
using System;

namespace Microsoft.DataTransfer.DocumentDb
{
    /// <summary>
    /// Contains default configuration for DocumentDB data adapters.
    /// </summary>
    public interface IDefaults
    {
        /// <summary>
        /// Gets the default DocumentDB connection mode.
        /// </summary>
        DocumentDbConnectionMode ConnectionMode { get; }

        /// <summary>
        /// Gets the default number of retries on transient failures.
        /// </summary>
        int NumberOfRetries { get; }

        /// <summary>
        /// Gets the default time interval between retries on transient failures.
        /// </summary>
        TimeSpan RetryInterval { get; }

        /// <summary>
        /// Gets the default collection throughput.
        /// </summary>
        int SinkCollectionThroughput { get; }

        /// <summary>
        /// Gets the default date and time handling strategy.
        /// </summary>
        DateTimeHandling SinkDateTimeHandling { get; }

        /// <summary>
        /// Gets the default bulk import stored procedure file name.
        /// </summary>
        string BulkSinkStoredProcFile { get; }

        /// <summary>
        /// Gets the default number of data artifacts bundled into one batch.
        /// </summary>
        int BulkSinkBatchSize { get; }

        /// <summary>
        /// Gets the default maximum size of the JSON payload that can be sent to the server.
        /// </summary>
        int BulkSinkMaxScriptSize { get; }

        /// <summary>
        /// Gets the default maximum number of parallel insert operations.
        /// </summary>
        int ParallelSinkNumberOfParallelRequests { get; }
    }
}
