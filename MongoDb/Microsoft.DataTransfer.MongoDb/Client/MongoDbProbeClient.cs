using Microsoft.DataTransfer.MongoDb.Shared;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.Client
{
    /// <summary>
    /// Simple MongoDB client to verify the connection.
    /// </summary>
    public sealed class MongoDbProbeClient
    {
        private static readonly Command<BsonDocument> PingCommand = "{ping:1}";

        /// <summary>
        /// Tests the MongoDB connection.
        /// </summary>
        /// <param name="connectionString">MongoDB connection string to use to connect.</param>
        /// <param name="cancellation">Cancellation token</param>
        ////public async Task TestConnection(string connectionString, CancellationToken cancellation)
        public async Task TestConnection(IMongoDbAdapterConfiguration configuration, CancellationToken cancellation)
        {
            var connectionString = configuration.ConnectionString;

            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            if (configuration.IsCosmosDBHosted)
            {
                await TestCosmosDbMongo(configuration, cancellation);
            }
            else
            {
                var url = new MongoUrl(connectionString);

                ////var x = new MongoClient(connectionString).GetDatabase(url.DatabaseName);
                ////var y = x.GetCollection<BsonDocument>("Transaction");
                await new MongoClient(connectionString)
                    .GetDatabase(url.DatabaseName)
                    .RunCommandAsync(PingCommand);
            }
        }

        private async Task TestCosmosDbMongo(IMongoDbAdapterConfiguration configuration, CancellationToken cancellation)
        {
            var url = new MongoUrl(configuration.ConnectionString);

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

            MongoClient client = new MongoClient(settings);
            await client.GetDatabase(url.DatabaseName).RunCommandAsync(PingCommand);
        }
    }
}
