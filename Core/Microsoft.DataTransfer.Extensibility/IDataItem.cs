using System.Collections.Generic;

namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Represents data artifact that can be transferred between source and sink.
    /// </summary>
    public interface IDataItem
    {
        /// <summary>
        /// Provides collection of field names available in the data artifact.
        /// </summary>
        /// <returns>Collection of field names.</returns>
        IEnumerable<string> GetFieldNames();

        /// <summary>
        /// Provides a value of the specified data artifact field.
        /// </summary>
        /// <param name="fieldName">Name of data artifact field.</param>
        /// <returns>Value of the field.</returns>
        object GetValue(string fieldName);
    }
}
