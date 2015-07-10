using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.DynamoDb.Wpf.Source
{
    /// <summary>
    /// Provides configuration for DynamoDB data source.
    /// </summary>
    public sealed class DynamoDbSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="DynamoDbSourceAdapterConfigurationProvider" />.
        /// </summary>
        public DynamoDbSourceAdapterConfigurationProvider()
            : base(new DynamoDbSourceAdapterInternalConfigurationProvider()) { }
    }
}
