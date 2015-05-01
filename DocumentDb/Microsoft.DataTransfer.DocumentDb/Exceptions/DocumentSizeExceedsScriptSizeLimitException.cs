using System;
using System.Runtime.Serialization;

namespace Microsoft.DataTransfer.DocumentDb.Exceptions
{
    [Serializable]
    sealed class DocumentSizeExceedsScriptSizeLimitException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="DocumentSizeExceedsScriptSizeLimitException" />.
        /// </summary>
        public DocumentSizeExceedsScriptSizeLimitException() { }

        /// <summary>
        /// Creates a new instance of <see cref="DocumentSizeExceedsScriptSizeLimitException" />.
        /// </summary>
        /// <param name="message">Error message.</param>
        public DocumentSizeExceedsScriptSizeLimitException(string message) : base(message) { }

        /// <summary>
        /// Creates a new instance of <see cref="DocumentSizeExceedsScriptSizeLimitException" />.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="inner">Inner exception.</param>
        public DocumentSizeExceedsScriptSizeLimitException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Creates a new instance of <see cref="DocumentSizeExceedsScriptSizeLimitException" />.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public DocumentSizeExceedsScriptSizeLimitException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
