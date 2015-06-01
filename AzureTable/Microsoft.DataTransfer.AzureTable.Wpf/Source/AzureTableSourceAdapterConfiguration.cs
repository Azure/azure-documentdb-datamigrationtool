using Microsoft.DataTransfer.AzureTable.Source;
using Microsoft.DataTransfer.AzureTable.Wpf.Shared;
using Microsoft.DataTransfer.Basics.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Source
{
    sealed class AzureTableSourceAdapterConfiguration : AzureTableAdapterConfiguration, IAzureTableSourceAdapterConfiguration
    {
        public static readonly string TablePropertyName =
            ObjectExtensions.MemberName<IAzureTableSourceAdapterConfiguration>(c => c.Table);

        public static readonly string InternalFieldsPropertyName =
            ObjectExtensions.MemberName<IAzureTableSourceAdapterConfiguration>(c => c.InternalFields);

        public static readonly string FilterPropertyName =
            ObjectExtensions.MemberName<IAzureTableSourceAdapterConfiguration>(c => c.Filter);

        public static readonly string ProjectionPropertyName =
            ObjectExtensions.MemberName<IAzureTableSourceAdapterConfiguration>(c => c.Projection);

        private string table;
        private AzureTableInternalFields? internalFields;
        private string filter;
        private ObservableCollection<string> projection;

        public string Table
        {
            get { return table; }
            set { SetProperty(ref table, value, ValidateNonEmptyString); }
        }

        public AzureTableInternalFields? InternalFields
        {
            get { return internalFields; }
            set { SetProperty(ref internalFields, value); }
        }

        public string Filter
        {
            get { return filter; }
            set { SetProperty(ref filter, value); }
        }

        public IEnumerable<string> Projection
        {
            get { return projection; }
        }

        public ObservableCollection<string> EditableProjection
        {
            get { return projection; }
            private set { SetProperty(ref projection, value); }
        }

        public AzureTableSourceAdapterConfiguration()
        {
            EditableProjection = new ObservableCollection<string>();

            InternalFields = Defaults.Current.SourceInternalFields;
        }
    }
}
