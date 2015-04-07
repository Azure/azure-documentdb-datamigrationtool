using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Entities;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Steps.Summary
{
    sealed class SummaryStep : NavigationStepBase, ISummaryStep
    {
        private readonly IDataTransferService transferService;
        private readonly IDataAdapterConfigurationProvidersCollection configurationProviders;
        private readonly ICommandLineProvider commandLineProvider;

        public override string Title
        {
            get { return StepsResources.SummaryStepTitle; }
        }

        public SummaryStep(IDataTransferService transferService, IDataAdapterConfigurationProvidersCollection configurationProviders,
            ICommandLineProvider commandLineProvider, IDataTransferModel transferModel)
                : base(transferModel)
        {
            Guard.NotNull("transferService", transferService);
            Guard.NotNull("configurationProviders", configurationProviders);
            Guard.NotNull("transferModel", transferModel);

            this.transferService = transferService;
            this.configurationProviders = configurationProviders;
            this.commandLineProvider = commandLineProvider;

            transferModel.PropertyChanged += OnTransferModelPropertyChanged;
            OnTransferModelPropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        private void OnTransferModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsAllowed = !TransferModel.HasImportStarted &&
                TransferModel.SourceAdapterName != null && TransferModel.SourceConfiguration != null &&
                TransferModel.SinkAdapterName != null && TransferModel.SinkConfiguration != null;
        }

        protected override UserControl CreatePresenter()
        {
            return new SummaryPage()
            {
                DataContext = new SummaryPageViewModel(
                    GetDisplayNames(transferService.GetKnownSources), GetDisplayNames(transferService.GetKnownSinks),
                    configurationProviders, commandLineProvider, TransferModel) 
            };
        }

        private static IReadOnlyDictionary<string, string> GetDisplayNames(Func<IReadOnlyDictionary<string, IDataAdapterDefinition>> adaptersProvider)
        {
            Guard.NotNull("adaptersProvider", adaptersProvider);
            return adaptersProvider().ToDictionary(a => a.Key, a => a.Value.DisplayName);
        }
    }
}
