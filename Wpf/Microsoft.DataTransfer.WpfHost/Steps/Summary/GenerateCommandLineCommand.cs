using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Steps.Summary
{
    sealed class GenerateCommandLineCommand : CommandBase
    {
        private readonly ICommandLineProvider commandLineProvider;

        private string sourceName;
        private IDataAdapterConfigurationProvider sourceConfigurationProvider;

        private string sinkName;
        private IDataAdapterConfigurationProvider sinkConfigurationProvider;

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
            this.commandLineProvider = commandLineProvider;
        }

        public override bool CanExecute(object parameter)
        {
            return sourceName != null && sourceConfigurationProvider != null &&
                sinkName != null && sinkConfigurationProvider != null;
        }

        public override void Execute(object parameter)
        {
            new CommandLinePreviewWindow
            {
                DataContext = commandLineProvider.Get(
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
