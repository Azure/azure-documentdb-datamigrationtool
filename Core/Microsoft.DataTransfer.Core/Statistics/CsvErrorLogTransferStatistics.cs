using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class CsvErrorLogTransferStatistics : ThreadSafeTransferStatisticsBase, IDisposable
    {
        private static IReadOnlyCollection<KeyValuePair<string, Exception>> EmptyErrors = new KeyValuePair<string, Exception>[0];

        private StreamWriter errorLogStreamWriter;
        private TextWriter errorLogSynchronizedWriter;
        private int errorsCount;

        public override int Failed
        {
            get { return errorsCount; }
        }

        public CsvErrorLogTransferStatistics(StreamWriter errorLogStreamWriter)
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

        public override void AddError(string dataItemId, Exception error)
        {
            Interlocked.Increment(ref errorsCount);

            var writer = errorLogSynchronizedWriter;
            if (writer != null)
            {
                writer.WriteLine(EscapeValue(dataItemId) + "," + EscapeValue(error.Message));
                writer.Flush();
            }
        }

        public override IReadOnlyCollection<KeyValuePair<string, Exception>> GetErrors()
        {
            return EmptyErrors;
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
