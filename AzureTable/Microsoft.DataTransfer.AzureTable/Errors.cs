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

        public static Exception EmptyResponseReceived()
        {
            return new InvalidOperationException(Resources.EmptyResponseReceived);
        }

        public static Exception SecondaryNotDefined(string message)
        {
            return new InvalidOperationException(string.Format("{0}, Inner Exception: {1}", Resources.SecondaryNotDefined, message));
        }
    }
}
