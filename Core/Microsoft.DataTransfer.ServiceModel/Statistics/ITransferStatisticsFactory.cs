
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
        /// <returns>New instance of data transfer statistics.</returns>
        ITransferStatistics Create(ITransferStatisticsConfiguration configuration);
    }
}
