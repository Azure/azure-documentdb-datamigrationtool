using Microsoft.DataTransfer.ServiceModel.Entities;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Steps.SourceSetup
{
    sealed class SourceSetupViewModel : AdapterSetupViewModelBase
    {
        public SourceSetupViewModel(IReadOnlyDictionary<string, IDataAdapterDefinition> adapters,
            Func<string, IDataAdapterConfigurationProvider> configurationProvidersSource, IDataTransferModel transferModel)
            : base(adapters, configurationProvidersSource, m => m.SourceAdapterName, transferModel) { }

        protected override void SetTransferModelAdapterConfiguration(object configuration)
        {
            TransferModel.SourceConfiguration = configuration;
        }
    }
}
