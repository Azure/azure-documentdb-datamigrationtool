using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.RavenDb
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception ConnectionStringMissing()
        {
            return new ArgumentException(Resources.ConnectionStringMissing);
        }

        public static Exception AmbiguousQuery()
        {
            return new ArgumentException(Resources.AmbiguousQuery);
        }

        public static Exception NonJsonDocumentRead()
        {
            return new InvalidOperationException(Resources.NonJsonDocumentRead);
        }
    }
}
