using System;

namespace Microsoft.DataTransfer.ServiceModel.Statistics
{
    /// <summary>
    /// Maintains data transfer process statistics.
    /// </summary>
    public interface ITransferStatistics
    {
        /// <summary>
        /// Starts measuring elapsed time.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops measuring elapsed time.
        /// </summary>
        void Stop();

        /// <summary>
        /// Reports one transferred data artifact.
        /// </summary>
        void AddTransferred();

        /// <summary>
        /// Reports one failed data artifact.
        /// </summary>
        /// <param name="dataItemId">Identifier of the failed data artifact.</param>
        /// <param name="error">Transfer error.</param>
        void AddError(string dataItemId, Exception error);

        /// <summary>
        /// Takes the snapshot of current transfer statistics.
        /// </summary>
        /// <returns>Transfer statistics snapshot.</returns>
        ITransferStatisticsSnapshot GetSnapshot();
    }
}
