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
        const string AuthenticationMechanism = "SCRAM-SHA-1";
        private static readonly Command<BsonDocument> PingCommand = "{ping:1}";

        /// <summary>
        /// Tests the MongoDB connection.
        /// </summary>
        /// <param name="configuration">MongoDB Adapter configuration to use to connect.  
        ///                             Provides access to all parameters from the screen.</param>
        /// <param name="cancellation">Cancellation token</param>
        public async Task TestConnection(IMongoDbAdapterConfiguration configuration, CancellationToken cancellation)
        {
            var connectionString = configuration.ConnectionString;

            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            var url = new MongoUrl(connectionString);
            var client = configuration.IsCosmosDBHosted ? new MongoClient(connectionString) : GetCosmosDBClient(url);

            await client.GetDatabase(url.DatabaseName)
                .RunCommandAsync(PingCommand);
        }

        /// <summary>
        /// Gets the cosmos database mongo client.
        /// </summary>
        /// <param name="url">The Mongo URL configuration information.</param>
        /// <returns></returns>
        private MongoClient GetCosmosDBClient(MongoUrl url)
        {
            MongoClientSettings settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(url.Server.Host, url.Server.Port),
                UseSsl = url.UseSsl
            };

            if (url.UseSsl)
            {
                settings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };
            }

            MongoIdentity identity = new MongoInternalIdentity(url.DatabaseName, url.Username);
            MongoIdentityEvidence evidence = new PasswordEvidence(url.Password);

            settings.Credentials = new List<MongoCredential>()
                                        {
                                            new MongoCredential(AuthenticationMechanism, identity, evidence)
                                        };

            return new MongoClient(settings);
        }
    }
}
