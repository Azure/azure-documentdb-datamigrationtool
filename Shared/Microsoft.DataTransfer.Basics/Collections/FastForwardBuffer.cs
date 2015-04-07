using System.Collections;
using System.Collections.Generic;

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
        }

        /// <summary>
        /// Adds new element to the end of the list.
        /// </summary>
        /// <param name="element">Element to add.</param>
        public void Add(T element)
        {
            tail = tail.Next = new Node { Data = element };
            ++count;
        }

        /// <summary>
        /// Removes elements from the beginning of the list.
        /// </summary>
        /// <param name="numberOfElements">Number of elements to remove.</param>
        public void SkipForward(int numberOfElements)
        {
            var skipped = 0;
            var current = head;
            while ((current = current.Next) != null && skipped < numberOfElements)
                ++skipped;

            count -= skipped;
            if ((head.Next = current) == null)
                tail = head;
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
