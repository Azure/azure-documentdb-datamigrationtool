using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using Microsoft.DataTransfer.Core.RemoteLogging;
using Microsoft.DataTransfer.ServiceModel.Errors;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class CosmosDBErrorLogTransferStatistics : ThreadSafeTransferStatisticsBase
    {
        private static readonly IReadOnlyCollection<KeyValuePair<string, string>> NoErrors = new KeyValuePair<string, string>[0];
        private readonly RemoteLoggingClientProvider remoteLoggingClientProvider = new RemoteLoggingClientProvider();
        private readonly IRemoteLogging remoteLogger;
        private int errorsCount;

        public CosmosDBErrorLogTransferStatistics(IErrorDetailsProvider errorDetailsProvider, IReadOnlyDictionary<string, string> destConfiguration, 
            string userProvidedLogDestination, CancellationToken cancellation) : base(errorDetailsProvider)
        {
            string destConnectionString;
            
            if(!string.IsNullOrEmpty(userProvidedLogDestination))
            {
                destConnectionString = userProvidedLogDestination;
            }
            if (destConfiguration.TryGetValue("ConnectionString", out destConnectionString) && destConnectionString.Contains(".table.cosmos"))
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(destConnectionString);
                TableConnectionPolicy connectionPolicy = new TableConnectionPolicy()
                {
                    UseDirectMode = true,
                    UseTcpProtocol = true,
                };
                remoteLogger = remoteLoggingClientProvider.CreateRemoteLoggingClient(storageAccount, connectionPolicy);
                remoteLogger.CreateRemoteLoggingTableIfNotExists(cancellation);
            }
            else
            {
                throw new Exception("Cosmos Table remote logging not possible. Destination needs to be a Cosmos Tables endpoint or" +
                    "provide connection string with the" +
                    "CosmosTableLogConnectionString flag to log.");
            }
        }

        public override int Failed
        {
            get { return errorsCount; }
        }

        public override IReadOnlyCollection<KeyValuePair<string, string>> GetErrors()
        {
            return NoErrors;
        }

        protected override void AddError(string dataItemId, string error)
        {
            Interlocked.Increment(ref errorsCount);
            remoteLogger.LogFailures(dataItemId, errorsCount.ToString(), error, "");
        }
    }
}
