using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.Core.Service
{
    sealed class DataTransferContext : IDataTransferContext
    {
        public string SourceName { get; set; }
        public string SinkName { get; set; }
    }
}
