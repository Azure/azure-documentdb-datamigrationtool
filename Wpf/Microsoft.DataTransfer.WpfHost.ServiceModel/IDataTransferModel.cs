using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;
using System.ComponentModel;
using System.Threading;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel
{
    /// <summary>
    /// Contains data that defines current import configuration and state. 
    /// </summary>
    public interface IDataTransferModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the configuration for data transfer process infrastructure.
        /// </summary>
        IInfrastructureConfiguration InfrastructureConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the name of the data source adapter.
        /// </summary>
        string SourceAdapterName { get; set; }

        /// <summary>
        /// Gets or sets the current data source adapter configuration.
        /// </summary>
        object SourceConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the name of the data sink adapter.
        /// </summary>
        string SinkAdapterName { get; set; }

        /// <summary>
        /// Gets or sets the current data sink adapter configuration.
        /// </summary>
        object SinkConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the value indicating that import was already started once.
        /// </summary>
        bool HasImportStarted { get; set; }

        /// <summary>
        /// Gets or sets the cancellation source of <see cref="CancellationToken" /> used in current data transfer process.
        /// </summary>
        /// <remarks>
        /// This property can be null, indicating that import is not running.
        /// </remarks>
        CancellationTokenSource ImportCancellation { get; set; }
    }
}
