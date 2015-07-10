using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Client.Enumeration;
using Microsoft.DataTransfer.DocumentDb.Source;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Source
{
    [TestClass]
    public class DocumentDbSourceAdapterTests : DataTransferTestBase
    {
        [TestMethod]
        public async Task ReadSampleData_AllDataRead()
        {
            const string TestCollectionName = "TestCollection";
            const string TestQuery = "SELECT * FROM Collection";

            const int NumberOfItems = 100;

            var sampleData = SampleData.GetSimpleDocuments(NumberOfItems);

            var client = Mocks
                    .Of<IDocumentDbReadClient>()
                    .Where(m => 
                        m.QueryDocumentsAsync(TestCollectionName, TestQuery, CancellationToken.None) ==
                            Task.FromResult<IAsyncEnumerator<IReadOnlyDictionary<string, object>>>(
                                new AsyncEnumeratorMock<Dictionary<string, object>>(sampleData.OfType<Dictionary<string, object>>().GetEnumerator())))
                    .First();

            var configuration = Mocks
                    .Of<IDocumentDbSourceAdapterInstanceConfiguration>()
                    .Where(m => 
                        m.Collection == TestCollectionName && 
                        m.Query == TestQuery)
                    .First();

            var readResults = new List<IDataItem>();
            using (var adapter = new DocumentDbSourceAdapter(client, PassThroughTransformation.Instance, configuration))
            {
                await adapter.InitializeAsync(CancellationToken.None);

                IDataItem dataItem;
                while ((dataItem = await adapter.ReadNextAsync(ReadOutputByRef.None, CancellationToken.None)) != null)
                    readResults.Add(dataItem);
            }

            DataItemCollectionAssert.AreEquivalent(sampleData, readResults, TestResources.InvalidDataItemsTransferred);
        }
    }
}
