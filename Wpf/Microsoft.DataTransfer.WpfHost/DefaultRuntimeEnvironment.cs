using Autofac;
using Autofac.Configuration;
using Microsoft.DataTransfer.Core;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.Model;
using Microsoft.DataTransfer.WpfHost.Model.Statistics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using Microsoft.DataTransfer.WpfHost.Shell;
using Microsoft.DataTransfer.WpfHost.Steps.Import;
using Microsoft.DataTransfer.WpfHost.Steps.InfrastructureSetup;
using Microsoft.DataTransfer.WpfHost.Steps.SinkSetup;
using Microsoft.DataTransfer.WpfHost.Steps.SourceSetup;
using Microsoft.DataTransfer.WpfHost.Steps.Summary;
using Microsoft.DataTransfer.WpfHost.Steps.Welcome;

namespace Microsoft.DataTransfer.WpfHost
{
    sealed class DefaultRuntimeEnvironment : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterModule<CoreServiceImplementation>()
                .RegisterModule(new TypeLimitedComponentsConfigurationSettingsReader(
                    "dataTransfer.configurationProviders", typeof(IDataAdapterConfigurationProvider)));

            builder
                .RegisterType<ApplicationController>()
                .As<IApplicationController>()
                .SingleInstance();

            builder
                .RegisterType<MainWindowViewModel>()
                .As<IMainWindowViewModel>();

            RegisterModel(builder);
            RegisterSteps(builder);
        }

        private static void RegisterModel(ContainerBuilder builder)
        {
            builder
                .RegisterDecorator<ITransferStatisticsFactory>((c, f) => new ObservableErrorsTransferStatisticsFactory(f))
                .As<ITransferStatisticsFactory>();

            builder
                .RegisterType<ImportSharedStorage>()
                .As<IImportSharedStorage>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<DataAdapterConfigurationProvidersCollection>()
                .As<IDataAdapterConfigurationProvidersCollection>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CommandLineProvider>()
                .As<ICommandLineProvider>()
                .SingleInstance();

            builder
                .RegisterType<DataTransferModel>()
                .As<IDataTransferModel>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<NavigationService>()
                .As<INavigationService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ErrorHandler>()
                .As<IErrorHandler>()
                .SingleInstance();

            builder
                .RegisterType<TaskBarService>()
                .As<ITaskBarService>()
                .SingleInstance();
        }

        private static void RegisterSteps(ContainerBuilder builder)
        {
            // Register in reverse order, looks like autofac actually respects the order, but in reverse :)

            builder
                .RegisterType<ImportStep>()
                .As<INavigationStep>();

            builder
                .RegisterType<SummaryStep>()
                .As<INavigationStep>();

            builder
                .RegisterType<InfrastructureSetupStep>()
                .As<INavigationStep>();

            builder
                .RegisterType<SinkSetupStep>()
                .As<INavigationStep>();

            builder
                .RegisterType<SourceSetupStep>()
                .As<INavigationStep>();

            builder
                .RegisterType<WelcomeStep>()
                .As<INavigationStep>();
        }
    }
}
