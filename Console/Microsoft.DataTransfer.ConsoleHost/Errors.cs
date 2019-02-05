using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.ConsoleHost
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception SourceMissing()
        {
            return new ArgumentException(Resources.SourceNotSpecified);
        }

        public static Exception TargetMissing()
        {
            return new ArgumentException(Resources.TargetNotSpecified);
        }

        public static Exception UnknownSource(string name)
        {
            return new KeyNotFoundException(FormatMessage(Resources.UnknownSourceFormat, name));
        }

        public static Exception UnknownDestination(string name)
        {
            return new KeyNotFoundException(FormatMessage(Resources.UnknownTargetFormat, name));
        }

        public static Exception DataAdapterConfigurationFactoryNotFound(Type configurationType)
        {
            return new KeyNotFoundException(FormatMessage(Resources.DataAdapterConfigurationFactoryNotFoundFormat, configurationType));
        }
    }
}
