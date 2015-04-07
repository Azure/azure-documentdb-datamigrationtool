using Microsoft.Azure.Documents.Client;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Shared;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    static class DocumentDbClientHelper
    {
        public static ConnectionPolicy ApplyConnectionMode(ConnectionPolicy connectionPolicy, DocumentDbConnectionMode? connectionMode)
        {
            Guard.NotNull("connectionPolicy", connectionPolicy);

            if (!connectionMode.HasValue)
                connectionMode = Defaults.Current.ConnectionMode;

            connectionPolicy.ConnectionMode = connectionMode == DocumentDbConnectionMode.Gateway
                ? ConnectionMode.Gateway : ConnectionMode.Direct;
            connectionPolicy.ConnectionProtocol = connectionMode == DocumentDbConnectionMode.DirectTcp
                ? Protocol.Tcp : Protocol.Https;

            return connectionPolicy;
        }
    }
}
