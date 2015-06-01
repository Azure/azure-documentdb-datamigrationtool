using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.Helpers;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Steps.Summary
{
    sealed class SummaryPageViewModel : BindableBase
    {
        private readonly IReadOnlyDictionary<string, string> sourcesDisplayNames;
        private readonly IReadOnlyDictionary<string, string> sinksDisplayNames;
        private readonly IDataAdapterConfigurationProvidersCollection configurationProviders;
        private readonly IDataTransferModel transferModel;

        private GenerateCommandLineCommand generateCommandLine;

        private IInfrastructureConfiguration infrastructureConfiguration;

        private string sourceAdapterDisplayName;
        private IDataAdapterConfigurationProvider sourceConfigurationProvider;

        private string sinkAdapterDisplayName;
        private IDataAdapterConfigurationProvider sinkConfigurationProvider;

        public ICommand GenerateCommandLine { get { return generateCommandLine; } }

        public IInfrastructureConfiguration InfrastructureConfiguration
        {
            get { return infrastructureConfiguration; }
            set { SetProperty(ref infrastructureConfiguration, value); }
        }

        public string SourceAdapterDisplayName
        {
            get { return sourceAdapterDisplayName; }
            set { SetProperty(ref sourceAdapterDisplayName, value); }
        }

        public IDataAdapterConfigurationProvider SourceConfigurationProvider
        {
            get { return sourceConfigurationProvider; }
            private set { SetProperty(ref sourceConfigurationProvider, value); }
        }

        public string SinkAdapterDisplayName
        {
            get { return sinkAdapterDisplayName; }
            set { SetProperty(ref sinkAdapterDisplayName, value); }
        }

        public IDataAdapterConfigurationProvider SinkConfigurationProvider
        {
            get { return sinkConfigurationProvider; }
            private set { SetProperty(ref sinkConfigurationProvider, value); }
        }

        public SummaryPageViewModel(IReadOnlyDictionary<string, string> sourcesDisplayNames, IReadOnlyDictionary<string, string> sinksDisplayNames,
            IDataAdapterConfigurationProvidersCollection configurationProviders, ICommandLineProvider commandLineProvider, IDataTransferModel transferModel)
        {
            this.sourcesDisplayNames = sourcesDisplayNames;
            this.sinksDisplayNames = sinksDisplayNames;

            this.configurationProviders = configurationProviders;

            this.generateCommandLine = new GenerateCommandLineCommand(commandLineProvider);

            this.transferModel = transferModel;
            transferModel.PropertyChanged += OnTransferModelPropertyChanged;
            OnTransferModelPropertyChanged(this, new PropertyChangedEventArgs(DataTransferModelProperties.InfrastructureConfiguration));
            OnTransferModelPropertyChanged(this, new PropertyChangedEventArgs(DataTransferModelProperties.SourceAdapterName));
            OnTransferModelPropertyChanged(this, new PropertyChangedEventArgs(DataTransferModelProperties.SinkAdapterName));
        }

        private void OnTransferModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == DataTransferModelProperties.InfrastructureConfiguration)
            {
                InfrastructureConfiguration = transferModel.InfrastructureConfiguration;
                generateCommandLine.InfrastructureConfiguration = transferModel.InfrastructureConfiguration;
            }
            else if (e.PropertyName == DataTransferModelProperties.SourceAdapterName)
            {
                generateCommandLine.SourceName = transferModel.SourceAdapterName;
                SourceAdapterDisplayName = GetValueOrDefault(sourcesDisplayNames, transferModel.SourceAdapterName);
                generateCommandLine.SourceConfigurationProvider = SourceConfigurationProvider = 
                    configurationProviders.GetForSource(transferModel.SourceAdapterName);
            }
            else if (e.PropertyName == DataTransferModelProperties.SinkAdapterName)
            {
                generateCommandLine.SinkName = transferModel.SinkAdapterName;
                SinkAdapterDisplayName = GetValueOrDefault(sinksDisplayNames, transferModel.SinkAdapterName);
                generateCommandLine.SinkConfigurationProvider = SinkConfigurationProvider = 
                    configurationProviders.GetForSink(transferModel.SinkAdapterName);
            }
        }

        private static string GetValueOrDefault(IReadOnlyDictionary<string, string> dictionary, string key)
        {
            string value;

            if (dictionary.TryGetValue(key, out value))
                return value;

            return null;
        }
    }
}
