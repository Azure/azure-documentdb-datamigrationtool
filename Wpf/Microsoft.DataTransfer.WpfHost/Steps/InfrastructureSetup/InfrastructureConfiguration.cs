using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;

namespace Microsoft.DataTransfer.WpfHost.Steps.InfrastructureSetup
{
    sealed class InfrastructureConfiguration : BindableBase, IInfrastructureConfiguration
    {
        private string errorLog;

        public string ErrorLog
        {
            get { return errorLog; }
            set { SetProperty(ref errorLog, value); }
        }

        public bool OverwriteErrorLog { get { return true; } }
    }
}
