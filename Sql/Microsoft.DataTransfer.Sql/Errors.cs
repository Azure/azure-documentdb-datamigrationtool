using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.Sql
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception ConnectionStringMissing()
        {
            return new ArgumentException(Resources.ConnectionStringMissing);
        }

        public static Exception QueryMissing()
        {
            return new ArgumentException(Resources.QueryMissing);
        }

        public static Exception AmbiguousQuery()
        {
            return new ArgumentException(Resources.AmbiguousQuery);
        }

        public static Exception CircularArcGeometryNotSupported()
        {
            return new NotSupportedException(Resources.CircularArcGeometryNotSupported);
        }

        public static Exception ErrorLoadingNativeBinaries(string assemblyName, int errorCode)
        {
            return new Exception(FormatMessage(Resources.ErrorLoadingNativeBinariesFormat, assemblyName, errorCode));
        }
    }
}
