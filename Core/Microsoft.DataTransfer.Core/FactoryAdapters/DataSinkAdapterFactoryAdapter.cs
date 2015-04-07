using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.Core.FactoryAdapters
{
    sealed class DataSinkAdapterFactoryAdapter<TConfiguration> :
        DataAdapterFactoryAdapterBase<IDataSinkAdapterFactory<TConfiguration>, IDataSinkAdapter>,
        IDataSinkAdapterFactoryAdapter
    {
        public DataSinkAdapterFactoryAdapter(IDataSinkAdapterFactory<TConfiguration> factory, string displayName)
            : base(factory, displayName) { }
    }
}
