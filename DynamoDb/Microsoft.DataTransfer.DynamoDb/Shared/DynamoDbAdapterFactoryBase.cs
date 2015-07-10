using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility.Basics;
using System;

namespace Microsoft.DataTransfer.DynamoDb.Shared
{
    abstract class DynamoDbAdapterFactoryBase : DataAdapterFactoryBase
    {
        protected static void ValidateBaseConfiguration(IDynamoDbAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();
        }
    }
}
