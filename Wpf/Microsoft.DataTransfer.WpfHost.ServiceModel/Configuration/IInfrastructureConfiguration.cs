using Microsoft.DataTransfer.ServiceModel.Errors;
using Microsoft.DataTransfer.ServiceModel.Statistics;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration
{
    /// <summary>
    /// Configuration for data transfer process infrastructure.
    /// </summary>
    public interface IInfrastructureConfiguration : ITransferStatisticsConfiguration, IErrorDetailsConfiguration { }
}
