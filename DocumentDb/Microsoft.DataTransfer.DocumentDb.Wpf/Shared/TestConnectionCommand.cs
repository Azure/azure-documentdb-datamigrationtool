using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    sealed class TestConnectionCommand : SynchronizedAsyncCommand
    {
        private DocumentDbProbeClient probeClient;

        public TestConnectionCommand()
        {
            probeClient = new DocumentDbProbeClient();
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            var configuration = parameter as IDocumentDbAdapterConfiguration;

            if (configuration == null)
                return;

            await probeClient.TestConnection(configuration.ConnectionString, configuration.ConnectionMode);

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
