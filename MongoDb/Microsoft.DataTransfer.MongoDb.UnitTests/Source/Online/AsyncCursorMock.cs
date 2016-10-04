using Microsoft.DataTransfer.Basics;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.UnitTests.Source.Online
{
    sealed class AsyncCursorMock<T> : IAsyncCursor<T>
    {
        private IEnumerator<IEnumerable<T>> batches;

        public bool Disposed { get; private set; }

        public IEnumerable<T> Current { get; private set; }

        public AsyncCursorMock(IEnumerable<IEnumerable<T>> batches)
        {
            this.batches = batches.GetEnumerator();
        }

        public bool MoveNext(CancellationToken cancellationToken)
        {
            return MoveNextAsync(cancellationToken).Result;
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            if (batches.MoveNext())
            {
                Current = batches.Current;
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public void Dispose()
        {
            Disposed = true;

            TrashCan.Throw(ref batches);
        }
    }
}
