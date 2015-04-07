using Microsoft.DataTransfer.ServiceModel.Entities;

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
