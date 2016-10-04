using Microsoft.DataTransfer.AzureTable.Shared;
using Microsoft.DataTransfer.WpfHost.Basics.ValueConverters;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Shared
{
    sealed class AzureStorageLocationModeValueConverter : EnumDisplayNameValueConverter.Base<AzureStorageLocationMode>
    {
        private static IDictionary<AzureStorageLocationMode, string> KnownValues =
            new Dictionary<AzureStorageLocationMode, string>
            {
                { AzureStorageLocationMode.PrimaryOnly, Resources.AzureStorageLocationMode_PrimaryOnly },
                { AzureStorageLocationMode.PrimaryThenSecondary, Resources.AzureStorageLocationMode_PrimaryThenSecondary },
                { AzureStorageLocationMode.SecondaryOnly, Resources.AzureStorageLocationMode_SecondaryOnly },
                { AzureStorageLocationMode.SecondaryThenPrimary, Resources.AzureStorageLocationMode_SecondaryThenPrimary }
            };

        public AzureStorageLocationModeValueConverter()
            : base(KnownValues) { }
    }
}
