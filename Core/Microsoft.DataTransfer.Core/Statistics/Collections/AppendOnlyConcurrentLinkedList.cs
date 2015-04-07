using System.Collections.Generic;
using System.Threading;

namespace Microsoft.DataTransfer.Core.Statistics.Collections
{
    sealed class AppendOnlyConcurrentLinkedList<T>
    {
        private ConcurrentLinkedListNode<T> head;
        private ConcurrentLinkedListNode<T> volatileTail;

        private int count;

        public AppendOnlyConcurrentLinkedList()
        {
            volatileTail = head = new ConcurrentLinkedListNode<T>(default(T));
        }

        public void Add(T element)
        {
            /*
             * This algorithm tries to update pointer to the next node, as opposed to tail node itself.
             * Drawback of this approach is that actual tail is not known, so additional tail look-up
             * in the form of nested while loop is required before every insert attempt.
             * Alternatevely, CAS operation can be applied to the tail node itself, in which case
             * count cannot be calculated along with the insert operation and requires iterating
             * when taking a snapshot.
             */
            var newNode = new ConcurrentLinkedListNode<T>(element);

            var tail = volatileTail;
            do
            {
                while (tail.Next != null)
                    tail = tail.Next;
            } while (!tail.SetNext(newNode));

            volatileTail = newNode;
            Interlocked.Increment(ref count);
        }

        public IReadOnlyCollection<T> GetSnapshot()
        {
            return new AppendOnlyConcurrentLinkedListSnapshot<T>(head, count);
        }
    }
}
