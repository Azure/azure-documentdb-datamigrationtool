using MongoDB.Bson;
using MongoDB.Driver;
using System;
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
        public async Task TestConnection(string connectionString, CancellationToken cancellation)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            var url = new MongoUrl(connectionString);

            await new MongoClient(connectionString)
                .GetDatabase(url.DatabaseName)
                .RunCommandAsync(PingCommand);
        }
    }
}
