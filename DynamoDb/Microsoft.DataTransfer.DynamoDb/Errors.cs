using Amazon.DynamoDBv2;
using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.DynamoDb
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception ConnectionStringMissing()
        {
            return new ArgumentException(Resources.ConnectionStringMissing);
        }

        public static Exception AccessKeyMissing()
        {
            return new ArgumentException(Resources.AccessKeyMissing);
        }

        public static Exception SecretKeyMissing()
        {
            return new ArgumentException(Resources.SecretKeyMissing);
        }

        public static Exception ServiceUrlMissing()
        {
            return new ArgumentException(Resources.ServiceUrlMissing);
        }

        public static Exception AmbiguousRequest()
        {
            return new ArgumentException(Resources.AmbiguousRequest);
        }

        public static Exception RequestMissing()
        {
            return new ArgumentException(Resources.RequestMissing);
        }

        public static Exception FailedToListTables(string serverResponse)
        {
            return new AmazonDynamoDBException(FormatMessage(Resources.FailedToListTablesFormat, serverResponse));
        }

        public static Exception TypeConversionNotSupported(Type objectType, Type converterType)
        {
            return new NotSupportedException(FormatMessage(Resources.TypeConversionNotSupportedFormat, objectType, converterType));
        }

        public static Exception UnsupportedBinaryFormat(string tokenType)
        {
            return new NotSupportedException(FormatMessage(Resources.UnsupportedBinaryFormatFormat, tokenType));
        }
    }
}
