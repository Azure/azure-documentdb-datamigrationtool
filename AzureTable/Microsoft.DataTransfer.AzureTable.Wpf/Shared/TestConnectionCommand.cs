using Microsoft.DataTransfer.AzureTable.Client;
using Microsoft.DataTransfer.AzureTable.Shared;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Shared
{
    sealed class TestConnectionCommand : SynchronizedAsyncCommand
    {
        private AzureTableProbeClient probeClient;

        public TestConnectionCommand()
        {
            probeClient = new AzureTableProbeClient();
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            var configuration = parameter as IAzureTableAdapterConfiguration;

            if (configuration == null)
                return;

            await probeClient.TestConnection(configuration.ConnectionString, configuration.LocationMode);

            MessageBox.Show(
                Resources.TestConnectionSuccessMessage,
                Resources.TestConnectionResultTitle,
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected override void HandleError(Exception error)
        {
            MessageBox.Show(
                error == null ? CommonResources.UnknownError : error.Message,
                Resources.TestConnectionResultTitle,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
