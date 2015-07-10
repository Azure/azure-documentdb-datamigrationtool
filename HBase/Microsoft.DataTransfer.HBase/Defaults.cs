using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.HBase
{
    /// <summary>
    /// Contains default configuration for HBase data adapters.
    /// </summary>
    public static class Defaults
    {
        private static object updateLock = new Object();
        private static IDefaults current;

        /// <summary>
        /// Gets current default configuration for HBase data adapters.
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
        /// Changes default configuration for HBase data adapters.
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
            public int SourceBatchSize { get { return 100; } }
        }
    }
}
