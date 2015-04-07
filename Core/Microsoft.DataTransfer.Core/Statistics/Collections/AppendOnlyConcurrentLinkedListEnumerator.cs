using Microsoft.DataTransfer.Basics;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Core.Statistics.Collections
{
    sealed class AppendOnlyConcurrentLinkedListEnumerator<T> : IEnumerator<T>
    {
        private ConcurrentLinkedListNode<T> head;
        private int count;

        private ConcurrentLinkedListNode<T> current;
        private int index;

        public T Current
        {
            get { return current.Value; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public AppendOnlyConcurrentLinkedListEnumerator(ConcurrentLinkedListNode<T> head, int count)
        {
            Guard.NotNull("head", head);
            this.current = this.head = head;
            this.count = count;
        }

        public bool MoveNext()
        {
            if (index >= count || current == null)
                return false;
            
            current = current.Next;
            ++index;
            return current != null;
        }

        public void Reset()
        {
            current = head;
            index = 0;
        }

        public void Dispose() { }
    }
}
