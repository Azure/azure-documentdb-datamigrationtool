using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Core.Autofac;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.FactoryAdapters
{
    abstract class DataAdapterFactoryAdapterBase<TFactory, TDataAdapter> : IDataAdapterFactoryAdapter<TDataAdapter>
        where TFactory : class, IDataAdapterFactory
    {
        private static readonly Type OpenGenericFactoryType;

        public string DisplayName { get; private set; }
        public string Description { get { return factory.Description; } }
        public Type ConfigurationType { get; private set; }

        private TFactory factory;
        private MethodInfo createMethod;

        static DataAdapterFactoryAdapterBase()
        {
            if (!typeof(TFactory).IsGenericType)
                throw Errors.NonGenericDataAdapterFactoryType(typeof(TFactory));
            OpenGenericFactoryType = typeof(TFactory).GetGenericTypeDefinition();
        }

        public DataAdapterFactoryAdapterBase(TFactory factory, string displayName)
        {
            Guard.NotNull("factory", factory);

            this.factory = factory;
            DisplayName = displayName;

            ConfigurationType = GetConfigurationType(factory.GetType());
            createMethod = factory.GetType().GetMethod("CreateAsync", new[] { ConfigurationType, typeof(IDataTransferContext), typeof(CancellationToken) });
        }

        public static Type GetConfigurationType(Type adapterFactoryType)
        {
            Guard.NotNull("adapterFactoryType", adapterFactoryType);

            var factoryInterface =
                adapterFactoryType
                    .FindInterfaces(TypesHelper.IsOpenGenericType, OpenGenericFactoryType)
                    .FirstOrDefault();

            if (factoryInterface == null)
                throw Errors.InvalidDataAdapterFactoryType(OpenGenericFactoryType, adapterFactoryType);

            return factoryInterface.GetGenericArguments()[0];
        }

        public Task<TDataAdapter> CreateAsync(object configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            if (configuration != null && !ConfigurationType.IsAssignableFrom(configuration.GetType()))
                throw Errors.InvalidDataAdapterConfigrationType(ConfigurationType, configuration.GetType());

            try
            {
                return (Task<TDataAdapter>)createMethod.Invoke(factory, new[] { configuration, context, cancellation });
            }
            catch (TargetInvocationException invocationException)
            {
                if (invocationException.InnerException != null)
                    throw invocationException.InnerException;

                throw;
            }
        }
    }
}
