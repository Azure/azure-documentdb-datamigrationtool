using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.Sql
{
    sealed class Errors : CommonErrors
    {
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
    }
}
