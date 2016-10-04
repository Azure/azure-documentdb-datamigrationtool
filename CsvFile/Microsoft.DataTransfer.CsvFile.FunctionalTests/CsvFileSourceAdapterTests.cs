using Microsoft.DataTransfer.CsvFile.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.CsvFile.FunctionalTests
{
    [TestClass]
    public class CsvFileSourceAdapterTests : DataTransferAdapterTestBase
    {
        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\BasicTest.csv", @"InputData")]
        public async Task ReadFlatDocuments_AllDataRead()
        {
            var configuration = Mocks
                    .Of<ICsvFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { @"InputData\BasicTest.csv" })
                    .First();

            var readResults = await ReadData(configuration);

            Assert.AreEqual(3, readResults.Count, TestResources.UnexpectedRecordsProcessed);
            Assert.AreEqual(6, readResults[0].GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\MalformedTest.csv", @"InputData")]
        public async Task ReadMalformedDocuments_MalformedDataIgnored()
        {
            var configuration = Mocks
                    .Of<ICsvFileSourceAdapterConfiguration>(c => 
                        c.Files == new[] { @"InputData\MalformedTest.csv" })
                    .First();

            var readResults = await ReadData(configuration);

            Assert.AreEqual(3, readResults.Count, TestResources.UnexpectedRecordsProcessed);
            Assert.AreEqual(6, readResults[0].GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\NestedDocumentsTest.csv", @"InputData")]
        public async Task ReadNestedDocuments_AllDataRead()
        {
            const string NestedDocumentFieldName = "Manager";

            var configuration = Mocks
                    .Of<ICsvFileSourceAdapterConfiguration>(c => 
                        c.Files == new[] { @"InputData\NestedDocumentsTest.csv" } &&
                        c.NestingSeparator == ".")
                    .First();

            var readResults = await ReadData(configuration);

            Assert.AreEqual(4, readResults.Count, TestResources.UnexpectedRecordsProcessed);

            var fields = readResults[0].GetFieldNames().ToArray();

            Assert.AreEqual(6, fields.Count(), TestResources.UnexpectedFieldsProcessed);
            CollectionAssert.Contains(fields, NestedDocumentFieldName, TestResources.NestedDocumentFieldNotProcessed);

            var nestedDocument = readResults[0].GetValue(NestedDocumentFieldName) as IDataItem;

            Assert.IsNotNull(nestedDocument, TestResources.NestedDocumentFieldNotProcessed);
            Assert.AreEqual(2, nestedDocument.GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\CustomCultureTest.csv", @"InputData")]
        public async Task ReadFlatDocuments_CustomCulture_AllDataRead()
        {
            var customCulture = new CultureInfo(CultureInfo.InvariantCulture.Name);
            customCulture.TextInfo.ListSeparator = ";";

            CultureInfo.DefaultThreadCurrentCulture = customCulture;

            var configuration = Mocks
                    .Of<ICsvFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { @"InputData\CustomCultureTest.csv" } &&
                        c.UseRegionalSettings == true)
                    .First();

            var readResults = await ReadData(configuration);

            Assert.AreEqual(3, readResults.Count, TestResources.UnexpectedRecordsProcessed);
            Assert.AreEqual(6, readResults[0].GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\BasicTest.csv", @"InputData")]
        public async Task ReadFlatDocuments_UseInvariantCultureWhenCustomCultureIsSet_AllDataRead()
        {
            var customCulture = new CultureInfo(CultureInfo.InvariantCulture.Name);
            customCulture.TextInfo.ListSeparator = ";";

            CultureInfo.DefaultThreadCurrentCulture = customCulture;

            var configuration = Mocks
                    .Of<ICsvFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { @"InputData\BasicTest.csv" })
                    .First();

            var readResults = await ReadData(configuration);

            Assert.AreEqual(3, readResults.Count, TestResources.UnexpectedRecordsProcessed);
            Assert.AreEqual(6, readResults[0].GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\BasicTest.gz", @"InputData")]
        public async Task ReadFlatDocumentsFromCompressedFile_AllDataRead()
        {
            var configuration = Mocks
                    .Of<ICsvFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { @"InputData\BasicTest.gz" } &&
                        c.Decompress == true)
                    .First();

            var readResults = await ReadData(configuration);

            Assert.AreEqual(3, readResults.Count, TestResources.UnexpectedRecordsProcessed);
            Assert.AreEqual(6, readResults[0].GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
        }

        private async Task<List<IDataItem>> ReadData(ICsvFileSourceAdapterConfiguration configuration)
        {
            using (var adapter = await new CsvFileSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                return await ReadDataAsync(adapter);
            }
        }
    }
}
