using Microsoft.DataTransfer.Core.Statistics.Collections;
using Microsoft.DataTransfer.ServiceModel.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class ThreadSafeTransferStatistics : ITransferStatistics, ITransferStatisticsSnapshot
    {
        private Stopwatch timer;
        private int transferred;
        private AppendOnlyConcurrentLinkedList<KeyValuePair<string, Exception>> errors;

        public TimeSpan ElapsedTime
        {
            get { return timer.Elapsed; }
        }

        public int Transferred
        {
            get { return transferred; }
        }

        public ThreadSafeTransferStatistics()
        {
            timer = new Stopwatch();
            errors = new AppendOnlyConcurrentLinkedList<KeyValuePair<string, Exception>>();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void AddTransferred()
        {
            Interlocked.Increment(ref transferred);
        }

        public void AddError(string dataItemId, Exception error)
        {
            errors.Add(new KeyValuePair<string, Exception>(dataItemId, error));
        }

        public ITransferStatisticsSnapshot GetSnapshot()
        {
            return this;
        }

        public IReadOnlyCollection<KeyValuePair<string, Exception>> GetErrors()
        {
            return errors.GetSnapshot();
        }
    }
}
