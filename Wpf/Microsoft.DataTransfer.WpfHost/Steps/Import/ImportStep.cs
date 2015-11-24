using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Errors;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Steps.Import
{
    sealed class ImportStep : NavigationStepBase, IActionStep
    {
        private IDataTransferService transferService;
        private IErrorDetailsProviderFactory errorDetailsProviderFactory;
        private ITransferStatisticsFactory statisticsFactory;
        private IErrorHandler errorHandler;
        private ITaskBarService taskBarService;

        public override string Title
        {
            get { return StepsResources.ImportStepTitle; }
        }

        public ImportStep(IDataTransferService transferService, IErrorDetailsProviderFactory errorDetailsProviderFactory,
            ITransferStatisticsFactory statisticsFactory, IErrorHandler errorHandler, ITaskBarService taskBarService,
            IDataTransferModel transferModel)
                : base(transferModel)
        {
            this.transferService = transferService;
            this.errorDetailsProviderFactory = errorDetailsProviderFactory;
            this.statisticsFactory = statisticsFactory;
            this.errorHandler = errorHandler;
            this.taskBarService = taskBarService;

            transferModel.Subscribe(m => m.HasImportStarted, OnImportStateChanged);
        }

        private void OnImportStateChanged(bool hasImportStarted)
        {
            IsAllowed = hasImportStarted;
        }

        protected override UserControl CreatePresenter()
        {
            return new ImportPage() { DataContext = new ImportViewModel() };
        }

        public async Task Execute()
        {
            TransferModel.HasImportStarted = true;

            ImportOperationContext operationContext = null;
            Exception criticalError = null;
            try
            {
                using (var cancellation = TransferModel.ImportCancellation = new CancellationTokenSource())
                {
                    var statistics = await statisticsFactory.Create(errorDetailsProviderFactory.Create(TransferModel.InfrastructureConfiguration),
                        TransferModel.InfrastructureConfiguration, cancellation.Token);

                    UpdateStatistics(
                        operationContext = new ImportOperationContext(statistics, Presenter.DataContext as ImportViewModel)
                        {
                            ViewModel = { IsImportRunning = true }
                        });
                
                    using (new DisposableDispatcherTimer<ImportOperationContext>(
                        GetProgressUpdateInterval(TransferModel.InfrastructureConfiguration), UpdateStatistics, operationContext))
                    {
                        await Task.Run(() =>
                            transferService.TransferAsync(
                            // From
                                TransferModel.SourceAdapterName, TransferModel.SourceConfiguration,
                            // To
                                TransferModel.SinkAdapterName, TransferModel.SinkConfiguration,
                            // With statistics
                                statistics,
                            // Allow cancellation
                                cancellation.Token));
                    }
                }
            }
            catch (Exception error)
            {
                // Preserve error and allow finally block to execute right away
                criticalError = error;
            }
            finally
            {
                TransferModel.ImportCancellation = null;

                if (operationContext != null)
                {
                    operationContext.ViewModel.IsImportRunning = false;
                    UpdateStatistics(operationContext);
                }
            }

            taskBarService.Notify();

            if (criticalError != null)
                errorHandler.Handle(criticalError);
        }

        private static TimeSpan GetProgressUpdateInterval(ITransferStatisticsConfiguration statisticsConfiguration)
        {
            return statisticsConfiguration.ProgressUpdateInterval
                ?? InfrastructureDefaults.Current.ProgressUpdateInterval;
        }

        private void UpdateStatistics(ImportOperationContext context)
        {
            var snapshot = context.Statistics.GetSnapshot();

            context.ViewModel.ElapsedTime = snapshot.ElapsedTime;
            context.ViewModel.Transferred = snapshot.Transferred;
            context.ViewModel.Failed = snapshot.Failed;
            context.ViewModel.Errors = snapshot.GetErrors();
        }
    }
}
