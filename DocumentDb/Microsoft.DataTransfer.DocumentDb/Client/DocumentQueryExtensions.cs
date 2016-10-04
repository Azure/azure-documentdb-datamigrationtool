using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    static class DocumentQueryExtensions
    {
        public async static Task<T> FirstOrDefault<T>(this IDocumentQuery<T> query, CancellationToken cancellation)
        {
            if (!query.HasMoreResults)
                return default(T);

            return (await query.ExecuteNextAsync<T>(cancellation)).FirstOrDefault();
        }
    }
}
