using Raven.Client.Document;
using System;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.RavenDb.Client
{
    /// <summary>
    /// Simple RavenDB client to verify the connection.
    /// </summary>
    public sealed class RavenDbProbeClient
    {
        /// <summary>
        /// Tests the RavenDB connection.
        /// </summary>
        /// <param name="connectionString">RavenDB connection string to use to connect.</param>
        public async Task TestConnectionAsync(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            using (var store = new DocumentStore())
            {
                store.ParseConnectionString(connectionString);
                store.Initialize(false);
                
                await store.AsyncDatabaseCommands.GetStatisticsAsync();
            }
        }
    }
}
