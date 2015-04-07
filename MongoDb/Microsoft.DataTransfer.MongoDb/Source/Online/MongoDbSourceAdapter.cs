using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.MongoDb.Shared;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.Source.Online
{
    sealed class MongoDbSourceAdapter : IDataSourceAdapter
    {
        private const string DocumentIdFieldName = "_id";

        private static RetryPolicy MongoRetryPolicy = new RetryPolicy(
            new MongoTransientErrorDetectionStrategy(), 
            new Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3)));

        private IMongoDbSourceAdapterInstanceConfiguration configuration;
        private IEnumerator<BsonDocument> documentsCursor;

        public MongoDbSourceAdapter(IMongoDbSourceAdapterInstanceConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);
            this.configuration = configuration;
        }

        public void Initialize()
        {
            var url = new MongoUrl(configuration.ConnectionString);

            MongoRetryPolicy.ExecuteAction(() =>
            {
                var server = new MongoClient(url).GetServer();
                server.Connect();

                var collection = server.GetDatabase(url.DatabaseName).GetCollection(configuration.Collection);
                var mongoCursor = String.IsNullOrEmpty(configuration.Query) 
                    ? collection.FindAll()
                    : collection.Find(new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(configuration.Query)));

                documentsCursor = mongoCursor.SetFields(ParseProjection(configuration.Projection)).GetEnumerator();
            });
        }

        private static FieldsBuilder ParseProjection(string projection)
        {
            var fields = new FieldsBuilder();

            if (String.IsNullOrEmpty(projection))
                return fields;

            var projectionDocument = BsonSerializer.Deserialize<BsonDocument>(projection);

            foreach (var element in projectionDocument)
            {
                var value = element.Value;

                if (value.IsBoolean && value.AsBoolean || value.IsInt32 && value.AsInt32 != 0)
                {
                    fields.Include(element.Name);
                }
                else if (value.IsBoolean && !value.AsBoolean || value.IsInt32 && value.AsInt32 == 0)
                {
                    fields.Exclude(element.Name);
                }
                else
                {
                    throw Errors.InvalidProjectionFormat();
                }
            }

            return fields;
        }

        public Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            return Task.Factory.StartNew<IDataItem>(ReadNext, readOutput);
        }

        private IDataItem ReadNext(object taskState)
        {
            var readOutput = (ReadOutputByRef)taskState;

            if (documentsCursor == null || !documentsCursor.MoveNext())
                return null;

            var document = documentsCursor.Current;

            BsonValue bsonValue;
            if (document.TryGetValue(DocumentIdFieldName, out bsonValue))
                readOutput.DataItemId = bsonValue.ToString(); 

            return new BsonDocumentDataItem(document);
        }

        public void Dispose()
        {
            TrashCan.Throw(ref documentsCursor);
        }
    }
}
