using Microsoft.DataTransfer.ServiceModel.Errors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ServiceModel.Statistics
{
    /// <summary>
    /// Provides data transfer statistics.
    /// </summary>
    public interface ITransferStatisticsFactory
    {
        /// <summary>
        /// Creates a new instance of data transfer statistics.
        /// </summary>
        /// <param name="errorDetailsProvider">An instance of <see cref="IErrorDetailsProvider" /> to use to extract error details.</param>
        /// <param name="configuration">Configuration for the data transfer statistics.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        Task<ITransferStatistics> Create(IErrorDetailsProvider errorDetailsProvider, ITransferStatisticsConfiguration configuration,
            CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorDetailsProvider"></param>
        /// <param name="configuration"></param>
        /// <param name="destConfiguration"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<ITransferStatistics> Create(IErrorDetailsProvider errorDetailsProvider, ITransferStatisticsConfiguration configuration,
            IReadOnlyDictionary<string, string> destConfiguration, CancellationToken cancellation);

    }
}
