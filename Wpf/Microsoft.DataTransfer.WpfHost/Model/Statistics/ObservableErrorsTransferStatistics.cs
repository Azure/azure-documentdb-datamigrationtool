using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Microsoft.DataTransfer.WpfHost.Model.Statistics
{
    sealed class ObservableErrorsTransferStatistics : ITransferStatistics
    {
        private readonly ITransferStatistics defaultStatistics;
        private readonly SynchronizationContext observableSynchronizationContext;

        private ObservableCollection<KeyValuePair<string, Exception>> errors;

        public ObservableErrorsTransferStatistics(ITransferStatistics defaultStatistics, SynchronizationContext observableSynchronizationContext)
        {
            this.defaultStatistics = defaultStatistics;
            this.observableSynchronizationContext = observableSynchronizationContext;
            errors = new ObservableCollection<KeyValuePair<string, Exception>>();
        }

        public void Start()
        {
            defaultStatistics.Start();
        }

        public void Stop()
        {
            defaultStatistics.Stop();
        }

        public void AddTransferred()
        {
            defaultStatistics.AddTransferred();
        }

        public void AddError(string dataItemId, Exception error)
        {
            observableSynchronizationContext.Post(AddErrorSynchronized, new KeyValuePair<string, Exception>(dataItemId, error));
        }

        private void AddErrorSynchronized(object state)
        {
            errors.Add((KeyValuePair<string, Exception>)state);
        }

        public ITransferStatisticsSnapshot GetSnapshot()
        {
            return new ObservableTransferStatisticsSnapshot(defaultStatistics.GetSnapshot(), errors);
        }

        sealed class ObservableTransferStatisticsSnapshot : ITransferStatisticsSnapshot
        {
            private readonly ITransferStatisticsSnapshot defaultSnapshot;
            private readonly IReadOnlyCollection<KeyValuePair<string, Exception>> errors;

            public ObservableTransferStatisticsSnapshot(ITransferStatisticsSnapshot defaultSnapshot, IReadOnlyCollection<KeyValuePair<string, Exception>> errors)
            {
                this.defaultSnapshot = defaultSnapshot;
                this.errors = errors;
            }

            public TimeSpan ElapsedTime
            {
                get { return defaultSnapshot.ElapsedTime; }
            }

            public int Transferred
            {
                get { return defaultSnapshot.Transferred; }
            }

            public int Failed
            {
                get { return errors.Count; }
            }

            public IReadOnlyCollection<KeyValuePair<string, Exception>> GetErrors()
            {
                return errors;
            }
        }
    }
}
