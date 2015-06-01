using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.ServiceModel.Entities;
using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Microsoft.DataTransfer.WpfHost.Steps
{
    abstract class AdapterSetupViewModelBase : BindableBase
    {
        private static readonly string ConfigurationPropertyName = ObjectExtensions.MemberName<IDataAdapterConfigurationProvider>(p => p.Configuration);

        private Func<string, IDataAdapterConfigurationProvider> configurationProvidersSource;
        private IDataAdapterConfigurationProvider currentConfigurationProviders;

        public IReadOnlyDictionary<string, IDataAdapterDefinition> Adapters { get; private set; }
        public IDataTransferModel TransferModel { get; private set; }

        public IDataAdapterConfigurationProvider CurrentConfigurationProvider
        {
            get { return currentConfigurationProviders; }
            private set { SetProperty(ref currentConfigurationProviders, value); }
        }

        public AdapterSetupViewModelBase(IReadOnlyDictionary<string, IDataAdapterDefinition> adapters,
            Func<string, IDataAdapterConfigurationProvider> configurationProvidersSource,
            Expression<Func<IDataTransferModel, string>> adapterIdProperty, IDataTransferModel transferModel)
        {
            Adapters = adapters;
            this.configurationProvidersSource = configurationProvidersSource;
            TransferModel = transferModel;

            transferModel.Subscribe(adapterIdProperty, UpdateCurrentProvider);
        }

        private void UpdateCurrentProvider(string adapterName)
        {
            if (CurrentConfigurationProvider != null)
                CurrentConfigurationProvider.PropertyChanged -= UpdateContextConfiguration;

            CurrentConfigurationProvider = configurationProvidersSource(adapterName);

            if (CurrentConfigurationProvider != null)
            {
                SetTransferModelAdapterConfiguration(CurrentConfigurationProvider.Configuration);
                CurrentConfigurationProvider.PropertyChanged += UpdateContextConfiguration;
            }
            else
            {
                SetTransferModelAdapterConfiguration(null);
            }
        }

        private void UpdateContextConfiguration(object sender, PropertyChangedEventArgs e)
        {
            var provider = sender as IDataAdapterConfigurationProvider;

            if (provider == null)
                return;

            if (e.PropertyName == ConfigurationPropertyName)
                SetTransferModelAdapterConfiguration(provider.Configuration);
        }

        protected abstract void SetTransferModelAdapterConfiguration(object configuration);
    }
}
