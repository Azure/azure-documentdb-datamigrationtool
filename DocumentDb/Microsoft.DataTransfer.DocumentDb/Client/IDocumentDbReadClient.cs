using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    interface IDocumentDbReadClient : IDisposable
    {
        Task<IAsyncEnumerator<IReadOnlyDictionary<string, object>>> QueryDocumentsAsync(
            string collectionNamePattern, string query, CancellationToken cancellation);
    }
}
