
namespace Microsoft.DataTransfer.Basics.Collections
{
    /// <summary>
    /// Read-only mapping collection.
    /// </summary>
    /// <typeparam name="TKey">Type of the mapping key.</typeparam>
    /// <typeparam name="TValue">Type of the mapped value.</typeparam>
    public interface IReadOnlyMap<TKey, TValue>
    {
        /// <summary>
        /// Performs forward lookup of the value by the given key.
        /// </summary>
        /// <param name="key">Mapping key to lookup.</param>
        /// <param name="value">Matching mapped value.</param>
        /// <returns>true if requested key presents in the map; otherwise, false.</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Performs reverse lookup of the key by the given value.
        /// </summary>
        /// <param name="value">Mapped value to lookup.</param>
        /// <param name="key">Matching mapping key.</param>
        /// <returns>true if requested value presents in the map; otherwise, false.</returns>
        bool TryGetKey(TValue value, out TKey key);
    }
}
