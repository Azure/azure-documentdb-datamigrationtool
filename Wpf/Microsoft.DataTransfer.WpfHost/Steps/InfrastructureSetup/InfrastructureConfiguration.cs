using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Errors;
using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;

namespace Microsoft.DataTransfer.WpfHost.Steps.InfrastructureSetup
{
    sealed class InfrastructureConfiguration : BindableBase, IInfrastructureConfiguration
    {
        private string errorLog;
        private ErrorDetails? errorDetails;

        public string ErrorLog
        {
            get { return errorLog; }
            set { SetProperty(ref errorLog, value); }
        }

        public bool OverwriteErrorLog { get { return true; } }

        public ErrorDetails? ErrorDetails
        {
            get { return errorDetails; }
            set { SetProperty(ref errorDetails, value); }
        }

        public InfrastructureConfiguration()
        {
            ErrorDetails = InfrastructureDefaults.Current.ErrorDetails;
        }
    }
}
