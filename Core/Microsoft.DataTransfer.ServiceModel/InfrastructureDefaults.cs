using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.ServiceModel.Errors;
using System;

namespace Microsoft.DataTransfer.ServiceModel
{
    /// <summary>
    /// Contains default configuration for data transfer infrastructure.
    /// </summary>
    public static class InfrastructureDefaults
    {
        private static object updateLock = new Object();
        private static IInfrastructureDefaults current;

        /// <summary>
        /// Gets current default configuration for data transfer infrastructure.
        /// </summary>
        public static IInfrastructureDefaults Current
        {
            get { return GetCurrent(); }
        }

        private static IInfrastructureDefaults GetCurrent()
        {
            if (current == null) lock (updateLock) if (current == null)
                current = new LibraryDefaults();

            return current;
        }

        /// <summary>
        /// Changes default configuration for data transfer infrastructure.
        /// </summary>
        /// <param name="defaults">New default configuration.</param>
        public static void SetCurrent(IInfrastructureDefaults defaults)
        {
            Guard.NotNull("defaults", defaults);

            lock (updateLock)
                current = defaults;
        }

        private sealed class LibraryDefaults : IInfrastructureDefaults
        {
            public ErrorDetails ErrorDetails { get { return ErrorDetails.None; } }

            public TimeSpan ProgressUpdateInterval { get { return TimeSpan.FromSeconds(1); } }
        }
    }
}
