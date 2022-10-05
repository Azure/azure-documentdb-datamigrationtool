using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.Interfaces
{
    public interface IDataSinkExtension : IDataTransferExtension
    {
        Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default);
    }
}
