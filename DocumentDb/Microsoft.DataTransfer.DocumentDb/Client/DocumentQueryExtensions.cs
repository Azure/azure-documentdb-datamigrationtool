using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    static class DocumentQueryExtensions
    {
        public async static Task<T> FirstOrDefault<T>(this IDocumentQuery<T> query)
        {
            if (!query.HasMoreResults)
                return default(T);

            return (await query.ExecuteNextAsync<T>()).FirstOrDefault();
        }
    }
}
