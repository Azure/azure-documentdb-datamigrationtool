
namespace Microsoft.DataTransfer.Sql.Source
{
    sealed class SqlDataSourceAdapterInstanceConfiguration : ISqlDataSourceAdapterInstanceConfiguration
    {
        public string ConnectionString { get; set; }
        public string Query { get; set; }
        public string NestingSeparator { get; set; }
    }
}
