using Microsoft.DataTransfer.Core.Statistics.Collections;
using Microsoft.DataTransfer.ServiceModel.Errors;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class InMemoryTransferStatistics : ThreadSafeTransferStatisticsBase
    {
        private AppendOnlyConcurrentLinkedList<KeyValuePair<string, string>> errors;

        public override int Failed
        {
            get { return errors.GetSnapshot().Count; }
        }

        public InMemoryTransferStatistics(IErrorDetailsProvider errorDetailsProvider)
            : base(errorDetailsProvider)
        {
            errors = new AppendOnlyConcurrentLinkedList<KeyValuePair<string, string>>();
        }

        protected override void AddError(string dataItemId, string error)
        {
            errors.Add(new KeyValuePair<string, string>(dataItemId, error));
        }

        public override IReadOnlyCollection<KeyValuePair<string, string>> GetErrors()
        {
            return errors.GetSnapshot();
        }
    }
}
