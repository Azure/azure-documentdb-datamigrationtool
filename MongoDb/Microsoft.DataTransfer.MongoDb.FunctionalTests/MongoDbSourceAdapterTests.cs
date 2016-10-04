using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.MongoDb.Source.Online;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.FunctionalTests
{
    [TestClass]
    public class MongoDbSourceAdapterTests : DataTransferAdapterTestBase
    {
        private const string CollectionNamePrefix = "TestCollection";

        private IMongoDbSourceAdapterConfiguration Configuration;
        private IMongoDatabase Database;
        private string CollectionName;
        private IMongoCollection<BsonDocument> Collection;

        [TestInitialize]
        public void Initialize()
        {
            CollectionName = String.Format(CultureInfo.InvariantCulture, "{0}_{1:N}", CollectionNamePrefix, Guid.NewGuid());

            Configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName)
                    .First();

            Database = new MongoClient(Configuration.ConnectionString)
                .GetDatabase(new MongoUrl(Configuration.ConnectionString).DatabaseName);

            Collection = Database.GetCollection<BsonDocument>(Configuration.Collection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.DropCollection(CollectionName);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSampleData_InclusiveProjectionApplied()
        {
            var expectedFields = new[] { "_id", "DateTimeProperty", "FloatProperty" };

            await Collection.InsertOneAsync(
                new BsonDocument(SampleData.GetSimpleDocuments(1)[0]));

            var configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName &&
                        c.Projection == "{DateTimeProperty: 1, FloatProperty: true}")
                    .First();

            using (var adapter = await new MongoDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                var dataItem = await adapter.ReadNextAsync(ReadOutputByRef.None, CancellationToken.None);

                Assert.IsNotNull(dataItem, TestResources.InvalidDataItem);

                CollectionAssert.AreEquivalent(expectedFields, dataItem.GetFieldNames().ToArray(), TestResources.InvalidDataItem);
            }
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSampleData_ExclusiveProjectionApplied()
        {
            var excludedProperties = new[] { "DateTimeProperty", "FloatProperty" };

            var document = SampleData.GetSimpleDocuments(1)[0];
            await Collection.InsertOneAsync(new BsonDocument(document));

            var configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName &&
                        c.Projection == "{DateTimeProperty: 0, FloatProperty: false}")
                    .First();

            using (var adapter = await new MongoDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                var dataItem = await adapter.ReadNextAsync(ReadOutputByRef.None, CancellationToken.None);

                Assert.IsNotNull(dataItem, TestResources.InvalidDataItem);

                var actualFields = dataItem.GetFieldNames().Except(new[] { "_id" }).ToList();
                Assert.AreEqual(document.Count - 2, actualFields.Count, TestResources.InvalidDataItem);

                foreach (var excludedProperty in excludedProperties)
                    Assert.IsFalse(actualFields.Contains(excludedProperty), TestResources.UnexpectedPropertyFormat, excludedProperty);
            }
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSimpleDocuments_AllFieldsRead()
        {
            var documents = SampleData.GetSimpleDocuments(5);

            foreach (var document in documents)
            {
                var bsonDocument = new BsonDocument(document);
                await Collection.InsertOneAsync(bsonDocument);
                document["_id"] = bsonDocument["_id"].ToString();
            }

            List<IDataItem> readResults;
            using (var adapter = await new MongoDbSourceAdapterFactory()
                .CreateAsync(Configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                readResults = await ReadDataAsync(adapter);
            }

            DataItemCollectionAssert.AreEquivalent(documents, readResults, TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSimpleDocuments_FilterIntegerProperty_MatchingDataRead()
        {
            var documents = SampleData.GetSimpleDocuments(5);

            foreach (var document in documents)
            {
                var bsonDocument = new BsonDocument(document);
                await Collection.InsertOneAsync(bsonDocument);
                document["_id"] = bsonDocument["_id"].ToString();
            }

            var configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName &&
                        c.Query == "{ IntegerProperty: { $gt: 2 } }")
                    .First();

            List<IDataItem> readResults;
            using (var adapter = await new MongoDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                readResults = await ReadDataAsync(adapter);
            }

            DataItemCollectionAssert.AreEquivalent(
                documents.Where(d => (int)d["IntegerProperty"] > 2),
                readResults,
                TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadDocumentWithNonJsonProperties_AllFieldsRead()
        {
            var jScriptValue = "var i=0;";
            var longValue = 9223372036854775797L;
            var byteArrayValue = new byte[] { 0xef, 0x23, 0x10, 0xaa };
            var guidValue = Guid.NewGuid();
            var symbolValue = "testSymbol";

            await Collection.InsertOneAsync(new BsonDocument
            {
                { "script", new BsonJavaScript(jScriptValue) },
                { "regex", new BsonRegularExpression("ab*", "si") },
                { "maxKey", BsonMaxKey.Value },
                { "longVal", new BsonInt64(9223372036854775797L) },
                { "byteArr", byteArrayValue },
                { "guid", guidValue },
                { "scopedJscript", new BsonJavaScriptWithScope(jScriptValue, new BsonDocument { { "i", 1 } }) },
                { "timestamp", new BsonTimestamp(1424288963, 32) },
                { "symbol", BsonSymbolTable.Lookup(symbolValue) }
            });

            using (var adapter = await new MongoDbSourceAdapterFactory()
                .CreateAsync(Configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                var dataItem = await adapter.ReadNextAsync(ReadOutputByRef.None, CancellationToken.None);

                Assert.AreEqual(jScriptValue, dataItem.GetValue("script"), TestResources.InvalidPropertyValueFormat, "script");
                Assert.AreEqual("/ab*/si", dataItem.GetValue("regex"), TestResources.InvalidPropertyValueFormat, "regex");
                Assert.IsNull(dataItem.GetValue("maxKey"), TestResources.InvalidPropertyValueFormat, "maxKey");
                Assert.AreEqual(longValue, dataItem.GetValue("longVal"), TestResources.InvalidPropertyValueFormat, "longVal");

                CollectionAssert.AreEqual(byteArrayValue, (byte[])dataItem.GetValue("byteArr"), TestResources.InvalidPropertyValueFormat, "longVal");

                Assert.AreEqual(guidValue, dataItem.GetValue("guid"), TestResources.InvalidPropertyValueFormat, "guid");

                var scopedJsReader = dataItem.GetValue("scopedJscript") as IDataItem;
                Assert.IsNotNull(scopedJsReader, TestResources.InvalidPropertyValueFormat, "scopedJscript");
                Assert.AreEqual(jScriptValue, scopedJsReader.GetValue("code"), TestResources.InvalidPropertyValueFormat, "code");

                var scopedJsScopeReader = scopedJsReader.GetValue("scope") as IDataItem;
                Assert.IsNotNull(scopedJsScopeReader, TestResources.InvalidPropertyValueFormat, "scope");
                Assert.AreEqual(1, scopedJsScopeReader.GetValue("i"), TestResources.InvalidPropertyValueFormat, "i");

                Assert.AreEqual(new DateTime(635598857630000032), dataItem.GetValue("timestamp"), TestResources.InvalidPropertyValueFormat, "timestamp");

                Assert.AreEqual(symbolValue, dataItem.GetValue("symbol"), TestResources.InvalidPropertyValueFormat, "symbol");
            }
        }
    }
}
