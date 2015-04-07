using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.TestsCommon.Mocks
{
    public sealed class DataTransferContextMock : IDataTransferContext
    {
        public static readonly DataTransferContextMock Instance = new DataTransferContextMock();

        public string SourceName
        {
            get { return "TestSource"; }
        }

        public string SinkName
        {
            get { return "TestSink"; }
        }
    }
}
