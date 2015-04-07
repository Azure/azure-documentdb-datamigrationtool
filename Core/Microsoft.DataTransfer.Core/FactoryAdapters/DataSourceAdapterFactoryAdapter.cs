using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.Core.FactoryAdapters
{
    sealed class DataSourceAdapterFactoryAdapter<TConfiguration> :
        DataAdapterFactoryAdapterBase<IDataSourceAdapterFactory<TConfiguration>, IDataSourceAdapter>,
        IDataSourceAdapterFactoryAdapter
    {
        public DataSourceAdapterFactoryAdapter(IDataSourceAdapterFactory<TConfiguration> factory, string displayName)
            : base(factory, displayName) { }
    }
}
