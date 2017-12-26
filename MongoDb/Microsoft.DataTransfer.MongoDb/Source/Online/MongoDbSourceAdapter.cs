using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using Microsoft.DataTransfer.MongoDb.Shared;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
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
        private IAsyncEnumerator<BsonDocument> documentsCursor;

        public MongoDbSourceAdapter(IMongoDbSourceAdapterInstanceConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);
            this.configuration = configuration;
        }

        private MongoClient GetClient(MongoUrl url)
        {
            if (!this.configuration.IsCosmosDBHosted)
            {
                return new MongoClient(url);
            }

            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(url.Server.Host, url.Server.Port);
            settings.UseSsl = url.UseSsl;
            if (url.UseSsl)
            {
                settings.SslSettings = new SslSettings();
                settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            }

            MongoIdentity identity = new MongoInternalIdentity(url.DatabaseName, url.Username);
            MongoIdentityEvidence evidence = new PasswordEvidence(url.Password);

            settings.Credentials = new List<MongoCredential>()
                                        {
                                            new MongoCredential("SCRAM-SHA-1", identity, evidence)
                                        };

            return new MongoClient(settings);
        }

        public async Task Initialize(CancellationToken cancellation)
        {
            var url = new MongoUrl(configuration.ConnectionString);

            FindOptions<BsonDocument, BsonDocument> options = null;

            if (!String.IsNullOrEmpty(configuration.Projection))
            {
                options =
                    new FindOptions<BsonDocument, BsonDocument>
                    {
                        Projection =
                            new BsonDocumentProjectionDefinition<BsonDocument, BsonDocument>(
                                BsonSerializer.Deserialize<BsonDocument>(configuration.Projection))
                    };
            }

            documentsCursor = new AsyncCursorEnumerator<BsonDocument>(
                await MongoRetryPolicy.ExecuteAsync(async () =>
                {
                    var collection =
                            GetClient(url)
                            .GetDatabase(url.DatabaseName)
                            .GetCollection<BsonDocument>(configuration.Collection);
                    
                    var mongoCursor = String.IsNullOrEmpty(configuration.Query) 
                        ? collection.FindAsync(
                            Builders<BsonDocument>.Filter.Empty,
                            options,
                            cancellation)
                        : collection.FindAsync(
                            BsonSerializer.Deserialize<BsonDocument>(configuration.Query),
                            options,
                            cancellation);

                    return await mongoCursor;
                },
                cancellation));
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            if (documentsCursor == null || !(await documentsCursor.MoveNextAsync(cancellation)))
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
