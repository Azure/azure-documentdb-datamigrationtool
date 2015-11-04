using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.ServiceModel.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class CsvErrorLogTransferStatistics : ThreadSafeTransferStatisticsBase, IDisposable
    {
        private static IReadOnlyCollection<KeyValuePair<string, string>> NoErrors = new KeyValuePair<string, string>[0];

        private StreamWriter errorLogStreamWriter;
        private TextWriter errorLogSynchronizedWriter;
        private int errorsCount;

        public override int Failed
        {
            get { return errorsCount; }
        }

        public CsvErrorLogTransferStatistics(StreamWriter errorLogStreamWriter, IErrorDetailsProvider errorDetailsProvider)
            : base(errorDetailsProvider)
        {
            Guard.NotNull("errorLogStream", errorLogStreamWriter);

            this.errorLogStreamWriter = errorLogStreamWriter;
            errorLogSynchronizedWriter = TextWriter.Synchronized(errorLogStreamWriter);
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
            TrashCan.Throw(ref errorLogSynchronizedWriter);
            TrashCan.Throw(ref errorLogStreamWriter);
        }

        protected override void AddError(string dataItemId, string error)
        {
            Interlocked.Increment(ref errorsCount);

            var writer = errorLogSynchronizedWriter;
            if (writer != null)
                writer.WriteLine(EscapeValue(dataItemId) + "," + EscapeValue(error));
        }

        public override IReadOnlyCollection<KeyValuePair<string, string>> GetErrors()
        {
            return NoErrors;
        }

        private static string EscapeValue(string value)
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
