using Microsoft.Azure.Storage.RetryPolicies;
using Microsoft.DataTransfer.AzureTable.Shared;

namespace Microsoft.DataTransfer.AzureTable.Client
{
    static class AzureTableClientHelper
    {
        public static LocationMode? ToSdkLocationMode(AzureStorageLocationMode? locationMode)
        {
            if (!locationMode.HasValue)
                locationMode = Defaults.Current.LocationMode;

            switch (locationMode)
            {
                case AzureStorageLocationMode.PrimaryOnly:
                    return LocationMode.PrimaryOnly;
                case AzureStorageLocationMode.PrimaryThenSecondary:
                    return LocationMode.PrimaryThenSecondary;
                case AzureStorageLocationMode.SecondaryOnly:
                    return LocationMode.SecondaryOnly;
                case AzureStorageLocationMode.SecondaryThenPrimary:
                    return LocationMode.SecondaryThenPrimary;
                default:
                    return null;
            }
        }
    }
}
