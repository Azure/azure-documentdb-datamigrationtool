using Autofac;
using Autofac.Configuration;
using Microsoft.DataTransfer.ConsoleHost.App.Handlers;
using Microsoft.DataTransfer.ConsoleHost.Configuration;
using Microsoft.DataTransfer.ConsoleHost.DataAdapters;
using Microsoft.DataTransfer.ConsoleHost.Extensibility;
using Microsoft.DataTransfer.Core;
using Microsoft.DataTransfer.ServiceModel.Errors;

namespace Microsoft.DataTransfer.ConsoleHost
{
    sealed class DefaultRuntimeEnvironment : Module
    {
        private readonly string[] arguments;

        public DefaultRuntimeEnvironment(string[] arguments)
        {
            this.arguments = arguments;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => CommandLineConfiguration.Parse(arguments))
                .As<IOneTimeDataTransferConfiguration>()
                .As<IRawInfrastructureConfiguration>()
                .SingleInstance();

            builder
                .RegisterModule<CoreServiceImplementation>()
                .RegisterModule(new TypeLimitedComponentsConfigurationSettingsReader(
                    "dataTransfer.configurationFactories", typeof(IDataAdapterConfigurationFactory)));

            builder
                .RegisterType<InfrastructureConfigurationFactory>()
                .As<IInfrastructureConfigurationFactory>()
                .SingleInstance();

            // Since console application can only be run once - lets take a shortcut and register
            // some components right away. As opposed to WPF, where we allow to restart the import,
            // in which case these will have to be created per every import operation.
            builder
                .Register(c => c
                    .Resolve<IInfrastructureConfigurationFactory>()
                    .Create(c.Resolve<IRawInfrastructureConfiguration>().InfrastructureConfiguration))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .Register(c => c.Resolve<IErrorDetailsProviderFactory>().Create(c.Resolve<IErrorDetailsConfiguration>()))
                .As<IErrorDetailsProvider>()
                .SingleInstance();

            builder.RegisterAggregationDecorator<IDataAdapterConfigurationFactory>(
                (c, f) => new DataAdapterConfigurationFactoryDispatcher(f));

            RegisterHandlers(builder);
        }

        private void RegisterHandlers(ContainerBuilder builder)
        {
            builder
                .RegisterType<HelpHandler>()
                .As<IHelpHandler>()
                .SingleInstance();

            builder
                .RegisterType<OneTimeDataTransferHandler>()
                .As<ITransferHandler>()
                .SingleInstance();

            builder
                .RegisterType<TransferStatisticsHandler>()
                .As<ITransferStatisticsHandler>()
                .SingleInstance();

            builder
                .RegisterType<ErrorHandler>()
                .As<IErrorHandler>()
                .SingleInstance();
        }
    }
}
