using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Source;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
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
                    .Where(m => m.QueryDocuments(TestCollectionName, TestQuery) == sampleData)
                    .First();

            var configuration = Mocks
                    .Of<IDocumentDbSourceAdapterInstanceConfiguration>()
                    .Where(m => 
                        m.CollectionName == TestCollectionName && 
                        m.Query == TestQuery)
                    .First();

            var readResults = new List<IDataItem>();
            using (var adapter = new DocumentDbSourceAdapter(client, PassThroughTransformation.Instance, configuration))
            {
                adapter.Initialize();

                IDataItem dataItem;
                while ((dataItem = await adapter.ReadNextAsync(ReadOutputByRef.None, CancellationToken.None)) != null)
                    readResults.Add(dataItem);
            }

            DataItemCollectionAssert.AreEquivalent(sampleData, readResults, TestResources.InvalidDataItemsTransferred);
        }
    }
}
