using Microsoft.DataTransfer.ServiceModel.Entities;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ServiceModel
{
    /// <summary>
    /// Represents core data transfer service.
    /// </summary>
    public interface IDataTransferService
    {
        /// <summary>
        /// Gets the list of known data source adapters.
        /// </summary>
        /// <returns>Collection of known data sources.</returns>
        IReadOnlyDictionary<string, IDataAdapterDefinition> GetKnownSources();

        /// <summary>
        /// Gets the list of known data sink adapters.
        /// </summary>
        /// <returns>Collection of known data sinks.</returns>
        IReadOnlyDictionary<string, IDataAdapterDefinition> GetKnownSinks();

        /// <summary>
        /// Performs data transfer from the specified source to the specified sink.
        /// </summary>
        /// <param name="sourceName">Name of the source data adapter.</param>
        /// <param name="sourceConfiguration">Source data adapter configuration.</param>
        /// <param name="sinkName">Name of the target data adapter.</param>
        /// <param name="sinkConfiguration">Target data adapter configuration.</param>
        /// <param name="statistics">Instance of <see cref="ITransferStatistics" /> to report data transfer progress to.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous data transfer operation.</returns>
        Task TransferAsync(
            string sourceName, object sourceConfiguration,
            string sinkName, object sinkConfiguration,
            ITransferStatistics statistics,
            CancellationToken cancellation);
    }
}
