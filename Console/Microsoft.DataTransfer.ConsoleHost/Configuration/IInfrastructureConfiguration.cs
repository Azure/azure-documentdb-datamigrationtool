using Microsoft.DataTransfer.ServiceModel.Errors;
using Microsoft.DataTransfer.ServiceModel.Statistics;

namespace Microsoft.DataTransfer.ConsoleHost.Configuration
{
    /// <summary>
    /// Configuration for data transfer process infrastructure.
    /// </summary>
    /// <remarks>
    /// This needs to be public to allow automatic proxy class generation.
    /// </remarks>
    public interface IInfrastructureConfiguration : ITransferStatisticsConfiguration, IErrorDetailsConfiguration { }
}
