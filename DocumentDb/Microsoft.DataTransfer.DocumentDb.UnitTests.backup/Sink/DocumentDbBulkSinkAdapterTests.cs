using Microsoft.DataTransfer.DocumentDb.Sink.Bulk;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Sink
{
    [TestClass]
    public class DocumentDbBulkSinkAdapterTests
    {
        [TestMethod]
        public async Task CompleteAsyncOrDispose_CleansUpAfterTransfer()
        {
            var clientMock = new DocumentDbWriteClientMock();

            var configurationMock = Mocks
                    .Of<IDocumentDbBulkSinkAdapterInstanceConfiguration>(m => 
                        m.Collection == "TestCollection" &&
                        m.StoredProcName == "test" &&
                        m.BatchSize == 5 &&
                        m.MaxScriptSize == 1024)
                    .First();

            using (var adapter = new DocumentDbBulkSinkAdapter(clientMock, PassThroughTransformation.Instance, configurationMock))
            {
                await adapter.InitializeAsync(CancellationToken.None);
                await adapter.CompleteAsync(CancellationToken.None);
            }

            CollectionAssert.AreEquivalent(clientMock.CreatedStoredProcedures.ToArray(), clientMock.DeletedStoredProcedures.ToArray(),
                TestResources.BulkSinkAdapterDidNotCleanup);
        }
    }
}
