using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.MongoDb
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception ConnectionStringMissing()
        {
            return new ArgumentException(Resources.ConnectionStringMissing);
        }

        public static Exception CollectionNameMissing()
        {
            return new ArgumentException(Resources.CollectionNameMissing);
        }

        public static Exception AmbiguousQuery()
        {
            return new ArgumentException(Resources.AmbiguousQuery);
        }

        public static Exception AmbiguousProjection()
        {
            return new ArgumentException(Resources.AmbiguousProjection);
        }

        public static Exception InvalidProjectionFormat()
        {
            return new FormatException(Resources.InvalidProjectionFormat);
        }
    }
}
