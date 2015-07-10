using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility.Basics;
using System;

namespace Microsoft.DataTransfer.HBase.Shared
{
    abstract class HBaseAdapterFactoryBase : DataAdapterFactoryBase
    {
        protected static void ValidateBaseConfiguration(IHBaseAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();
        }
    }
}
