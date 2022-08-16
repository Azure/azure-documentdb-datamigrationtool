using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.Interfaces
{
    public interface IDataTransferExtension
    {
        string DisplayName { get; }
        IAsyncEnumerable<IDataItem> ReadAsSourceAsync(CancellationToken cancellationToken = default);
        Task WriteAsSinkAsync(IAsyncEnumerable<IDataItem> dataItems, CancellationToken cancellationToken = default);
        Task Configure(IConfiguration configuration);
    }
}
