using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Core
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception InvalidDataAdapterConfigrationType(Type expected, Type actual)
        {
            return new InvalidOperationException(FormatMessage(
                Resources.InvalidDataAdapterConfigrationTypeFormat, expected, actual));
        }

        public static Exception InvalidDataAdapterFactoryType(Type expected, Type actual)
        {
            return new InvalidOperationException(FormatMessage(
                Resources.InvalidDataAdapterFactoryTypeFormat, expected, actual));
        }

        public static Exception UnknownDataSource(string name)
        {
            return new KeyNotFoundException(FormatMessage(Resources.UnknownDataSourceFormat, name));
        }

        public static Exception UnknownDataSink(string name)
        {
            return new KeyNotFoundException(FormatMessage(Resources.UnknownDataSinkFormat, name));
        }

        public static Exception NonGenericDataAdapterFactoryType(Type type)
        {
            return new InvalidOperationException(FormatMessage(Resources.NonGenericDataAdapterFactoryTypeFormat, type));
        }
    }
}
