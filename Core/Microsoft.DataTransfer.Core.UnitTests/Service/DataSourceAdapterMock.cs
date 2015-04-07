using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.UnitTests.Service
{
    sealed class DataSourceAdapterMock : IDataSourceAdapter
    {
        private IEnumerator<IDataItem> dataItems;
        private Action<IDataItem> dataReadCallback;

        public DataSourceAdapterMock(IEnumerable<IDataItem> dataItems)
            : this(dataItems, null) { }

        public DataSourceAdapterMock(IEnumerable<IDataItem> dataItems, Action<IDataItem> dataReadCallback)
        {
            this.dataItems = dataItems.GetEnumerator();
            this.dataReadCallback = dataReadCallback;
        }

        public Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            var result = !dataItems.MoveNext() ? null : dataItems.Current;

            if (dataReadCallback != null)
                dataReadCallback(result);

            return Task.FromResult(result);
        }

        public void Dispose()
        {
            dataItems.Dispose();
        }
    }
}
