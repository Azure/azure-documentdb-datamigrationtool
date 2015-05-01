using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.AzureTable
{
    sealed class Errors : CommonErrors
    {
        public static Exception ConnectionStringMissing()
        {
            return new ArgumentException(Resources.ConnectionStringMissing);
        }

        public static Exception EmptyResponseReceived()
        {
            return new InvalidOperationException(Resources.EmptyResponseReceived);
        }
    }
}
