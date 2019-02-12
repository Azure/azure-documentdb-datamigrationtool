using System.Collections.Concurrent;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;

namespace Microsoft.DataTransfer.AzureTable.RemoteLogging
{
    /// <summary>
    /// Provides a client for remote logging in the application.
    /// </summary>
    public class RemoteLoggingClientProvider
    {
        private static readonly ConcurrentDictionary<string, RemoteLogging> clientProvider =
            new ConcurrentDictionary<string, RemoteLogging>();

        /// <summary>
        /// If a remote logger exists, provide it to the caller
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RemoteLogging GetRemoteLogger(string key)
        {            
            if (clientProvider.TryGetValue(key, out RemoteLogging client))
            {
                return client;
            }
            return null;
        }

        /// <summary>
        /// Create a new remote logger
        /// </summary>
        /// <param name="account"></param>
        /// <param name="connectionPolicy"></param>
        /// <returns></returns>
        public RemoteLogging CreateRemoteLoggingClient(CloudStorageAccount account, TableConnectionPolicy connectionPolicy)
        {
            RemoteLogging client = new RemoteLogging(account, connectionPolicy);
            clientProvider["tableapibulk"] = client;
            return client;
        }
    }
}
