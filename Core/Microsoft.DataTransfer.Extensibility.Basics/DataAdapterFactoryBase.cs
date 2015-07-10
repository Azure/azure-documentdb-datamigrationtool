using System;
using System.IO;

namespace Microsoft.DataTransfer.Extensibility.Basics
{
    /// <summary>
    /// Provides basic functionality for the data adapter factories.
    /// </summary>
    public abstract class DataAdapterFactoryBase
    {
        /// <summary>
        /// Picks a value from raw input or the file.
        /// </summary>
        /// <param name="value">Raw string value.</param>
        /// <param name="fileName">Name of the file to read value from.</param>
        /// <param name="ambiguousErrorProvider">An <see cref="Exception" /> factory delegate to use when both arguments are set.</param>
        /// <returns>Raw input value or file content.</returns>
        protected static string StringValueOrFile(string value, string fileName, Func<Exception> ambiguousErrorProvider)
        {
            var isFileSet = !String.IsNullOrEmpty(fileName);

            if (!String.IsNullOrEmpty(value) && isFileSet)
                throw ambiguousErrorProvider();

            return isFileSet ? File.ReadAllText(fileName) : value;
        }
    }
}
