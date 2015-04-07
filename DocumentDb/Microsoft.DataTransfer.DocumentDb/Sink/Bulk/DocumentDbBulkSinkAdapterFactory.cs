using Microsoft.DataTransfer.Extensibility.Basics.Sink;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    /// <summary>
    /// Provides data sink adapters capable of writing data to DocumentDB account in batches.
    /// </summary>
    public sealed class DocumentDbBulkSinkAdapterFactory : DataSinkAdapterFactoryWrapper<IDocumentDbBulkSinkAdapterConfiguration>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DocumentDbBulkSinkAdapterFactory" />.
        /// </summary>
        public DocumentDbBulkSinkAdapterFactory() 
            : base(new DocumentDbBulkSinkAdapterInternalFactory()) { }
    }
}
