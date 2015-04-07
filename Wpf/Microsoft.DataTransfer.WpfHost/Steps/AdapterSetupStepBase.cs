using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.Helpers;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.DataTransfer.WpfHost.Steps
{
    abstract class AdapterSetupStepBase : NavigationStepBase
    {
        protected IDataTransferService TransferService { get; private set; }
        protected IDataAdapterConfigurationProvidersCollection ConfigurationProviders { get; private set; }

        public AdapterSetupStepBase(IDataTransferService transferService, IDataAdapterConfigurationProvidersCollection configurationProviders,
            IDataTransferModel transferModel)
                : base(transferModel)
        {
            TransferService = transferService;
            ConfigurationProviders = configurationProviders;

            transferModel.PropertyChanged += OnTransferModelPropertyChanged;
        }

        protected void Initialize()
        {
            OnTransferModelPropertyChanged(this, new PropertyChangedEventArgs(DataTransferModelProperties.HasImportStarted));
        }

        private void OnTransferModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsValid = IsTransferModelValid();

            if (e.PropertyName == DataTransferModelProperties.HasImportStarted)
                IsAllowed = !TransferModel.HasImportStarted;
        }

        protected abstract bool IsTransferModelValid();
    }
}
