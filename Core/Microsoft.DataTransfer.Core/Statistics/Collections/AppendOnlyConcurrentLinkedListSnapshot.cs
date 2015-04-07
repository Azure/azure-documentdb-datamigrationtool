using Microsoft.DataTransfer.Basics;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Core.Statistics.Collections
{
    sealed class AppendOnlyConcurrentLinkedListSnapshot<T> : IReadOnlyCollection<T>
    {
        private ConcurrentLinkedListNode<T> head;
        private int count;

        public int Count
        {
            get { return count; }
        }

        public AppendOnlyConcurrentLinkedListSnapshot(ConcurrentLinkedListNode<T> head, int count)
        {
            Guard.NotNull("head", head);
            this.head = head;
            this.count = count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new AppendOnlyConcurrentLinkedListEnumerator<T>(head, count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
