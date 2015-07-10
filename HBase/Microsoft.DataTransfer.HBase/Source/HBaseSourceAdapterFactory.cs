using Microsoft.DataTransfer.Extensibility.Basics.Source;

namespace Microsoft.DataTransfer.HBase.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from HBase.
    /// </summary>
    public sealed class HBaseSourceAdapterFactory : DataSourceAdapterFactoryWrapper<IHBaseSourceAdapterConfiguration>
    {
        /// <summary>
        /// Creates a new instance of <see cref="HBaseSourceAdapterFactory" />.
        /// </summary>
        public HBaseSourceAdapterFactory()
            : base(new HBaseSourceAdapterInternalFactory()) { }
    }
}
