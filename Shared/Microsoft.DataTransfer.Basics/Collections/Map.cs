using System.Collections;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Basics.Collections
{
    /// <summary>
    /// Mapping table with two-way lookup support.
    /// </summary>
    /// <typeparam name="TKey">Type of the mapping key.</typeparam>
    /// <typeparam name="TValue">Type of the mapped value.</typeparam>
    public sealed class Map<TKey, TValue> : IReadOnlyMap<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, TValue> forward;
        private Dictionary<TValue, TKey> reverse;

        /// <summary>
        /// Creates a new instance of <see cref="Map{TKey, TValue}" />.
        /// </summary>
        public Map()
        {
            forward = new Dictionary<TKey, TValue>();
            reverse = new Dictionary<TValue, TKey>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="Map{TKey, TValue}" />.
        /// </summary>
        /// <param name="source">Collection of key-value pairs to initialize the lookup table from.</param>
        public Map(IEnumerable<KeyValuePair<TKey, TValue>> source)
            : this()
        {
            Guard.NotNull("source", source);

            foreach (var item in source)
                Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds new mapping to the lookup table.
        /// </summary>
        /// <param name="key">Mapping key.</param>
        /// <param name="value">Mapped value.</param>
        public void Add(TKey key, TValue value)
        {
            forward.Add(key, value);
            reverse.Add(value, key);
        }

        /// <summary>
        /// Performs forward lookup of the value by the given key.
        /// </summary>
        /// <param name="key">Mapping key to lookup.</param>
        /// <param name="value">Matching mapped value.</param>
        /// <returns>true if requested key presents in the map; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return forward.TryGetValue(key, out value);
        }

        /// <summary>
        /// Performs reverse lookup of the key by the given value.
        /// </summary>
        /// <param name="value">Mapped value to lookup.</param>
        /// <param name="key">Matching mapping key.</param>
        /// <returns>true if requested value presents in the map; otherwise, false.</returns>
        public bool TryGetKey(TValue value, out TKey key)
        {
            return reverse.TryGetValue(value, out key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns>A collection of <see cref="KeyValuePair{TKey, TValue}" /> that can be used to iterate through the list.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return forward.GetEnumerator();
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
