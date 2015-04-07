using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System.Threading;

namespace Microsoft.DataTransfer.WpfHost.Shell.Navigation
{
    sealed class CancelImportCommand : CommandBase
    {
        private IDataTransferModel transferModel;

        public CancelImportCommand(IDataTransferModel transferModel)
        {
            this.transferModel = transferModel;

            transferModel.Subscribe(m => m.ImportCancellation, OnCancellationChanged);
        }

        public override bool CanExecute(object parameter)
        {
            return transferModel.ImportCancellation != null && !transferModel.ImportCancellation.IsCancellationRequested;
        }

        public override void Execute(object parameter)
        {
            var cancellation = transferModel.ImportCancellation;
            if (cancellation != null)
                cancellation.Cancel();

            RaiseCanExecuteChanged();
        }

        private void OnCancellationChanged(CancellationTokenSource cancellation)
        {
            RaiseCanExecuteChanged();
        }
    }
}
