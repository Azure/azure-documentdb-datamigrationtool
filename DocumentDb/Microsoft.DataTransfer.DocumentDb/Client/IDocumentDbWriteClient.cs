using Microsoft.Azure.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    interface IDocumentDbWriteClient : IDisposable
    {
        Task<string> GetOrCreateCollectionAsync(string collectionName, string partitionKey, int desiredThroughput, IndexingPolicy indexingPolicy, CancellationToken cancellation);
        Task CreateDocumentAsync(string collectionLink, object document, bool disableAutomaticIdGeneration);
        Task UpsertDocumentAsync(string collectionLink, object document, bool disableAutomaticIdGeneration);

        Task<string> CreateStoredProcedureAsync(string collectionLink, string name, string body);
        Task<StoredProcedureResult<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedureLink, params dynamic[] args);
        Task DeleteStoredProcedureAsync(string storedProcedureLink);
    }
}
