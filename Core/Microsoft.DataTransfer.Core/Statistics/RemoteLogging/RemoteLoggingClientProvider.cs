using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;

namespace Microsoft.DataTransfer.Core.RemoteLogging
{
    /// <summary>
    /// Provides a client for remote logging in the application.
    /// </summary>
    public class RemoteLoggingClientProvider
    {
        /// <summary>
        /// Create a new remote logger
        /// </summary>
        /// <param name="account">cloudstorage account details</param>
        /// <param name="connectionPolicy">connection policy details</param>
        /// <returns></returns>
        public RemoteLogging CreateRemoteLoggingClient(CloudStorageAccount account, TableConnectionPolicy connectionPolicy)
        {
            return new RemoteLogging(account, connectionPolicy);            
        }
    }
}
