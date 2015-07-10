using Microsoft.DataTransfer.Extensibility.Basics.Source;

namespace Microsoft.DataTransfer.DynamoDb.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from Amazon DynamoDB.
    /// </summary>
    public sealed class DynamoDbSourceAdapterFactory : DataSourceAdapterFactoryWrapper<IDynamoDbSourceAdapterConfiguration>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DynamoDbSourceAdapterFactory" />.
        /// </summary>
        public DynamoDbSourceAdapterFactory()
            : base(new DynamoDbSourceAdapterInternalFactory()) { }
    }
}
