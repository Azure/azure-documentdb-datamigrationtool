using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;
using System;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Steps.Summary
{
    sealed class GenerateCommandLineCommand : CommandBase
    {
        private readonly ICommandLineProvider commandLineProvider;

        private IInfrastructureConfiguration infrastructureConfiguration;

        private string sourceName;
        private IDataAdapterConfigurationProvider sourceConfigurationProvider;

        private string sinkName;
        private IDataAdapterConfigurationProvider sinkConfigurationProvider;

        public IInfrastructureConfiguration InfrastructureConfiguration
        {
            get { return infrastructureConfiguration; }
            set { SetProperty(ref infrastructureConfiguration, value); }
        }

        public string SourceName
        {
            get { return sourceName; }
            set { SetProperty(ref sourceName, value); }
        }

        public IDataAdapterConfigurationProvider SourceConfigurationProvider
        {
            get { return sourceConfigurationProvider; }
            set { SetProperty(ref sourceConfigurationProvider, value); }
        }

        public string SinkName
        {
            get { return sinkName; }
            set { SetProperty(ref sinkName, value); }
        }

        public IDataAdapterConfigurationProvider SinkConfigurationProvider
        {
            get { return sinkConfigurationProvider; }
            set { SetProperty(ref sinkConfigurationProvider, value); }
        }

        public GenerateCommandLineCommand(ICommandLineProvider commandLineProvider)
        {
            Guard.NotNull("commandLineProvider", commandLineProvider);
            this.commandLineProvider = commandLineProvider;
        }

        public override bool CanExecute(object parameter)
        {
            return infrastructureConfiguration != null &&
                sourceName != null && sourceConfigurationProvider != null &&
                sinkName != null && sinkConfigurationProvider != null;
        }

        public override void Execute(object parameter)
        {
            new CommandLinePreviewWindow
            {
                DataContext = commandLineProvider.Get(
                    infrastructureConfiguration,
                    sourceName, sourceConfigurationProvider.CommandLineArguments,
                    sinkName, sinkConfigurationProvider.CommandLineArguments),
                Owner = Application.Current.MainWindow
            }.ShowDialog();
        }

        private void SetProperty<T>(ref T storage, T value)
        {
            if (Object.Equals(storage, value))
                return;

            storage = value;
            RaiseCanExecuteChanged();
        }
    }
}
