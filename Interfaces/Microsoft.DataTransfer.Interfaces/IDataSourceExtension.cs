using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.Interfaces
{
    public interface IDataSourceExtension : IDataTransferExtension
    {
        IAsyncEnumerable<IDataItem> ReadAsync(IConfiguration config, CancellationToken cancellationToken = default);
    }
}
