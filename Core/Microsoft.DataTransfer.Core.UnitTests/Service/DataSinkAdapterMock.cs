using Microsoft.DataTransfer.Basics.Threading;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.UnitTests.Service
{
    sealed class DataSinkAdapterMock : IDataSinkAdapter
    {
        private Action<IDataItem> writeCallback;
        private List<IDataItem> receivedData;
        public ICollection<IDataItem> ReceivedData { get { return receivedData; } }

        public int MaxDegreeOfParallelism { get { return 1; } }

        public DataSinkAdapterMock() : this(null) { }

        public DataSinkAdapterMock(Action<IDataItem> writeCallback)
        {
            this.writeCallback = writeCallback;
            receivedData = new List<IDataItem>();
        }

        public Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            if (writeCallback != null)
                writeCallback(dataItem);

            receivedData.Add(dataItem);
            return TaskHelper.NoOp;
        }

        public Task CompleteAsync(CancellationToken cancellation)
        {
            return TaskHelper.NoOp;
        }

        public void Dispose() { }
    }
}
