using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.MongoDb.Client;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Shared
{
    sealed class TestConnectionCommand : SynchronizedAsyncCommand
    {
        private MongoDbProbeClient probeClient;

        public TestConnectionCommand()
        {
            probeClient = new MongoDbProbeClient();
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await probeClient.TestConnection(parameter as string, CancellationToken.None);

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
