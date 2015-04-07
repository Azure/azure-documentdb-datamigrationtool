using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Sql.Client;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.DataTransfer.Sql.Wpf.Shared
{
    sealed class TestConnectionCommand : SynchronizedAsyncCommand
    {
        private SqlProbeClient probeClient;

        public TestConnectionCommand()
        {
            probeClient = new SqlProbeClient();
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await probeClient.TestConnection(parameter as string);

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
