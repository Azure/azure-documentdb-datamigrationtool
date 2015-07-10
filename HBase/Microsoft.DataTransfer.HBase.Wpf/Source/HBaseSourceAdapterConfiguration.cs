using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.HBase.Source;
using Microsoft.DataTransfer.HBase.Wpf.Shared;

namespace Microsoft.DataTransfer.HBase.Wpf.Source
{
    sealed class HBaseSourceAdapterConfiguration : HBaseAdapterConfiguration, IHBaseSourceAdapterConfiguration
    {
        public static readonly string TablePropertyName =
            ObjectExtensions.MemberName<IHBaseSourceAdapterConfiguration>(c => c.Table);

        public static readonly string FilterPropertyName =
            ObjectExtensions.MemberName<IHBaseSourceAdapterConfiguration>(c => c.Filter);

        public static readonly string FilterFilePropertyName =
            ObjectExtensions.MemberName<IHBaseSourceAdapterConfiguration>(c => c.FilterFile);

        public static readonly string ExcludeIdPropertyName =
            ObjectExtensions.MemberName<IHBaseSourceAdapterConfiguration>(c => c.ExcludeId);

        public static readonly string BatchSizePropertyName =
            ObjectExtensions.MemberName<IHBaseSourceAdapterConfiguration>(c => c.BatchSize);

        private string table;

        private bool useFilterFile;
        private string filter;
        private string filterFile;

        private bool excludeId;
        private int? batchSize;

        public string Table
        {
            get { return table; }
            set { SetProperty(ref table, value, ValidateNonEmptyString); }
        }

        public bool UseFilterFile
        {
            get { return useFilterFile; }
            set { SetProperty(ref useFilterFile, value); }
        }

        public string Filter
        {
            get { return useFilterFile ? null : filter; }
            set { SetProperty(ref filter, value); }
        }

        public string FilterFile
        {
            get { return useFilterFile ? filterFile : null; }
            set { SetProperty(ref filterFile, value); }
        }

        public bool ExcludeId
        {
            get { return excludeId; }
            set { SetProperty(ref excludeId, value); }
        }

        public int? BatchSize
        {
            get { return batchSize; }
            set { SetProperty(ref batchSize, value, ValidatePositiveInteger); }
        }

        public HBaseSourceAdapterConfiguration()
        {
            BatchSize = Defaults.Current.SourceBatchSize;
        }
    }
}
