using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client.Enumeration
{
    sealed class EmptyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        public static readonly EmptyAsyncEnumerator<T> Instance = new EmptyAsyncEnumerator<T>();

        public T Current { get { return default(T); } }

        public Task<bool> MoveNextAsync()
        {
            return Task.FromResult(false);
        }

        public void Dispose() { }
    }
}
