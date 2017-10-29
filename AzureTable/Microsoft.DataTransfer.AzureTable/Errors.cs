using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.AzureTable
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception ConnectionStringMissing()
        {
            return new ArgumentException(Resources.ConnectionStringMissing);
        }

        public static Exception TableNameMissing()
        {
            return new ArgumentException(Resources.TableNameMissing);
        }

        public static Exception BatchSizeInvalid(int maxBatchSize)
        {
            return new ArgumentException(
                string.Format(Resources.BatchSizeInvalid, maxBatchSize/(1024*1024))
            );
        }

        public static Exception EmptyResponseReceived()
        {
            return new InvalidOperationException(Resources.EmptyResponseReceived);
        }
    }
}
