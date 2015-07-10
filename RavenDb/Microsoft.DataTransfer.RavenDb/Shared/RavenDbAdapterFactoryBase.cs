using Microsoft.DataTransfer.Extensibility.Basics;
using System;

namespace Microsoft.DataTransfer.RavenDb.Shared
{
    abstract class RavenDbAdapterFactoryBase : DataAdapterFactoryBase
    {
        protected static void ValidateBaseConfiguration(IRavenDbAdapterConfiguration configuration)
        {
            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();
        }
    }
}
