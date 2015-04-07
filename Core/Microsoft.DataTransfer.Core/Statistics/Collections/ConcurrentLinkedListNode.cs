using System.Threading;

namespace Microsoft.DataTransfer.Core.Statistics.Collections
{
    sealed class ConcurrentLinkedListNode<T>
    {
        private ConcurrentLinkedListNode<T> next;

        public ConcurrentLinkedListNode<T> Next
        {
            get { return next; }
        }

        public T Value { get; private set; }

        public ConcurrentLinkedListNode(T value)
        {
            Value = value;
        }

        public bool SetNext(ConcurrentLinkedListNode<T> newNext)
        {
            var oldNext = next;
            return Interlocked.CompareExchange(ref next, newNext, oldNext) == oldNext;
        }
    }
}
