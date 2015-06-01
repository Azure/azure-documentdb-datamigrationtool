using Microsoft.DataTransfer.ServiceModel;
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
        private ITransferStatisticsFactory statisticsFactory;
        private IErrorHandler errorHandler;

        public override string Title
        {
            get { return StepsResources.ImportStepTitle; }
        }

        public ImportStep(IDataTransferService transferService, ITransferStatisticsFactory statisticsFactory, IErrorHandler errorHandler, IDataTransferModel transferModel)
            : base(transferModel)
        {
            this.transferService = transferService;
            this.statisticsFactory = statisticsFactory;
            this.errorHandler = errorHandler;

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
                var statistics = statisticsFactory.Create(TransferModel.InfrastructureConfiguration);
                operationContext = new ImportOperationContext(statistics, Presenter.DataContext as ImportViewModel);
                operationContext.ViewModel.IsImportRunning = true;

                using (var cancellation = TransferModel.ImportCancellation = new CancellationTokenSource())
                {
                    using (var timer = new Timer(UpdateStatistics, operationContext, TimeSpan.Zero, TimeSpan.FromSeconds(1)))
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

            if (criticalError != null)
                errorHandler.Handle(criticalError);
        }

        private void UpdateStatistics(object state)
        {
            var context = state as ImportOperationContext;
            if (context == null)
                return;

            var snapshot = context.Statistics.GetSnapshot();

            context.ViewModel.ElapsedTime = snapshot.ElapsedTime;
            context.ViewModel.Transferred = snapshot.Transferred;
            context.ViewModel.Failed = snapshot.Failed;
            context.ViewModel.Errors = snapshot.GetErrors();
        }
    }
}
