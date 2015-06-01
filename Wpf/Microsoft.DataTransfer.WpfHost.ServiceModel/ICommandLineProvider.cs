using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel
{
    /// <summary>
    /// Provides command line for the console version of the import tool.
    /// </summary>
    public interface ICommandLineProvider
    {
        /// <summary>
        /// Generates a command line based on the provided configuration.
        /// </summary>
        /// <param name="infrastructureConfiguration">Configuration for the data transfer process infrastructure.</param>
        /// <param name="sourceName">Name of the data source adapter.</param>
        /// <param name="sourceArguments">Configuration arguments for the data source adapter.</param>
        /// <param name="sinkName">Name of the data sink adapter.</param>
        /// <param name="sinkArguments">Configuration arguments for the data sink adapter.</param>
        /// <returns>Command line arguments string.</returns>
        string Get(IInfrastructureConfiguration infrastructureConfiguration,
            string sourceName, IReadOnlyDictionary<string, string> sourceArguments,
            string sinkName, IReadOnlyDictionary<string, string> sinkArguments);
    }
}
