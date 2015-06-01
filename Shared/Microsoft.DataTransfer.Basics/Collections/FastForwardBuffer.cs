using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.DataTransfer.Basics.Collections
{
    /// <summary>
    /// Light-weight forward-only linked list.
    /// </summary>
    /// <typeparam name="T">Type of the element</typeparam>
    public sealed class FastForwardBuffer<T> : IEnumerable<T>
    {
        sealed class Node
        {
            public T Data;
            public Node Next;
        }

        private Node head, tail;
        private int count;

        private object skipLock;

        /// <summary>
        /// Gets the count of elements in the list.
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>
        /// Creates a new instance of <see cref="FastForwardBuffer{T}" />.
        /// </summary>
        public FastForwardBuffer()
        {
            head = tail = new Node();
            skipLock = new object();
        }

        /// <summary>
        /// Adds new element to the end of the list.
        /// </summary>
        /// <param name="element">Element to add.</param>
        public void Add(T element)
        {
            var newTail = new Node { Data = element };

            Node oldTail;
            do
            {
                oldTail = tail;
            }
            while (Interlocked.CompareExchange(ref tail, newTail, oldTail) != oldTail);

            oldTail.Next = newTail;

            Interlocked.Increment(ref count);
        }

        /// <summary>
        /// Removes elements from the beginning of the list.
        /// </summary>
        /// <param name="numberOfElements">Number of elements to remove.</param>
        public void SkipForward(int numberOfElements)
        {
            lock (skipLock)
            {
                var skipped = 0;

                var newHead = head;
                while (skipped < numberOfElements && newHead.Next != null)
                {
                    newHead = newHead.Next;
                    ++skipped;
                }

                head = newHead;

                Interlocked.Add(ref count, -skipped);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}" /> that can be used to iterate through the list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while ((current = current.Next) != null)
                yield return current.Data;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> that can be used to iterate through the list.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
