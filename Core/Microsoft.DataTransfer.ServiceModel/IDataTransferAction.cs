using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ServiceModel
{
    /// <summary>
    /// Represents data transfer action.
    /// </summary>
    public interface IDataTransferAction
    {
        /// <summary>
        /// Performs data transfer from the provided source to the provided sink.
        /// </summary>
        /// <param name="source">Source data adapter to read data from.</param>
        /// <param name="sink">Sink data adapter to write data to.</param>
        /// <param name="statistics">Instance of <see cref="ITransferStatistics" /> to report data transfer progress to.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous data transfer operation.</returns>
        Task ExecuteAsync(IDataSourceAdapter source, IDataSinkAdapter sink, ITransferStatistics statistics, CancellationToken cancellation);
    }
}
