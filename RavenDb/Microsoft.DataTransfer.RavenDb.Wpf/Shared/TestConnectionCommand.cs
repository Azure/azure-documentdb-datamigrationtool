using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Shared
{
    sealed class TestConnectionCommand : SynchronizedAsyncCommand
    {
        protected override async Task ExecuteAsync(object parameter)
        {
            var connectionStringOptions = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionString(parameter as string);
            connectionStringOptions.Parse();

            var options = connectionStringOptions.ConnectionStringOptions;

            var localDocumentStore = new DocumentStore();


            if (options.ResourceManagerId != Guid.Empty)
                localDocumentStore.ResourceManagerId = options.ResourceManagerId;
            if (options.Credentials != null)
                localDocumentStore.Credentials = options.Credentials;
            if (string.IsNullOrEmpty(options.Url) == false)
                localDocumentStore.Url = options.Url;
            if (string.IsNullOrEmpty(options.DefaultDatabase) == false)
                localDocumentStore.DefaultDatabase = options.DefaultDatabase;
            if (string.IsNullOrEmpty(options.ApiKey) == false)
                localDocumentStore.ApiKey = options.ApiKey;

            localDocumentStore.EnlistInDistributedTransactions = options.EnlistInDistributedTransactions;

            localDocumentStore.Initialize();

            await localDocumentStore.AsyncDatabaseCommands.GetStatisticsAsync();
            
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
