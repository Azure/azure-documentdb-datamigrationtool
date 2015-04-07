using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.Sql.Wpf.Source
{
    /// <summary>
    /// Provides configuration for SQL data source.
    /// </summary>
    public sealed class SqlDataSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="SqlDataSourceAdapterConfigurationProvider" />.
        /// </summary>
        public SqlDataSourceAdapterConfigurationProvider()
            : base(new SqlDataSourceAdapterInternalConfigurationProvider()) { }
    }
}
