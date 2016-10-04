using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Sink;
using System;

namespace Microsoft.DataTransfer.DocumentDb
{
    /// <summary>
    /// Contains default configuration for DocumentDB data adapters.
    /// </summary>
    public static class Defaults
    {
        private static object updateLock = new Object();
        private static IDefaults current;

        /// <summary>
        /// Gets current default configuration for DocumentDB data adapters.
        /// </summary>
        public static IDefaults Current
        {
            get { return GetCurrent(); }
        }

        private static IDefaults GetCurrent()
        {
            if (current == null) lock (updateLock) if (current == null)
                        current = new LibraryDefaults();

            return current;
        }

        /// <summary>
        /// Changes default configuration for DocumentDB data adapters.
        /// </summary>
        /// <param name="defaults">New default configuration.</param>
        public static void SetCurrent(IDefaults defaults)
        {
            Guard.NotNull("defaults", defaults);

            lock (updateLock)
                current = defaults;
        }

        private sealed class LibraryDefaults : IDefaults
        {
            public DocumentDbConnectionMode ConnectionMode { get { return DocumentDbConnectionMode.DirectTcp; } }

            public int NumberOfRetries { get { return 30; } }
            public TimeSpan RetryInterval { get { return TimeSpan.FromSeconds(1); } }

            public int SinkCollectionThroughput { get { return 1000; } }
            public DateTimeHandling SinkDateTimeHandling { get { return DateTimeHandling.String; } }

            public int BulkSinkBatchSize { get { return 50; } }
            public int BulkSinkMaxScriptSize { get { return 512 * 1024 - 10; } } // Allow 10 bytes for additional stored procedure overhead
            public string BulkSinkStoredProcFile { get { return "BulkInsert.js"; } }

            public int ParallelSinkNumberOfParallelRequests { get { return 10; } }
        }
    }
}
