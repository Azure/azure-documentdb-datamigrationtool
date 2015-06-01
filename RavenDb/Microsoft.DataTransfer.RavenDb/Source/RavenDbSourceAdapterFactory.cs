using Microsoft.DataTransfer.Extensibility.Basics.Source;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from RavenDB.
    /// </summary>
    public sealed class RavenDbSourceAdapterFactory : DataSourceAdapterFactoryWrapper<IRavenDbSourceAdapterConfiguration>
    {
        /// <summary>
        /// Creates a new instance of <see cref="RavenDbSourceAdapterFactory" />.
        /// </summary>
        public RavenDbSourceAdapterFactory()
            : base(new RavenDbSourceAdapterInternalFactory()) { }
    }
}
