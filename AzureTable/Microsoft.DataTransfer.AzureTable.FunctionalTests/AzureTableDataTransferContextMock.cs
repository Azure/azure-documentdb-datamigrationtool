using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.AzureTable.FunctionalTests
{
    sealed class AzureTableDataTransferContextMock : IDataTransferContext
    {
        public string SourceName { get; set; }

        public string SinkName { get; set; }

        public string RunConfigSignature { get; set; }

        public bool EnableResumeFunction { get; set; }
    }
}
