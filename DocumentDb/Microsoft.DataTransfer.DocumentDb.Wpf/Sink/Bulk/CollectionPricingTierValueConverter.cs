using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.WpfHost.Basics.ValueConverters;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Bulk
{
    sealed class CollectionPricingTierValueConverter : EnumDisplayNameValueConverter.Base<CollectionPricingTier>
    {
        private static IDictionary<CollectionPricingTier, string> KnownValues =
            new Dictionary<CollectionPricingTier, string>
            {
                { CollectionPricingTier.S1, Resources.CollectionPricingTier_S1 },
                { CollectionPricingTier.S2, Resources.CollectionPricingTier_S2 },
                { CollectionPricingTier.S3, Resources.CollectionPricingTier_S3 }
            };

        public CollectionPricingTierValueConverter()
            : base(KnownValues) { }
    }
}
