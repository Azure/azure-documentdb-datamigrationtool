using Microsoft.DataTransfer.Core.Statistics.Collections;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class InMemoryTransferStatistics : ThreadSafeTransferStatisticsBase
    {
        private AppendOnlyConcurrentLinkedList<KeyValuePair<string, Exception>> errors;

        public override int Failed
        {
            get { return errors.GetSnapshot().Count; }
        }

        public InMemoryTransferStatistics()
        {
            errors = new AppendOnlyConcurrentLinkedList<KeyValuePair<string, Exception>>();
        }

        public override void AddError(string dataItemId, Exception error)
        {
            errors.Add(new KeyValuePair<string, Exception>(dataItemId, error));
        }

        public override IReadOnlyCollection<KeyValuePair<string, Exception>> GetErrors()
        {
            return errors.GetSnapshot();
        }
    }
}
