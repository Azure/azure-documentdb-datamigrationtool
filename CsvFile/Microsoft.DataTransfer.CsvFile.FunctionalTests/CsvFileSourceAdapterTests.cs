using Microsoft.DataTransfer.CsvFile.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.CsvFile.FunctionalTests
{
    [TestClass]
    public class CsvFileSourceAdapterTests
    {
        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\BasicTest.csv", @"InputData")]
        public async Task ReadFlatDocuments_AllDataRead()
        {
            var configuration = Mocks
                    .Of<ICsvFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { @"InputData\BasicTest.csv" })
                    .First();

            var records = await CsvFileHelper.ReadCsv(configuration);

            Assert.AreEqual(3, records.Count, TestResources.UnexpectedRecordsProcessed);
            Assert.AreEqual(6, records[0].GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\MalformedTest.csv", @"InputData")]
        public async Task ReadMalformedDocuments_MalformedDataIgnored()
        {
            var configuration = Mocks
                    .Of<ICsvFileSourceAdapterConfiguration>(c => 
                        c.Files == new[] { @"InputData\MalformedTest.csv" })
                    .First();

            var records = await CsvFileHelper.ReadCsv(configuration);

            Assert.AreEqual(3, records.Count, TestResources.UnexpectedRecordsProcessed);
            Assert.AreEqual(6, records[0].GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
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

            var records = await CsvFileHelper.ReadCsv(configuration);

            Assert.AreEqual(4, records.Count, TestResources.UnexpectedRecordsProcessed);

            var fields = records[0].GetFieldNames().ToArray();

            Assert.AreEqual(6, fields.Count(), TestResources.UnexpectedFieldsProcessed);
            CollectionAssert.Contains(fields, NestedDocumentFieldName, TestResources.NestedDocumentFieldNotProcessed);

            var nestedDocument = records[0].GetValue(NestedDocumentFieldName) as IDataItem;

            Assert.IsNotNull(nestedDocument, TestResources.NestedDocumentFieldNotProcessed);
            Assert.AreEqual(2, nestedDocument.GetFieldNames().Count(), TestResources.UnexpectedFieldsProcessed);
        }
    }
}
