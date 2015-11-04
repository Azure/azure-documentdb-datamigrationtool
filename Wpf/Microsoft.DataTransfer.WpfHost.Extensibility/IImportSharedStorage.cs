using System;

namespace Microsoft.DataTransfer.WpfHost.Extensibility
{
    /// <summary>
    /// Allows to pass data between different import sources and sinks. 
    /// </summary>
    public interface IImportSharedStorage
    {
        /// <summary>
        /// Adds a value to the storage if the key does not already exist.
        /// </summary>
        /// <typeparam name="T">Type of the value to store.</typeparam>
        /// <param name="key">Key to identify the value.</param>
        /// <param name="valueFactory">The function used to generate a value for the key.</param>
        /// <returns>
        /// The value for the key. This will be either the existing value for the key
        /// if the key is already in the storage, or the new value for the key as
        /// returned by <paramref name="valueFactory" /> if the key was not in the storage.
        /// </returns>
        T GetOrAdd<T>(object key, Func<object, T> valueFactory) where T : class;
    }
}
