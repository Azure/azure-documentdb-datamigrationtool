using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.ConsoleHost.Extensibility
{
    /// <summary>
    /// Creates an instance of known configuration types from command line arguments and
    /// provides configuration options description.
    /// </summary>
    public interface IDataAdapterConfigurationFactory
    {
        /// <summary>
        /// Creates an instance of known configuration types from command line arguments.
        /// </summary>
        /// <param name="configurationType">Required configuration type.</param>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns>New instance of configuration populated from command line arguments.</returns>
        object TryCreate(Type configurationType, IReadOnlyDictionary<string, string> arguments);

        /// <summary>
        /// Provides description for configuration options of known configuration types.
        /// </summary>
        /// <param name="configurationType">Type of configuration.</param>
        /// <returns>Description of available configuration options.</returns>
        IReadOnlyDictionary<string, string> TryGetConfigurationOptions(Type configurationType);
    }
}
