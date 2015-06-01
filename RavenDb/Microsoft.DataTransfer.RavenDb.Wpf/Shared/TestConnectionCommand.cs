using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.RavenDb.Client;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Shared
{
    sealed class TestConnectionCommand : SynchronizedAsyncCommand
    {
        private RavenDbProbeClient probeClient;

        public TestConnectionCommand()
        {
            probeClient = new RavenDbProbeClient();
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await probeClient.TestConnectionAsync(parameter as string);

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
