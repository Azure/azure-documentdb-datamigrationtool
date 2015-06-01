using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.DataTransfer.Core.Statistics
{
    abstract class ThreadSafeTransferStatisticsBase : ITransferStatistics, ITransferStatisticsSnapshot
    {
        private Stopwatch timer;
        private int transferred;

        public TimeSpan ElapsedTime
        {
            get { return timer.Elapsed; }
        }

        public int Transferred
        {
            get { return transferred; }
        }

        public abstract int Failed { get; }

        public ThreadSafeTransferStatisticsBase()
        {
            timer = new Stopwatch();
        }

        public virtual void Start()
        {
            timer.Start();
        }

        public virtual void Stop()
        {
            timer.Stop();
        }

        public void AddTransferred()
        {
            Interlocked.Increment(ref transferred);
        }

        public abstract void AddError(string dataItemId, Exception error);

        public ITransferStatisticsSnapshot GetSnapshot()
        {
            return this;
        }

        public abstract IReadOnlyCollection<KeyValuePair<string, Exception>> GetErrors();
    }
}
