using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.ServiceModel.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.FactoryAdapters
{
    interface IDataAdapterFactoryAdapter<TDataAdapter> : IDataAdapterDefinition
    {
        Task<TDataAdapter> CreateAsync(object configuration, IDataTransferContext context, CancellationToken cancellation);
    }
}
