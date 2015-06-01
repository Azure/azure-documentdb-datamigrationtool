using Microsoft.DataTransfer.ServiceModel.Statistics;

namespace Microsoft.DataTransfer.WpfHost.Steps.Import
{
    sealed class ImportOperationContext
    {
        public readonly ITransferStatistics Statistics;
        public readonly ImportViewModel ViewModel;

        public ImportOperationContext(ITransferStatistics statistics, ImportViewModel viewModel)
        {
            Statistics = statistics;
            ViewModel = viewModel;
        }
    }
}
