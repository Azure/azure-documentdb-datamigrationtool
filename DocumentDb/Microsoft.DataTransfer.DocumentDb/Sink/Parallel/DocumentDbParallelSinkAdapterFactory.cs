using Microsoft.DataTransfer.Extensibility.Basics.Sink;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    /// <summary>
    /// Provides data sink adapters capable of writing data to DocumentDB account in parallel.
    /// </summary>
    public sealed class DocumentDbParallelSinkAdapterFactory : DataSinkAdapterFactoryWrapper<IDocumentDbParallelSinkAdapterConfiguration>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DocumentDbParallelSinkAdapterFactory" />.
        /// </summary>
        public DocumentDbParallelSinkAdapterFactory()
            : base(new DocumentDbParallelSinkAdapterInternalFactory()) { }
    }
}
