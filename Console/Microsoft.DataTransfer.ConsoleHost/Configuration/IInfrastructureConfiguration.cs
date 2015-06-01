using Microsoft.DataTransfer.ServiceModel.Statistics;

namespace Microsoft.DataTransfer.ConsoleHost.Configuration
{
    /// <summary>
    /// Configuration for data transfer process infrastructure.
    /// </summary>
    // This needs to be public to allow automatic proxy class generation.
    public interface IInfrastructureConfiguration : ITransferStatisticsConfiguration { }
}
