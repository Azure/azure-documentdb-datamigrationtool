using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.ServiceModel.Errors;
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
        private readonly IErrorDetailsProvider errorDetailsProvider;
        private readonly SynchronizationContext observableSynchronizationContext;

        private ObservableCollection<KeyValuePair<string, string>> errors;

        public ObservableErrorsTransferStatistics(ITransferStatistics defaultStatistics, IErrorDetailsProvider errorDetailsProvider,
            SynchronizationContext observableSynchronizationContext)
        {
            Guard.NotNull("defaultStatistics", defaultStatistics);
            Guard.NotNull("errorDetailsProvider", errorDetailsProvider);
            Guard.NotNull("observableSynchronizationContext", observableSynchronizationContext);

            this.defaultStatistics = defaultStatistics;
            this.errorDetailsProvider = errorDetailsProvider;
            this.observableSynchronizationContext = observableSynchronizationContext;

            errors = new ObservableCollection<KeyValuePair<string, string>>();
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
            observableSynchronizationContext.Post(AddErrorSynchronized,
                new KeyValuePair<string, string>(dataItemId, errorDetailsProvider.Get(error)));
        }

        private void AddErrorSynchronized(object state)
        {
            errors.Add((KeyValuePair<string, string>)state);
        }

        public ITransferStatisticsSnapshot GetSnapshot()
        {
            return new ObservableTransferStatisticsSnapshot(defaultStatistics.GetSnapshot(), errors);
        }

        sealed class ObservableTransferStatisticsSnapshot : ITransferStatisticsSnapshot
        {
            private readonly ITransferStatisticsSnapshot defaultSnapshot;
            private readonly IReadOnlyCollection<KeyValuePair<string, string>> errors;

            public ObservableTransferStatisticsSnapshot(ITransferStatisticsSnapshot defaultSnapshot, IReadOnlyCollection<KeyValuePair<string, string>> errors)
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

            public IReadOnlyCollection<KeyValuePair<string, string>> GetErrors()
            {
                return errors;
            }
        }
    }
}
