using Microsoft.DataTransfer.Extensibility.Basics.Source;

namespace Microsoft.DataTransfer.DocumentDb.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from DocumentDB.
    /// </summary>
    public sealed class DocumentDbSourceAdapterFactory : DataSourceAdapterFactoryWrapper<IDocumentDbSourceAdapterConfiguration>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DocumentDbSourceAdapterFactory" />.
        /// </summary>
        public DocumentDbSourceAdapterFactory()
            : base(new DocumentDbSourceAdapterInternalFactory()) { }
    }
}
