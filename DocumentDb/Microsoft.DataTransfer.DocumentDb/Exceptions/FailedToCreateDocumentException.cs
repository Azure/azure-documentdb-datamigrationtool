using System;
using System.Runtime.Serialization;

namespace Microsoft.DataTransfer.DocumentDb.Exceptions
{
    /// <summary>
    /// Represents errors that occur when DocumentDB document cannot be created.
    /// </summary>
    [Serializable]
    public class FailedToCreateDocumentException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="FailedToCreateDocumentException" />.
        /// </summary>
        public FailedToCreateDocumentException() { }

        /// <summary>
        /// Creates a new instance of <see cref="FailedToCreateDocumentException" />.
        /// </summary>
        /// <param name="message">Error message.</param>
        public FailedToCreateDocumentException(string message) : base(message) { }

        /// <summary>
        /// Creates a new instance of <see cref="FailedToCreateDocumentException" />.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="inner">Inner exception.</param>
        public FailedToCreateDocumentException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Creates a new instance of <see cref="FailedToCreateDocumentException" />.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected FailedToCreateDocumentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
