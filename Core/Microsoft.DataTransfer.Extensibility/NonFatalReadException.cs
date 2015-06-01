using Microsoft.DataTransfer.Basics;
using System;
using System.Runtime.Serialization;

namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Represents non-critical data read error.
    /// </summary>
    /// <remarks>
    /// This exception can be thrown when data source adapter encounters an error, but can continue reading next data artifacts.
    /// </remarks>
    [Serializable]
    public class NonFatalReadException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="NonFatalReadException" />.
        /// </summary>
        public NonFatalReadException() { }

        /// <summary>
        /// Creates a new instance of <see cref="NonFatalReadException" />.
        /// </summary>
        /// <param name="message">Error message.</param>
        public NonFatalReadException(string message)
            : base(message) { }

        /// <summary>
        /// Creates a new instance of <see cref="NonFatalReadException" />.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public NonFatalReadException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Creates a new instance of <see cref="NonFatalReadException" />.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected NonFatalReadException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Converts provided <paramref name="source" /> <see cref="Exception" /> to <see cref="NonFatalReadException" />.
        /// </summary>
        /// <param name="source">Source <see cref="Exception" /> to extract message and inner exception from.</param>
        /// <returns>New instance of <see cref="NonFatalReadException" /> with source message and inner exception.</returns>
        public static NonFatalReadException Convert(Exception source)
        {
            Guard.NotNull("source", source);
            return new NonFatalReadException(source.Message, source.InnerException);
        }
    }
}
