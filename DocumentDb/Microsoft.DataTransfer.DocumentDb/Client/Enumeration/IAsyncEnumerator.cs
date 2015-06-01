using System;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client.Enumeration
{
    interface IAsyncEnumerator<out T> : IDisposable
    {
        T Current { get; }
        Task<bool> MoveNextAsync();
    }
}
