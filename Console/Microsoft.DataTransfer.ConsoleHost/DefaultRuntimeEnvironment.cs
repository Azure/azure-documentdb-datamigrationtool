using Autofac;
using Autofac.Configuration;
using Microsoft.DataTransfer.ConsoleHost.App.Handlers;
using Microsoft.DataTransfer.ConsoleHost.Configuration;
using Microsoft.DataTransfer.ConsoleHost.DataAdapters;
using Microsoft.DataTransfer.ConsoleHost.Extensibility;
using Microsoft.DataTransfer.Core;

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
                .RegisterModule<CoreServiceImplementation>()
                .RegisterModule(new TypeLimitedComponentsConfigurationSettingsReader(
                    "dataTransfer.configurationFactories", typeof(IDataAdapterConfigurationFactory)));

            builder.RegisterAggregationDecorator<IDataAdapterConfigurationFactory>((c, f) =>
                new DataAdapterConfigurationFactoryDispatcher(f));

            RegisterActions(builder);
        }

        private void RegisterActions(ContainerBuilder builder)
        {
            builder
                .RegisterType<HelpHandler>()
                .As<IHelpHandler>()
                .SingleInstance();

            builder
                .Register(c => new CommandLineOneTimeTransferConfiguration(arguments))
                .As<IOneTimeDataTransferConfiguration>();

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
