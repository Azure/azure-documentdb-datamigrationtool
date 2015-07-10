using Microsoft.DataTransfer.Basics;
using System;
using System.Net;

namespace Microsoft.DataTransfer.HBase
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception InvalidServerResponse(HttpStatusCode statusCode)
        {
            return new WebException(FormatMessage(Resources.InvalidServerResponseFormat,
                (int)statusCode, Enum.GetName(typeof(HttpStatusCode), statusCode)));
        }

        public static Exception ConnectionStringMissing()
        {
            return new ArgumentException(Resources.ConnectionStringMissing);
        }

        public static Exception ServiceUrlMissing()
        {
            return new ArgumentException(Resources.ServiceUrlMissing);
        }

        public static Exception TableNameMissing()
        {
            return new ArgumentException(Resources.TableNameMissing);
        }

        public static Exception AmbiguousFilter()
        {
            return new ArgumentException(Resources.AmbiguousFilter);
        }

        public static Exception SourceIsNotInitialized()
        {
            return new InvalidOperationException(Resources.SourceIsNotInitialized);
        }

        public static Exception FailedToCreateScanner(HttpStatusCode statusCode)
        {
            return new WebException(FormatMessage(Resources.FailedToCreateScannerFormat,
                (int)statusCode, Enum.GetName(typeof(HttpStatusCode), statusCode)));
        }

        public static Exception ScannerLocationMissing()
        {
            return new WebException(Resources.ScannerLocationMissing);
        }

        public static Exception InvalidScannerLocation()
        {
            return new WebException(Resources.InvalidScannerLocation);
        }
    }
}
