using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    interface IDocumentDbReadClient : IDisposable
    {
        IEnumerable<IReadOnlyDictionary<string, object>> QueryDocuments(string collectionName, string query);
    }
}
