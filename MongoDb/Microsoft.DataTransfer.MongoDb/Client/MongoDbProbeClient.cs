using MongoDB.Driver;
using System;

namespace Microsoft.DataTransfer.MongoDb.Client
{
    /// <summary>
    /// Simple MongoDB client to verify the connection.
    /// </summary>
    public sealed class MongoDbProbeClient
    {
        /// <summary>
        /// Tests the MongoDB connection.
        /// </summary>
        /// <param name="connectionString">MongoDB connection string to use to connect.</param>
        public void TestConnection(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();
            
            var server = new MongoClient(connectionString).GetServer();
            server.Connect();
            server.Disconnect();
        }
    }
}
